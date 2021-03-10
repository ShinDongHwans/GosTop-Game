using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodlandLevelGenerator {

    public static Theme theme;

    static int npcNumber;
    static Tuple<int, float>[] npcList;

    static int itemNumber;
    static Tuple<int, float>[] itemList;

    public static Level generate(int width, int height, int upStairNum, int downStairNum, int portal = -1, int floor = 0) {
        theme = WoodlandTheme.instance;
        var level = new Level(theme, width, height);
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                dig(theme, level, i, j);
            }
        }

        int groundCount = width * height;

        for(int i = 0; i < width; i++) {
            int l = Random.Range(1, 4);
            for(int j = 0; j < l; j++) {
                if(!level.tiles[i][j].blocksMove) groundCount--;
                wall(theme, level, i, j);
            }
            l = Random.Range(1, 4);
            for(int j = 0; j < l; j++) {
                if(!level.tiles[i][height - j - 1].blocksMove) groundCount--;
                wall(theme, level, i, height - j - 1);
            }
        }

        for(int i = 0; i < height; i++) {
            int l = Random.Range(1, 4);
            for(int j = 0; j < l; j++) {
                if(!level.tiles[j][i].blocksMove) groundCount--;
                wall(theme, level, j, i);
            }
            l = Random.Range(1, 4);
            for(int j = 0; j < l; j++) {
                if(!level.tiles[width - j - 1][i].blocksMove) groundCount--;
                wall(theme, level, width - j - 1, i);
            }
        }

        level.groundNumber = groundCount;

        int portalNum = portal >= 0 ? 1 : 0;
        var stairs = new int[upStairNum + downStairNum + portalNum];
        level.upStairs = new Vector2Int[upStairNum];
        level.downStairs = new Vector2Int[downStairNum];

        for(int i = 0; i < stairs.Length; i++) {
            stairs[i] = Random.Range(0, groundCount);
            for(int j = 0; j < i; j++) {
                if(stairs[i] == stairs[j]) {
                    stairs[i] = (stairs[i] + 1) % groundCount;
                    j = -1;
                }
            }
        }
        
        for(int i = 0; i < level.width; i++) {
            for(int j = 0; j < level.height; j++) {
                if(!level.tiles[i][j].blocksMove) {
                    groundCount--;
                    for(int k = 0; k < stairs.Length; k++) {
                        if(groundCount == stairs[k]) {
                            if(k < upStairNum) {
                                level.upStairs[k] = new Vector2Int(i, j);
                                upStair(theme, level, i, j);
                                level.tiles[i][j].decoData = k;
                            } else if(k - upStairNum < downStairNum) {
                                level.downStairs[k - upStairNum] = new Vector2Int(i, j);
                                downStair(theme, level, i, j);
                                level.tiles[i][j].decoData = k - upStairNum;
                            } else {
                                level.portal = new Vector2Int(i, j);
                                makePortal(theme, level, i, j);
                                level.tiles[i][j].decoData = portal;
                            }
                        }
                    }
                }
            }
        }


        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(level.tiles[i][j].decoType == 0) {
                    if(!level.tiles[i][j].blocksMove && !level.tiles[i][j].isTransport) {
                        if(Random.Range(0f, 1f) <= 0.8f) {
                            deco(theme, level, i, j);
                        }
                    }
                }
            }
        }

        npcNumber = 12 + (width + height) / 8;
        npcList = new Tuple<int, float>[] {
            new Tuple<int, float>(NPCInfo.FindInfoByName("Unicorn"), 1f),
            new Tuple<int, float>(NPCInfo.FindInfoByName("Snake"), 1f),
            new Tuple<int, float>(NPCInfo.FindInfoByName("Dark Sheep"), 1f),
            new Tuple<int, float>(NPCInfo.FindInfoByName("Deer"), 1f),
            new Tuple<int, float>(NPCInfo.FindInfoByName("Elephant"), 1f),
            new Tuple<int, float>(NPCInfo.FindInfoByName("Plant"), 1f),
            new Tuple<int, float>(NPCInfo.FindInfoByName("Plant Humanoid"), 1f),
        };

        itemNumber = 10 + (width + height) / 8;
        itemList = new Tuple<int, float>[] {
            new Tuple<int, float>(ItemInfo.FindInfoById("Barrel"), 0.02f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Dungeon Breaker"), 0.02f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Caese Knuckle"), 0.005f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Tornado Staff"), Mathf.Max(floor - 3, 0) * 0.2f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Iron Bow"), 0.02f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Full Wand"), 0.002f),
            new Tuple<int, float>(ItemInfo.FindInfoById("King's Crown"), Mathf.Max(floor - 2, 0) * 0.1f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Leather Gloves"), 0.02f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Rainbow Pendant"), 0.0001f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Healing Potion"), 0.1f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Mega Potion"), 0.05f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Donut"), 0.03f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Stranger"), 0.05f),
            new Tuple<int, float>(ItemInfo.FindInfoById("Legendary Mana Elixir"), 0.0001f),
        };

        MakeNPCs(level);
        MakeItems(level);
        return level;
    }


    private static void wall(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[1], MapTile.TYPE_WALL);
    }


    private static void dig(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0);
    }

    private static void deco(Theme theme, Level level, int x, int y) {
        int v = Random.Range(0, theme.spriteDeco.Length);
        int type;
        if(v >= theme.spriteDeco.Length - 2) {
            type = MapTile.TYPE_WALL;
        } else {
            type = 0;
        }
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0, theme.spriteDeco[v], type, -1);
    }

    private static void door(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0,
                    theme.spriteDoor[0],
                    (int)MapTile.EType.BlockMove |
                    (int)MapTile.EType.BlockView |
                    (int)MapTile.EType.Door,
                    10000);
    }

    private static void upStair(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0,
                    theme.spriteStair[0],
                    (int)MapTile.EType.UpStair,
                    10000);
    }

    private static void downStair(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0,
                    theme.spriteStair[1],
                    (int)MapTile.EType.DownStair,
                    10000);
    }

    private static void makePortal(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0,
                    Util.LoadSprite("DawnLike/Objects/Door0", 37),
                    (int)MapTile.EType.Portal,
                    10000);
    }

    public static Vector2Int RandomPosition(Level level) {
        int r = Random.Range(0, level.groundNumber);
        while(r >= 0) {
            for(int i = 0; i < level.width; i++) {
                for(int j = 0; j < level.height; j++) {
                    if(!level.tiles[i][j].blocksMove && !level.tiles[i][j].isDoor && !level.tiles[i][j].isTransport && r-- <= 0) {
                        return new Vector2Int(i, j);
                    }
                }
            }
        }
        return new Vector2Int(0, 0);
    }

    public static void MakeNPCs(Level level) {
        level.npcs = new List<NPCStatus>();
        
        float psum = 0f;
        foreach(var x in npcList) {
            psum += x.second;
        }
        Vector2Int v;
        for(int i = 0; i < npcNumber; i++) {
            float random = Random.Range(0f, psum);
            while(true) {
                v = RandomPosition(level);
                int j;
                for(j = 0; j < i; j++) {
                    if(level.npcs[j].position == v) break;
                }
                if(j >= i) break;
            }
            foreach(var x in npcList) {
                if(random <= x.second) {
                    level.npcs.Add(new NPCStatus(v, x.first));
                    break;
                } else {
                    random -= x.second;
                }
            }
        }
    }

    public static void MakeItems(Level level) {
        level.items = new List<DroppedItem>();
        
        float psum = 0f;
        foreach(var x in itemList) {
            psum += x.second;
        }
        Vector2Int v;
        for(int i = 0; i < itemNumber; i++) {
            float random = Random.Range(0f, psum);
            v = RandomPosition(level);
            foreach(var x in itemList) {
                if(random <= x.second) {
                    Item item;
                    int kind = x.first;
                    if(ItemInfo.info[kind] is EquipmentInfo) {
                        var prob = Random.Range(0f, 1f);
                        int en = 1;
                        if (prob >= 0.8f) en = 3;
                        else if (prob >= 0.4f) en = 2;
                        item = new Equipment(kind, en);
                    } else {
                        item = new Consumable(kind, 1);
                    }
                    level.items.Add(new DroppedItem(v, item));
                    break;
                } else {
                    random -= x.second;
                }
            }
        }
    }
}
