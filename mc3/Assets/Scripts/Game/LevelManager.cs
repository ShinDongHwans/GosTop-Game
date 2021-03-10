using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public const int lastFloor = 15;


    public const float zTile = 2f;
    public const float zDeco = 1.5f;
    public const float zShadow = -5f;
    public const float zItem = 1f;
    public const float zNPC = 0.5f;

    
    
    public List<Level>[] levels;
    public Level currentLevel {
        get; internal set;
    }
    public int theme {
        get; internal set;
    }
    public static int[] themeFloor = new int[] {-1, 4, 6, 8};
    public int currentLevelIndex {
        get; internal set;
    }
    GameObject topObject;
    List<GameObject> currentLevelStaticObjects;

    bool[][] currentLightMap;
    Texture2D shadowTexture = null;
    Sprite shadowSprite;


    public LevelManager() : base() {
        instance = this;
        levels = new List<Level>[] {
            new List<Level>(),
            new List<Level>(),
            new List<Level>(),
            new List<Level>()
        };
        currentLevel = null;
        theme = 0;
        currentLevelIndex = -1;
    }
    void Awake() {
        ChangeLevel(0);
    }

    /* --- Generate/Level Change --- */
    public void ChangeLevel(int level) {
        /* Generate Levels */
        while(level >= levels[theme].Count) {
            int upSt = levels[theme].Count == 0 ? 1 : levels[theme][levels[theme].Count - 1].downStairs.Length;
            int downSt = levels[theme].Count + 1 == lastFloor? 1 : Random.Range(1, 2);
            if (theme > 0 && levels[theme].Count + 1 == 5) downSt = 0;
            Level newLevel = null;
            switch(theme) {
                case 0: {
                    int portal = -1;
                    for(int i = 1; i < themeFloor.Length; i++) {
                        if(themeFloor[i] == levels[theme].Count) { portal = i; break; }
                    }
                    newLevel = MainLevelGenerator.generate(20 + levels[0].Count, upSt, downSt, portal, levels[0].Count);
                    if(portal >= 0) {
                        Level themeLevel = null;
                        switch(portal) {
                            case 1:
                                themeLevel = WoodlandLevelGenerator.generate(40 + levels[1].Count, 42 + levels[1].Count, 0, downSt, themeFloor[portal], 0);
                                break;
                            case 2:
                                themeLevel = LavalandLevelGenerator.generate(40 + levels[2].Count, 40 + levels[2].Count, 0, downSt, themeFloor[portal], 0);
                                break;
                            case 3:
                                themeLevel = SnowlandLevelGenerator.generate(40 + levels[3].Count, 40 + levels[3].Count, 0, downSt, themeFloor[portal], 0);
                                break;
                        }
                        levels[portal].Add(themeLevel);
                    }
                } break;
                case 1:
                    newLevel = WoodlandLevelGenerator.generate(30 + levels[1].Count, 32 + levels[1].Count, upSt, downSt, -1, levels[1].Count);
                    break;
                case 2:
                    newLevel = LavalandLevelGenerator.generate(35 + levels[2].Count, 35 + levels[2].Count, upSt, downSt, -1, levels[2].Count);
                    break;
                case 3:
                    newLevel = SnowlandLevelGenerator.generate(35 + levels[3].Count, 35 + levels[3].Count, upSt, downSt, -1, levels[3].Count);
                    break;
            }
            levels[theme].Add(newLevel);
        }
        if(currentLevel != null) {
            DestroyCurrentLevel();
        }
        currentLevel = levels[theme][level];
        currentLevelIndex = level;
        InstantiateLevel(currentLevel);
        InitializeShadow();
    }

    public Vector2Int? ChangeLevelUsingStair(Vector2Int pos) {
        var tile = currentLevel.tiles[pos.x][pos.y];
        if(tile.isPortal) {
            Vector2Int newPos;
            if(theme == 0) {
                newPos = (Vector2Int)levels[tile.decoData][0].portal;
                theme = tile.decoData;
                ChangeLevel(0);
            } else {
                newPos = (Vector2Int)levels[0][tile.decoData].portal;
                theme = 0;
                ChangeLevel(tile.decoData);
            }
            return newPos;
        } else if(tile.isStair) {
            Vector2Int newPos;
            if(tile.isUpStair) {
                if(currentLevelIndex == 0) return null;
                ChangeLevel(currentLevelIndex - 1);
                newPos = levels[theme][currentLevelIndex].downStairs[tile.decoData];
            } else {
                ChangeLevel(currentLevelIndex + 1);
                newPos = levels[theme][currentLevelIndex].upStairs[tile.decoData];
            }
            return newPos;
        } else return null;
    }


    /* --- Instantiate --- */
    private void InstantiateLevel(Level level) {
        var tilePrefab = Resources.Load<GameObject>("Prefabs/Game/Tile/Tile");
        var npcPrefab = Resources.Load<GameObject>("Prefabs/Game/Unit/NPC");

        


        topObject = new GameObject();
        topObject.name = "-Level-";
        currentLevelStaticObjects = new List<GameObject>();


        // Prerendering
        /*
        level.Prerendering();
        for(int i = 0; i < level.preWidth; i++) {
            for(int j = 0; j < level.preHeight; j++) {
                var t = Instantiate(tilePrefab, new Vector3(i * Level.preS - 0.5f, j * Level.preS - 0.5f, zTile), new Quaternion());
                t.transform.SetParent(topObject.transform, false);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = level.prerendered[i * level.preHeight + j];
                currentLevelStaticObjects.Add(t);

                for(int k = i * Level.preS; k < (i + 1) * Level.preS && k < level.width; k++) {
                    for(int l = j * Level.preS; l < (j + 1) * Level.preS && l < level.height; l++) {
                        if(level.tiles[k][l].deco != null) {
                            var d = Instantiate(tilePrefab, new Vector3(k, l, zDeco), new Quaternion());
                            d.transform.SetParent(topObject.transform, false);
                            var dRenderer = d.GetComponent<SpriteRenderer>();
                            dRenderer.sprite = level.tiles[k][l].deco;
                            currentLevelStaticObjects.Add(d);
                        }
                    }
                }
            }
        } */

        for(int i = 0; i < level.width; i++) {
            for(int j = 0; j < level.height; j++) {
                var t = Instantiate(tilePrefab, new Vector3(i, j, zTile), new Quaternion());
                t.transform.SetParent(topObject.transform, false);
                var renderer = t.GetComponent<SpriteRenderer>();
                renderer.sprite = level.tiles[i][j].sprite;
                currentLevelStaticObjects.Add(t);
                level.tiles[i][j].tileObject = t;
                if(level.tiles[i][j].deco != null) {
                    var d = Instantiate(tilePrefab, new Vector3(i, j, zDeco), new Quaternion());
                    d.transform.SetParent(topObject.transform, false);
                    var dRenderer = d.GetComponent<SpriteRenderer>();
                    dRenderer.sprite = level.tiles[i][j].deco;
                    currentLevelStaticObjects.Add(d);
                    level.tiles[i][j].decoObject = d;
                }
            }
        }

        /* Objects */

        foreach(var i in level.npcs) {
            var d = Instantiate(npcPrefab,
                new Vector3(i.position.x, i.position.y, zNPC), 
                new Quaternion());
            d.transform.SetParent(topObject.transform, false);
            i.SetGameObject(d);
        }

        foreach(var i in level.items) {
            InstantiateDroppedItem(i);
        }
    }

    private void InstantiateDroppedItem(DroppedItem di) {
        var tilePrefab = Resources.Load<GameObject>("Prefabs/Game/Tile/Tile");
        var d = Instantiate(tilePrefab,
                new Vector3(di.position.x, di.position.y, zItem), 
                new Quaternion());
        d.transform.SetParent(topObject.transform, false);
        var dRenderer = d.GetComponent<SpriteRenderer>();
        dRenderer.sprite = ItemInfo.info[di.item.kind].sprite;
        di.gameObject = d;
    }

    public void DestroyCurrentLevel() {
        foreach(var x in currentLevelStaticObjects) {
            Destroy(x);
        }

        foreach(var npc in currentLevel.npcs) {
            npc.DestroyGameObject();
        }

        foreach(var item in currentLevel.items) {
            if(item.gameObject) GameObject.Destroy(item.gameObject);
            item.gameObject = null;
        }
        Destroy(topObject);
        topObject = null;
    }


    /* --- Shadow --- */

    const float SHADOW_ALPHA = 1f;
    public void InitializeShadow() {
        var black = new Color(0.0f, 0.0f, 0.0f, SHADOW_ALPHA);
        var half = new Color(0.0f, 0.0f, 0.0f, SHADOW_ALPHA * 0.5f);
        shadowTexture = new Texture2D(currentLevel.width * 2 + 2, currentLevel.height * 2 + 2);
        for(int i = 0; i < currentLevel.width * 2 + 2; i++) {
            for(int j = 0; j < currentLevel.height * 2 + 2; j++) {
                if(IsVectorVisited(new Vector2Int((i - 1) >> 1, (j - 1) >> 1)))
                    shadowTexture.SetPixel(i, j, half);
                else
                    shadowTexture.SetPixel(i, j, black);
            }
        }

        // shadowTexture.filterMode = FilterMode.Point;

        shadowTexture.Apply();
        shadowSprite = Sprite.Create(shadowTexture,
            new Rect(0, 0, currentLevel.width * 2 + 2, currentLevel.height * 2 + 2),
            new Vector2(0f, 0f),
            2f);
    }

    public void UpdateShadow(Vector2Int position, Vector2Int lastPosition) {
        currentLightMap = currentLevel.CalculateShadow(position);
        UpdateObjectsVisibility(position);
        InstantiateShadow(position, lastPosition);
    }

    public void UpdateObjectsVisibility(Vector2Int position) {
        foreach(DroppedItem i in currentLevel.items) {
            if(i.gameObject != null) {
                i.gameObject.GetComponent<Renderer>().enabled =
                    IsVectorInSight(i.position, position);
            }
        }

        foreach(NPCStatus i in currentLevel.npcs) {
            i.SetVisibility(IsVectorInSight(i.position, position));
        }
    }

    private void InstantiateShadow(Vector2Int position, Vector2Int lastPosition) {
        var black = new Color(0.0f, 0.0f, 0.0f, SHADOW_ALPHA);
        var half = new Color(0.0f, 0.0f, 0.0f, SHADOW_ALPHA * 0.5f);
        var trans = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        Color color;
        
        /* Remove Last */
        for(int i = 0; i < Level.SHADOW_SIZE2; i++) {
            for(int j = 0; j < Level.SHADOW_SIZE2; j++) {
                Vector2Int realP = lastPosition + new Vector2Int(i - Level.SHADOW_SIZE, j - Level.SHADOW_SIZE);
                if(currentLevel.IsVectorInMap(realP)) {
                    if(currentLevel.visitedMap[realP.x][realP.y]) color = half;
                    else color = black;
                    for(int k = 0; k < 2; k++) {
                        for(int l = 0; l < 2; l++) {
                            shadowTexture.SetPixel(1 + realP.x * 2 + k, 1 + realP.y * 2 + l, color);
                        }
                    }
                }
            }
        }

        /* Update Current */
        for(int i = 0; i < Level.SHADOW_SIZE2; i++) {
            for(int j = 0; j < Level.SHADOW_SIZE2; j++) {
                Vector2Int realP = position + new Vector2Int(i - Level.SHADOW_SIZE, j - Level.SHADOW_SIZE);
                if(currentLevel.IsVectorInMap(realP)) {
                    if(!currentLevel.visitedMap[realP.x][realP.y]) color = black;
                    else if(currentLightMap[i][j]) color = trans;
                    else color = half;
                    for(int k = 0; k < 2; k++) {
                        for(int l = 0; l < 2; l++) {
                            shadowTexture.SetPixel(1 + realP.x * 2 + k, 1 + realP.y * 2 + l, color);
                        }
                    }
                }
            }
        }

        shadowTexture.Apply();
        
        var s = GameObject.Find("Shadow");
        s.GetComponent<SpriteRenderer>().sprite = shadowSprite;
        s.transform.position = new Vector3(-1f, -1f, zShadow);
    }



    public void AddDroppedItem(Vector2Int position, Item item) {
        var di = new DroppedItem(position, item);
        currentLevel.items.Add(di);
        InstantiateDroppedItem(di);
    }
    
    public void RemoveDroppedItem(DroppedItem di) {
        if(di.gameObject) {
            GameObject.Destroy(di.gameObject);
        }
        currentLevel.items.Remove(di);
    }

    public void RemoveNPCOfIndex(int idx) {
        var npc = currentLevel.npcs[idx];
        if(npc.gameObject) {
            GameObject.Destroy(npc.gameObject);
            GameObject.Destroy(npc.gaugeBar);
        }
        currentLevel.npcs.RemoveAt(idx);
    }

    public bool IsVectorInSight(Vector2Int point, Vector2Int player) {
        if(!currentLevel.IsVectorInMap(point)) return false;
        Vector2Int d = point - player;
        int x = d.x + Level.SHADOW_SIZE;
        int y = d.y + Level.SHADOW_SIZE;
        return 0 <= x && x < Level.SHADOW_SIZE2 && 0 <= y && y < Level.SHADOW_SIZE2 && currentLightMap[x][y];
    }

    public bool IsVectorVisited(Vector2Int point) {
        if(!currentLevel.IsVectorInMap(point)) return false;
        return currentLevel.visitedMap[point.x][point.y];
    }

    public bool IsEnemyInSight(Vector2Int player) {
        foreach(var npc in currentLevel.npcs) {
            if(IsVectorInSight(npc.position, player)) return true;
        }
        return false;
    }

    public void MarkAllVisited() {
        currentLevel.MarkAllVisited();
        for(int i = 1; i < currentLevel.width - 1; i += 5) {
            for(int j = 1; j < currentLevel.height - 1; j += 5) {
                InstantiateShadow(GameManager.instance.player.position, new Vector2Int(i, j));
            }
        }
    }
}
