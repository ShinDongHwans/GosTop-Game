using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    
    /* --- 현재 게임 상태 관련 properties --- */
    public LevelManager levelManager {
        get; internal set;
    }
    public PlayerStatus player {
        get; internal set;
    }

    /* --- 플레이어 행동 관련 --- */
    public class PlayerAction {
        public enum EType {
            Rest, Move, Attack, PickUpItems
        }
        
        public EType type;
        public Vector2Int position;

        public PlayerAction(EType type, Vector2Int position) {
            this.type = type;
            this.position = position;
        }
    }
    
    /* 플레이어의 예약된 행동 목록. [0]이 바로 다음 행동 */
    private List<PlayerAction> playerToDo;
    private bool isPlayerToDoEmpty {
        get { return playerToDo == null || playerToDo.Count <= 0; }
    }
    
    /* 플레이어 외의 NPC들 중 다음 턴에 움직이기로 예약된 녀석들 */
    private List<NPCStatus> npcsToDo;
    private bool isNPCToDoEmpty {
        get { return npcsToDo == null || npcsToDo.Count <= 0; }
    }

    /* 현재 플레이어가 행동할 차례인지 */
    public bool isPlayerTurn {
        get { return isPlayerToDoEmpty && isNPCToDoEmpty; }
    }

    /* 플레이어, 시야의 NPC의 행동들을 실행시키는 간격을 재는 타이머 */
    public const float actionInterval = 0.15f;
    private float actionTimer = float.NaN;
    

    /* 카메라가 플레이어를 쫓는지 */
    public bool cameraTrackPlayer;

    /* --- 현재 게임이 어떤 상태인지. --- */
    public enum GameStatus{
        PLAYING, /* 일반적인 상태 */
        INVOKING, /* 마법 발동을 위해 타겟을 설정중 */        
        FINISHED, /* 이미 게임 오버 되었음 */
    }
    private GameStatus _status;
    public GameStatus status {
        get { return _status; }
        set { if(_status != GameStatus.FINISHED) _status = value; }
    }
    public Magic invokingMagic;
    public List<Event> events;



    /* ----------- Methods ----------- */

    public GameManager() : base() {
        instance = this;
    }
    
    void Awake()
    {
        Screen.SetResolution(Screen.width, Screen.height, false);
        levelManager = LevelManager.instance;
        InitializePlayer();
        playerToDo = null;
        npcsToDo = null;
        cameraTrackPlayer = true;
        events = new List<Event>();
    }

    void Start() {
        Toast.Make("hello!");
        status = GameStatus.PLAYING;
        Application.targetFrameRate = 60;
    }

    void Update() {
        HandleEvent();

        if(events.Count > 0) { /* Events exist */
            for(int i = events.Count; --i >= 0;) {
                if(!events[i].RunStep()) {
                    /* Event Finished */
                    events.RemoveAt(i);
                }
            }
        } else if(HandleActions()) {
            levelManager.UpdateObjectsVisibility(player.position);
        }

        if(cameraTrackPlayer) MoveCameraToPlayer();
    }
    
    void OnClick(Vector2Int tilePosition) {
        /* To do something */
        switch(status) {
        case GameStatus.FINISHED: /* Already Game */
            MakeReport();
            SceneManager.LoadScene("GameOverScene");
            break;
        case GameStatus.INVOKING: {
            if(!levelManager.IsVectorVisited(tilePosition)) return;
            if(!PassTurnOfPlayer(1)) {
                Toast.Make("It's not your turn", 0.5f);
            }
            if(invokingMagic == null) {
                Log.Make("Nothing occurred");
            } else {
                invokingMagic.Invoke(player, tilePosition);
            }
            status = GameStatus.PLAYING;
        } break;
        case GameStatus.PLAYING: {
            if(!levelManager.IsVectorVisited(tilePosition)) return;
            var lastPosition = player.position;
            if(lastPosition != tilePosition) {
                if(levelManager.IsVectorInSight(tilePosition, player.position) &&
                    GetUnitAt(tilePosition) != null) {
                    /* Attack */
                    ReserveAttack(tilePosition);
                } else {
                    /* Move */
                    var path = levelManager.currentLevel.FindPath(lastPosition, tilePosition, true);
                    if(path != null) {
                        ReservePlayerMove(path);
                        //ShowPath(lastPosition, path);
                    }
                }
            } else {
                /* Clicked Player */
                var tile = levelManager.currentLevel.tiles[player.position.x][player.position.y];
                if(tile.isStair) {
                    if(levelManager.currentLevelIndex == 0 && tile.isUpStair) FinishGame("Escaped");
                    else if(levelManager.currentLevelIndex == LevelManager.lastFloor - 1 && tile.isDownStair) FinishGame("Clear");
                    else {
                        Vector2Int newPos = (Vector2Int)levelManager.ChangeLevelUsingStair(player.position);
                        MovePlayerTo(newPos);
                        Toast.Make("Floor " + (levelManager.currentLevelIndex + 1));
                    }
                } else if(tile.isPortal) {
                    Vector2Int newPos = (Vector2Int)levelManager.ChangeLevelUsingStair(player.position);
                        MovePlayerTo(newPos);
                    Toast.Make("Floor " + (levelManager.currentLevelIndex + 1));
                } else ReserveSimpleAction(PlayerAction.EType.PickUpItems);
            }
        } break;
        }

        
    }


    /* Player */
    void InitializePlayer() {
        player = new PlayerStatus();
        player.gameObject = GameObject.Find("Player");
        var spriteRenderer = player.gameObject.GetComponent<SpriteRenderer>();
        var sprites = Resources.LoadAll<Sprite>("Sprites/");
        player.gameObject.GetComponent<Player>().sprites =
                PlayerStatus.JOB_DEFAULT_SETTINGS[PlayerStatus.startingJob].sprites;

        MovePlayerTo(levelManager.currentLevel.upStairs[0]);
    }

    public void MovePlayerTo(Vector2Int p) {
        var lastPosition = player.position;
        player.position = p;
        levelManager.UpdateShadow(p, lastPosition);
        cameraTrackPlayer = true;
    }

    void PickUpItems() {
        List<DroppedItem> items = levelManager.currentLevel.GetItemsAt(player.position);
        if(items.Count <= 0) return;
        DroppedItem di = items[0];
        bool pickedUp = false;
        string name = di.item.GetName();
        if(di.item is Consumable) {
            int i;
            for(i = 0; i < player.inventory.Count; i++) {
                if(player.inventory[i].kind == di.item.kind) {
                    ((Consumable)player.inventory[i]).count += ((Consumable)di.item).count;
                    levelManager.RemoveDroppedItem(di);
                    Log.Make("You pick up " + name);
                    pickedUp = true;
                    break;
                }
            }
        }
        if(!pickedUp) {
            if(player.inventory.Count < player.bag * PlayerStatus.BAG_SIZE) {
                player.inventory.Add(di.item);
                levelManager.RemoveDroppedItem(di);
                Log.Make("You pick up " + name);
                pickedUp = true;
            } else {
                Log.Make("Your bags are full");
            }
        }
    }

    public bool DropItem(int index, int count = -1) {
        if(!PassTurnOfPlayer(1)) return false;
        Item _item = player.inventory[index];
        string name = null;
        if(_item is Consumable) {
            var item = _item as Consumable;
            if(count <= 0 || count == item.count) {
                name = _item.GetName();
                levelManager.AddDroppedItem(player.position, _item);
                player.inventory.RemoveAt(index);
            } else {
                var di = new Consumable(item.kind, count);
                name = item.GetName();
                levelManager.AddDroppedItem(player.position, di);
                item.count -= count;
            }
        } else {
            name = _item.GetName();
            levelManager.AddDroppedItem(player.position, _item);
            player.inventory.RemoveAt(index);
        }
        if(name != null) {
            Log.Make("You dropped " + name);
        }
        return true;
    }

    /* Action Timer Handling */
    public bool HandleActions() {
        /* Check Timing */
        if(float.IsNaN(actionTimer)) return false;
        actionTimer -= Time.deltaTime;
        if(actionTimer > 0f) return false;
        actionTimer += actionInterval;

        /* Do Stacked Actions */
        bool inSight = false;
        
        if(npcsToDo != null && npcsToDo.Count != 0) {
            /* NPC should do something */
            NPCStatus npc;
            Vector2Int pos;
            do {
                var last = npcsToDo.Count - 1;
                npc = npcsToDo[last];
                pos = npc.position;
                npcsToDo.RemoveAt(last);
                npc.Step();
                inSight = levelManager.IsVectorInSight(pos, player.position);
            } while(npcsToDo.Count > 0 && !inSight);
        }

        bool result = false;
        if(!inSight && playerToDo != null && playerToDo.Count != 0) {
            result = true;
            if(playerToDo.Capacity > 1 && levelManager.IsEnemyInSight(player.position)) {
                CancelReservedPlayerActions();
                result = false;
            } else {
                /* Player will do reserved actions */
                var last = playerToDo.Count - 1;
                PlayerAction action = playerToDo[last];
                playerToDo.RemoveAt(last);
                switch(action.type) {
                    case PlayerAction.EType.Rest:
                        player.Damage(-1);
                        break;
                    case PlayerAction.EType.Move:
                        MovePlayerTo(action.position);
                        break;
                    case PlayerAction.EType.Attack:
                        /* Check Range */
                        if(player.attackRange.IsInRange(action.position, player.position)) {
                            var npc = levelManager.currentLevel.GetNPCAt(action.position);
                            player.Attack(npc);
                        } else {
                            Toast.Make("Out of range");
                            CancelReservedPlayerActions();
                            result = false;
                        }
                        break;
                    case PlayerAction.EType.PickUpItems:
                        PickUpItems();
                        break;
                }
            }
        } else CancelReservedPlayerActions();

        result &= UpdateUnitsStatus();

        if(result) {
            player.OnTurnEnd();
            /* reserveNPCs */
            ReserveNPCs();
        }

        return result;
    }

    public bool UpdateUnitsStatus() {
        if(player.hp <= 0) {
            FinishGame("You Died");
            return false;
        }

        /* Update NPCs */
        for(int i = levelManager.currentLevel.npcs.Count; --i >= 0;) {
            var npc = levelManager.currentLevel.npcs[i];
            if(npc.hp <= 0) { /* Dead */
                player.GainExp(npc.info.exp);
                if(npc.info.dropList != null) {
                    foreach(var x in npc.info.dropList) {
                        if(Random.Range(0f, 1f) <= x.second) {
                            var info = ItemInfo.info[x.first];
                            if(info is EquipmentInfo) {
                                levelManager.AddDroppedItem(npc.position, new Equipment(x.first));
                            } else {
                                levelManager.AddDroppedItem(npc.position, new Consumable(x.first));
                            }
                            
                        }
                    }
                }
                levelManager.RemoveNPCOfIndex(i);
                Log.Make(npc.GetName() + " died");
            } else {
                if(levelManager.IsVectorInSight(npc.position, player.position)) {
                    npc.OnFindPlayer();
                }
            }
        }
        return true;
    }



    private bool ReservePlayerActions(List<PlayerAction> actions) {
        if(isPlayerTurn) {
            playerToDo = new List<PlayerAction>(actions);
            if(npcsToDo == null || npcsToDo.Count == 0) {
                actionTimer = 0f;
            }
            return true;
        } else {
            Toast.Make("It's not your turn", 0.5f);
            return false;
        }
    }

    public bool ReservePlayerMove(List<Vector2Int> movements) {
        List<PlayerAction> list;
        if(levelManager.IsEnemyInSight(player.position)) {
            list = new List<PlayerAction>(1);
            list.Add(new PlayerAction(PlayerAction.EType.Move, movements[0]));
        } else {
            list = new List<PlayerAction>(movements.Count);
            for(int i = movements.Count; --i >= 0;) {
                list.Add(new PlayerAction(PlayerAction.EType.Move, movements[i]));
            }
        }
        return ReservePlayerActions(list);
    }

    public bool ReserveAttack(Vector2Int pos) {
        List<PlayerAction> list = new List<PlayerAction>(1);
        list.Add(new PlayerAction(PlayerAction.EType.Attack, pos));
        return ReservePlayerActions(list);
    }

    public void CancelReservedPlayerActions() {
        playerToDo = null;
        if(npcsToDo == null || npcsToDo.Count == 0)
            actionTimer = float.NaN;
    }

    public bool PassTurnOfPlayer(int n) {
        var list = new List<PlayerAction>();
        for(int i = 0; i < n; i++)
            list.Add(new PlayerAction(PlayerAction.EType.Rest, player.position));
        return ReservePlayerActions(list);
    }

    public bool ReserveSimpleAction(PlayerAction.EType type) {
        var list = new List<PlayerAction>(1);
        list.Add(new PlayerAction(type, player.position));
        return ReservePlayerActions(list);
    }



    public void ReserveNPCs() {
        npcsToDo = new List<NPCStatus>(levelManager.currentLevel.npcs);
    }


    public void ReserveEvent(Event ev) {
        events.Add(ev);
        ev.StartEvent();
    }


    /* Invoke */
    public bool TryInvokeMagic(int id) {
        if(!isPlayerTurn) {
            Toast.Make("It's not your turn", 0.5f);
            return false;
        }
        if(status == GameStatus.INVOKING) {
            status = GameStatus.PLAYING;
            Toast.Make("Invoking Canceled", 1f);
            return true;
        } else {
            invokingMagic = id >= 0 ? Magic.info[id] : null;
            status = GameStatus.INVOKING;
            Toast.Make("Pick the target", 1f);
            return true;
        }
    }

    public bool TryInvokeItem(Item item) {
        if(!isPlayerTurn) {
            Toast.Make("It's not your turn", 0.5f);
            return false;
        } else if(item is Equipment) {
            var e = item as Equipment;
            if(e.charge <= 0) {
                return TryInvokeMagic(-1);
            } else {
                e.charge--;
                return TryInvokeMagic(item.info.magic);
            }
        } else {
            return TryInvokeMagic(item.info.magic);
        }
    }


    /* Util */
    public Status GetUnitAt(Vector2Int position) {
        if(position == player.position) {
            return player;
        } else {
            return levelManager.currentLevel.GetNPCAt(position);
        }
    }

    public List<NPCStatus> GetNPCsInSight() {
        var list = new List<NPCStatus>();
        foreach(var npc in levelManager.currentLevel.npcs) {
            if(IsVectorInSight(npc.position)) {
                list.Add(npc);
            }
        }
        return list;
    }

    public Status GetNearestUnitOnLine(Vector2Int target, Vector2Int origin) {
        return levelManager.currentLevel.GetNearestUnitOnLine(target, origin, player.position);
    }

    public bool IsVectorInSight(Vector2Int v) {
        if(player == null || levelManager == null) return false;
        return levelManager.IsVectorInSight(v, player.position);
    }
    

    /* Camera */
    void MoveCameraToPlayer() {
        var cp = Camera.main.transform.position;
        var pp = player.position;
        Camera.main.transform.position =
            new Vector3((2 * cp.x + (float)pp.x) / 3, (2 * cp.y + (float)pp.y) / 3, -10f);
    }

    /* Event Handling */
    int pointerDown = 0;
    Vector3 cameraFirst;
    Vector3 pointerFirst;
    void HandleEvent() {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            pointerDown = 1;
            cameraFirst = Camera.main.transform.position;
            pointerFirst = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0)) {
            var p = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if(pointerDown == 1) {
                if((pointerFirst - p).sqrMagnitude > 0.003f) {
                    pointerDown = 2;
                    cameraTrackPlayer = false;
                }
            } else if(pointerDown == 2) {
                var scale = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)) - Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
                var d = (pointerFirst - p);
                d.Scale(scale);
                Camera.main.transform.position = cameraFirst + d;
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            if(pointerDown == 1) {
                /* DO Something */
                var v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                OnClick(new Vector2Int((int)(v.x + 0.5f), (int)(v.y + 0.5f)));
            }
            pointerDown = 0;
        }
#else
        if(Input.touchCount == 1) {
            switch(Input.GetTouch(0).phase) {
                case TouchPhase.Began: if(!EventSystem.current.IsPointerOverGameObject(0)) {
                    pointerDown = 1;
                    cameraFirst = Camera.main.transform.position;
                    pointerFirst = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
                } break;
                case TouchPhase.Moved: {
                    var p = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    if(pointerDown == 1) {
                        if((pointerFirst - p).sqrMagnitude > 0.003f) {
                            pointerDown = 2;
                            cameraTrackPlayer = false;
                        }
                    } else if(pointerDown == 2) {
                        var scale = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f)) - Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
                        var d = (pointerFirst - p);
                        d.Scale(scale);
                        Camera.main.transform.position = cameraFirst + d;
                    }
                } break;
                case TouchPhase.Ended: {
                    if(pointerDown == 1) {
                        /* DO Something */
                        var v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        OnClick(new Vector2Int((int)(v.x + 0.5f), (int)(v.y + 0.5f)));
                    }
                    pointerDown = 0;
                } break;
            }
        } else if(Input.touchCount == 2 &&
                !EventSystem.current.IsPointerOverGameObject(0) &&
                !EventSystem.current.IsPointerOverGameObject(1)) {
            var p0 = Input.GetTouch(0).position;
            var d0 = Input.GetTouch(0).deltaPosition;
            var o0 = p0 - d0;
            var p1 = Input.GetTouch(1).position;
            var d1 = Input.GetTouch(1).deltaPosition;
            var o1 = p1 - d1;
            var curDist = p1 - p0;
            var prevDist = o1 - o0;
            Camera.main.orthographicSize *= Mathf.Sqrt(prevDist.sqrMagnitude / curDist.sqrMagnitude);
            pointerDown = 0;
        }
#endif
    }



    /* Finishing Game */
    public void FinishGame(string result) {
        status = GameStatus.FINISHED;
        Report.result = result;
        Toast.Make(result, 60);
    }
    
    public void MakeReport() {
        Report.floor = levelManager.levels[0].Count;
        Report.playerLevel = player.level;
        Report.playerHP = player.hp;
    }


    /* For Debug */
    private void ShowPath(Vector2Int lastPosition, List<Vector2Int> path) {
        var pathObj = GameObject.Find("PathLine");
        if(pathObj == null) {
            pathObj = new GameObject();
            pathObj.AddComponent<LineRenderer>();
        }
        var renderer = pathObj.GetComponent<LineRenderer>();
        var positions = new Vector3[path.Count + 1];
        for(int i = 0; i < path.Count; i++) {
            positions[i + 1] = new Vector3(path[i].x, path[i].y, -5f);
        }
        positions[0] = new Vector3(lastPosition.x, lastPosition.y, -5f);
        renderer.positionCount = positions.Length;
        renderer.SetPositions(positions);
        renderer.startWidth = 0.1f;
        renderer.endWidth = 0.1f;
        renderer.startColor = Color.blue;
        renderer.endColor = Color.blue;
        pathObj.name = "PathLine";
    }

    private void ShowAttackRange() {
        var p = player.position;
        var attackRange = player.attackRange;
        var rangeObj = GameObject.Find("RangeObject");
        if(rangeObj == null) {
            rangeObj = new GameObject();
            rangeObj.AddComponent<LineRenderer>();
            rangeObj.name = "RangeObject";
        }
        var renderer = rangeObj.GetComponent<LineRenderer>();
        renderer.startColor = renderer.endColor = new Color(1f, 0f, 0f, 0.5f);
        renderer.material.SetColor("_TintColor", new Color(1f, 0f, 0f, 0.5f));
        renderer.startWidth = renderer.endWidth = (attackRange.to - attackRange.from + 1) * 0.5f;
        var d = (attackRange.to + attackRange.from + 1) * 0.5f;
        renderer.positionCount = 5;
        renderer.SetPositions(new Vector3[] {
            new Vector3(p.x - d, p.y - d, -5f),
            new Vector3(p.x + d, p.y - d, -5f),
            new Vector3(p.x + d, p.y + d, -5f),
            new Vector3(p.x - d, p.y + d, -5f),
            new Vector3(p.x - d, p.y - d, -5f),
        });
    }
}
