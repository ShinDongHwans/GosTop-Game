using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCStatus : Status {
    //몬스터 성격 
    public enum ERelationship {
        Hostile,
        HostileArchor,
        HostileMagic1,
        HostileMagicRandomSeldom,
        HostileMagicRandomOften,
        Neutral,
    }

    //몬스터 액션
    public enum EAction {
        Resting,
        Chasing,
        Fearing,
    }

    public override Vector2Int position {
        set {
            base.position = value;
            if(gaugeBar != null)
                gaugeBar.transform.position = 
                    gameObject.transform.position + new Vector3(0f, -0.5f, -0.01f);
            SetVisibility(GameManager.instance.IsVectorInSight(value));
        }
    }

    public GameObject gaugeBar {
        get;
        internal set;
    }

    public int kind;

    public NPCInfo info {
        get { return NPCInfo.info[kind]; }
    }

    public override string GetName() { return info.name; }

    public override UnitStatus status {
        get { return UnitStatus.CalculateStatusFromStatNPC(info.stat); }
    }

    public ERelationship relationship;
    public EAction actionType;

    public NPCStatus(int x, int y, int kind)
            : base(new Vector2Int(x, y),
                NPCInfo.info[kind].stat,
                NPCInfo.info[kind].resistances) {
        this.kind = kind;
        actionType = EAction.Resting;
        relationship = NPCInfo.info[kind].relationship;
        SetHP(status.maxHP);
    }

    public NPCStatus(Vector2Int v, int kind) : this(v.x, v.y, kind) { }
    public NPCStatus(int kind) : this(0, 0, kind) { }


    /* AI */
    public virtual void Step() {
        switch(relationship) {
            case ERelationship.Neutral: break;
            case ERelationship.Hostile:
                HostileStep();
                break;
            case ERelationship.HostileMagic1:
                HostileMagic1Step();
                break;
            case ERelationship.HostileMagicRandomSeldom:
                HostileMagicRandomSeldomStep();
                break;
            case ERelationship.HostileMagicRandomOften:
                HostileMagicRandomOftenStep();
                break;
            case ERelationship.HostileArchor:
                HostileArchorStep();
                break;
        }
    }

    // --- AI ---

    public void HostileStep() {
        switch(actionType) {
            case EAction.Resting:
                /* DO NOTHING */
                break;
            case EAction.Chasing: SimpleChasing(); break;
            case EAction.Fearing: SimpleFearing(); break;
        }
    }

    public void HostileArchorStep() {
        switch(actionType) {
            case EAction.Resting:
                /* DO NOTHING */
            case EAction.Chasing: {
                var player = GameManager.instance.player;
                if(info.attackRange.IsInRange(player.position, position))
                    /* Atatck */
                    Magic.info[Magic.FindInfoByName("Arrow1")].Invoke(this, player.position);
                else if(Util.MaxDistance(position,player.position) > info.attackRange.to){
                    SimpleChasing();
                }
                else
                    SimpleMoving();
            } break;
            case EAction.Fearing: SimpleFearing(); break;
        }
    }

    public void HostileMagic1Step() {
        switch(actionType) {
            case EAction.Resting:
                /* DO NOTHING */
                break;
            case EAction.Chasing: {
                var player = GameManager.instance.player;
                if(GameManager.instance.IsVectorInSight(position)) {
                    var hpRatio = hp / (float)status.maxHP;
                    if(info.HasMagic("Small Heal") && Random.Range(0f, 1f) <= 0.8f - hpRatio) {
                        Magic.info[Magic.FindInfoByName("Small Heal")].Invoke(this, position);
                    } else if(info.HasMagic("Fire Bolt") && Random.Range(0f, 1f) <= 0.3f) {
                        Magic.info[Magic.FindInfoByName("Fire Bolt")].Invoke(this, player.position);
                    } else {
                        SimpleChasing();
                    }
                } else SimpleChasing();
            } break;
            case EAction.Fearing: SimpleFearing(); break;
        }
    }

    public void HostileMagicRandomSeldomStep()
    {
        switch (actionType)
        {
            case EAction.Resting:
                /* DO NOTHING */
                break;
            case EAction.Chasing:
                {
                    var player = GameManager.instance.player;
                    var hpRatio = hp / (float)status.maxHP;
                    if(info.HasMagic("Small Heal") && Random.Range(0f, 1f) <= 0.5f - hpRatio)
                            {
                        Magic.info[Magic.FindInfoByName("Small Heal")].Invoke(this, position);
                    }
                    else if (GameManager.instance.IsVectorInSight(position))
                    {
                        int[] magiclist = NPCInfo.info[this.kind].magics;
                        int attack_check = 0;
                        if (magiclist.Length != 0)
                        {
                            float count = 0.15f / (magiclist.Length);
                            float probabilty = Random.Range(0f, 1f);
                            //15%의 확률로 가진 마법 중 하나 날림
                            for (int i = 0; i < magiclist.Length; i++)
                            {
                                if (probabilty <= (i + 1) * count)
                                {
                                    Magic.info[magiclist[i]].Invoke(this, player.position);
                                    attack_check++;
                                    break;
                                }
                            }
                        }
                        //나머지 확률로 평타 혹은 이동
                        if (attack_check == 0) {
                            SimpleChasing();
                        }
                        
                    }
                    else SimpleChasing();
                }
                break;
            case EAction.Fearing: SimpleFearing(); break;
        }
    }

    public void HostileMagicRandomOftenStep()
    {
        switch (actionType)
        {
            case EAction.Resting:
                /* DO NOTHING */
                break;
            case EAction.Chasing:
                {
                    var player = GameManager.instance.player;
                    var hpRatio = hp / (float)status.maxHP;
                    if (info.HasMagic("Small Heal") && Random.Range(0f, 1f) <= 0.5f - hpRatio)
                    {
                        Magic.info[Magic.FindInfoByName("Small Heal")].Invoke(this, position);
                    }
                    else if (GameManager.instance.IsVectorInSight(position))
                    {
                        int[] magiclist = NPCInfo.info[this.kind].magics;
                        int attack_check = 0;
                        if (magiclist.Length != 0)
                        {
                            float count = 0.3f / (magiclist.Length);
                            float probabilty = Random.Range(0f, 1f);
                            //30%의 확률로 가진 마법 중 하나 날림
                            for (int i = 0; i < magiclist.Length; i++)
                            {
                                if (probabilty <= (i + 1) * count)
                                {
                                    Magic.info[magiclist[i]].Invoke(this, player.position);
                                    attack_check++;
                                    break;
                                }
                            }
                        }
                        //나머지 확률로 평타 혹은 이동
                        if (attack_check == 0)
                        {
                            SimpleChasing();
                        }

                    }
                    else SimpleChasing();
                }
                break;
            case EAction.Fearing: SimpleFearing(); break;
        }
    }

    public void SimpleChasing() {
        var player = GameManager.instance.player;
        if(info.attackRange.IsInRange(player.position, position)) {
            /* Atatck */
            Attack(player);
        } else {
            var next = FindNextPositionToPlayer();
            if(next != null && player.position != (Vector2Int)next) {
                this.position = (Vector2Int)next;
            }
        }
    }

    public void SimpleFearing() {
        var player = GameManager.instance.player;
        var d = position - player.position;
        var v = new Vector2Int[]{
            new Vector2Int(d.x >= 0 ? 1 : -1, d.y >= 0 ? 1 : -1),
            new Vector2Int(d.x >= 0 ? 1 : -1, 0),
            new Vector2Int(0, d.y >= 0 ? 1 : -1),};
        foreach(var x in v) {
            if(!GameManager.instance.levelManager.currentLevel.tiles[position.x + x.x][position.y + x.y].blocksMove &&
                    GameManager.instance.GetUnitAt(position + x) == null) {
                position += x;
                return;
            }
        }
        actionType = EAction.Chasing;
    }

    public void SimpleMoving() {
        var player = GameManager.instance.player;
        if(info.attackRange.IsInRange(player.position, position)) {
            actionType = EAction.Chasing;
        }
        else{
            var d = position - player.position;
            var v = new Vector2Int[]{
                new Vector2Int(d.x >= 0 ? 1 : -1, d.y >= 0 ? 1 : -1),
                new Vector2Int(d.x >= 0 ? 1 : -1, 0),
                new Vector2Int(0, d.y >= 0 ? 1 : -1),};
            foreach(var x in v) {
                if(!GameManager.instance.levelManager.currentLevel.tiles[position.x + x.x][position.y + x.y].blocksMove &&
                        GameManager.instance.GetUnitAt(position + x) == null) {
                    position += x;
                    return;
                }
            }
            actionType = EAction.Chasing;
        }
    }

    // --- AI END ---

    public virtual void OnFindPlayer() {
        switch(relationship) {
            case ERelationship.Hostile:
            case ERelationship.HostileArchor:
                if(actionType == EAction.Resting)
                    actionType = EAction.Chasing;
                break;
            case ERelationship.HostileMagic1:
                if(actionType == EAction.Resting)
                    actionType = EAction.Chasing;
                break;
            case ERelationship.HostileMagicRandomSeldom:
                if (actionType == EAction.Resting)
                    actionType = EAction.Chasing;
                break;
            case ERelationship.HostileMagicRandomOften:
                if (actionType == EAction.Resting)
                    actionType = EAction.Chasing;
                break;
        }
    }


    /* Move Methods */

    public Vector2Int? FindNextPositionToPlayer() {
        var gameManager = GameManager.instance;
        var levelManager = gameManager.levelManager;
        var player = gameManager.player;
        var path = levelManager.currentLevel.FindPath(position, gameManager.player.position, false);
        if(path == null || levelManager.currentLevel.GetNPCAt(path[0]) != null) {
            path = levelManager.currentLevel.FindPath(position, gameManager.player.position, true);
            if(path == null) return null;
        }
        return path[0];
    }

    /* HP Changing Methods */
    public void SetHP(int hp) {
        this.hp = hp;
        UpdateGaugeBar();
    }

    public override void Heal(int value) { 
        base.Heal(value);
        UpdateGaugeBar();
     }

    public override void Damage(int value) { 
        base.Damage(value);
        UpdateGaugeBar();
     }


    /* Util */
    public void SetGameObject(GameObject gameObject) {
        this.gameObject = gameObject;
        gameObject.GetComponent<NPC>().sprites = NPCInfo.info[kind].sprites;
        this.gaugeBar = GameObject.Instantiate(Resources.Load("Prefabs/Game/UI/GaugeBar")) as GameObject;
        UpdateGaugeBar();
        this.position = position;
    }

    public void DestroyGameObject() {
        if(gameObject) GameObject.Destroy(gameObject);
        if(gaugeBar) GameObject.Destroy(gaugeBar);
        gameObject = null;
        gaugeBar = null;
    }

    public void UpdateGaugeBar() {
        if(gaugeBar == null) return;
        int i;
        var ratio = ((float)hp) / status.maxHP;
        var renderer = gaugeBar.GetComponent<SpriteRenderer>();
        if(ratio >= 1f) i = 0;
        else if(ratio < 0f) i = 15;
        else i = 15 - (int)(ratio * 16);
        renderer.sprite =
                Resources.LoadAll<Sprite>("Sprites/DawnLike/GUI/gauge_4")[i];
    }

    public void SetVisibility(bool isVisible) {
        if(gameObject != null)
            gameObject.GetComponent<SpriteRenderer>().enabled = isVisible;
        if(gaugeBar != null)
            gaugeBar.GetComponent<SpriteRenderer>().enabled = isVisible;
    }

}
