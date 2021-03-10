using System.Collections.Generic;
using UnityEngine;

public class MainLevelGenerator {

    static RectInt[] rects;
    static List<List<RectInt>> rooms;
    static int FindRoom(RectInt r) {
        for(int i = 0; i < rooms.Count; i++) {
            if(rooms[i].Contains(r)) return i;
        }
        return -1;
    }

    static int[][] map;
    const int EMPTY = 0;
    const int WALL = 1;
    const int DEEP = 2;
    const int DOOR = 3;
    const int UPSTAIR = 4;
    const int DOWNSTAIR = 5;
    const int PORTAL = 6;
    static int groundNumber;
    static int w;
    static int h;

    static Vector2Int[] upStairs;
    static Vector2Int[] downStairs;
    static Vector2Int? mPortal;

    static int npcNumber;
    static Tuple<int, float>[] npcList;

    static int itemNumber;
    static Tuple<int, float>[] itemList;

    static void MakeWalls(int size) {
        rects = new RectInt[size / 3 * 2];
        rooms = new List<List<RectInt>>();

        if(size < 8) size = 8;

        int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
        for(int k = 0; k < rects.Length; k++) {
            rects[k].width = Random.Range(2, 8);
            rects[k].height = Random.Range(2, 32 / rects[k].width);
            float r = Random.Range(4.0f, size);
            float th = (k + Random.Range(-0.8f, 0.8f)) * 2 * Mathf.PI / rects.Length;
            rects[k].x = (int)(r * Mathf.Cos(th));
            rects[k].y = (int)(r * Mathf.Sin(th));
            if(k == 0 || xMin > rects[k].xMin) xMin = rects[k].xMin;
            if(k == 0 || xMax < rects[k].xMax) xMax = rects[k].xMax;
            if(k == 0 || yMin > rects[k].yMin) yMin = rects[k].yMin;
            if(k == 0 || yMax < rects[k].yMax) yMax = rects[k].yMax;
        }

        w = xMax - xMin + 2;
        h = yMax - yMin + 2;

        map = new int[w][];
        for(int i = 0; i < w; i++) {
            map[i] = new int[h];
            for(int j = 0; j < h; j++) {
                map[i][j] = WALL;
            }
        }

        groundNumber = 0;

        for(int k = 0; k < rects.Length; k++) {
            for(int i = rects[k].xMin - xMin + 1; i <= rects[k].xMax - xMin; i++) {
                for(int j = rects[k].yMin - yMin + 1; j <= rects[k].yMax - yMin; j++) {
                    if(map[i][j] == WALL) groundNumber++;
                    map[i][j] = EMPTY;
                }
            }
            var rect = rects[k] = new RectInt(
                rects[k].xMin - xMin + 1,
                rects[k].yMin - yMin + 1,
                rects[k].xMax - rects[k].xMin,
                rects[k].yMax - rects[k].yMin);
            var list = new List<List<RectInt>>();
            foreach(List<RectInt> l in rooms) {
                foreach(var r in l) {
                    if(IsCollieded(rect, r)) {
                        list.Add(l);
                        break;
                    }
                }
            }
            if(list.Count == 0) rooms.Add(new List<RectInt>(new RectInt[]{rect}));
            else if(list.Count == 1) list[0].Add(rect);
            else {
                list[0].Add(rect);
                for(int i = 1; i < list.Count; i++) {
                    list[0].AddRange(list[i]);
                    rooms.Remove(list[i]);
                }
            }
        }
    }

    static void MakeStairs(int upStairNum, int downStairNum, int portal) {
        int portalNum = portal >= 0 ? 1 : 0;
        var stairs = new int[upStairNum + downStairNum + portalNum];
        upStairs = new Vector2Int[upStairNum];
        downStairs = new Vector2Int[downStairNum];
        mPortal = null;

        for(int i = 0; i < stairs.Length; i++) {
            stairs[i] = Random.Range(0, groundNumber);
            for(int j = 0; j < i; j++) {
                if(stairs[i] == stairs[j]) {
                    stairs[i] = (stairs[i] + 1) % groundNumber;
                    j = -1;
                }
            }
        }

        int cnt = 0;
        
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                if(map[i][j] == 0) {
                    cnt++;
                    for(int k = 0; k < stairs.Length; k++) {
                        if(cnt == stairs[k]) {
                            if(k < upStairNum) {
                                upStairs[k] = new Vector2Int(i, j);
                                map[i][j] = UPSTAIR;
                            } else if(k - upStairNum < downStairNum) {
                                downStairs[k - upStairNum] = new Vector2Int(i, j);
                                map[i][j] = DOWNSTAIR;
                            } else {
                                mPortal = new Vector2Int(i, j);
                                map[i][j] = PORTAL;
                            }
                        }
                    }
                }
            }
        }
    }

    static void MakePassage(int a, int b) {
        List<RectInt> roomA = rooms[a];
        List<RectInt> roomB = rooms[b];
        int ai = Random.Range(0, roomA.Count);
        int bi = Random.Range(0, roomB.Count);
        int ax = Random.Range(roomA[ai].xMin, roomA[ai].xMax);
        int ay = Random.Range(roomA[ai].yMin, roomA[ai].yMax);
        int bx = Random.Range(roomB[bi].xMin, roomB[bi].xMax);
        int by = Random.Range(roomB[bi].yMin, roomB[bi].yMax);
        
        int x = ax;
        int y = ay;

        bool doorFlag = false;
        
        while(x != bx || y != by) {
            int dx = x < bx ? 1 : -1;
            int dy = y < by ? 1 : -1;
            int lx = 0;
            int ly = 0;
            if(x == bx) ly = dy;
            else if(y == by) lx = dx;
            else if(Random.Range(0, 2) == 0) lx = dx;
            else ly = dy;
            x += lx;
            y += ly;
            if(map[x][y] == WALL) {
                if(!doorFlag) {
                    map[x][y] = DOOR;
                    if(lx != 0) { x += lx; map[x][y] = EMPTY; }
                    else { y += ly; map[x][y] = EMPTY; }
                    doorFlag = true;
                } else {
                    groundNumber++;
                    map[x][y] = EMPTY;
                    if((lx != -1 && map[x + 1][y] == EMPTY) ||
                        (lx != 1 && map[x - 1][y] == EMPTY) ||
                        (ly != -1 && map[x][y + 1] == EMPTY) ||
                        (ly != 1 && map[x][y - 1] == EMPTY)) {
                        doorFlag = true;
                        map[x][y] = DOOR;
                        groundNumber--;
                    }
                }
            }
        }
    }

    static void MakePassages() {
        for(int i = 0; i < rooms.Count; i++) {
            MakePassage(i, (i + 1) % rooms.Count);
        }
        /* Remove Strange Doors */
        Vector2Int[] off = new Vector2Int[] {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
        };
        for(int i = 1; i < w - 1; i++) {
            for(int j = 1; j < h - 1; j++) {
                if(map[i][j] == DOOR) {
                    if(map[i - 1][j] == DOOR || map[i + 1][j] == DOOR || map[i][j - 1] == DOOR || map[i][j + 1] == DOOR) {
                        map[i][j] = EMPTY;
                        groundNumber++;
                    } else {
                        int[] t = new int[] {-1, -1, -1, -1, -1};
                        t[0] = 0;
                        for(int l = 1; l <= 4; l++) {
                            t[l] = -1;
                            for(int k = 0; k < 8; k++) {
                                bool a = map[i + off[(t[l - 1] + k) % 8].x][j + off[(t[l - 1] + k) % 8].y] == WALL;
                                bool b = l % 2 == 0;
                                if(a ^ b) {
                                    t[l] = (t[l - 1] + k) % 8;
                                    break;
                                }
                            }
                            if(t[l] < 0) break;
                        }
                        if(t[4] >= 0 && t[2] == t[4]) {
                            map[i][j] = EMPTY;
                            groundNumber++;
                        }
                    }
                }
            }
        }
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
                v = RandomPosition();
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

    public static void MakeItems(Level level, int floor) {
        level.items = new List<DroppedItem>();
        
        float psum = 0f;
        foreach(var x in itemList) {
            psum += x.second;
        }
        Vector2Int v;
        for(int i = 0; i < itemNumber; i++) {
            float random = Random.Range(0f, psum);
            v = RandomPosition();
            foreach(var x in itemList) {
                if(random <= x.second) {
                    Item item;
                    int kind = x.first;
                    if(ItemInfo.info[kind] is EquipmentInfo) {
                        item = new Equipment(kind, Random.Range(0, 2 + floor / 5));
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

    public static Vector2Int RandomPosition() {
        int r = Random.Range(0, groundNumber);
        while(r >= 0) {
            for(int i = 0; i < w; i++) {
                for(int j = 0; j < h; j++) {
                    if(map[i][j] == 0 && r-- <= 0) {
                        return new Vector2Int(i, j);
                    }
                }
            }
        }
        return new Vector2Int(0, 0);
    }


    public static Level generate(int size, int upStairNum, int downStairNum, int portal = -1, int floor = 0) {
        Theme theme = DefaultTheme.instance;

        MakeWalls(size);
        MakeStairs(upStairNum, downStairNum, portal);
        MakePassages();

        var level = new Level(theme, w, h);
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                switch(map[i][j]) {
                    case EMPTY:
                        dig(theme, level, i, j);
                        if(Random.Range(0f, 1f) <= 0.05f)
                            deco(theme, level, i, j);
                        break;
                    case WALL:
                        wall(theme, level, i, j);
                        break;
                    case DOOR:
                        door(theme, level, i, j);
                        break;
                    case UPSTAIR:
                        upStair(theme, level, i, j);
                        break;
                    case DOWNSTAIR:
                        downStair(theme, level, i, j);
                        break;
                    case PORTAL:
                        makePortal(theme, level, i, j);
                        level.tiles[i][j].decoData = portal;
                        break;
                }
            }
        }

        level.upStairs = upStairs;
        level.downStairs = downStairs;
        level.portal = mPortal;
        level.groundNumber = groundNumber;

        npcNumber = 8 + size / 4;
        itemNumber = 10 - floor/3;
        if (floor < 5)
        {
            npcList = new Tuple<int, float>[] {
                //일반 몬스터 4~5층에서 80%차지
                new Tuple<int, float>(NPCInfo.FindInfoByName("Mouse"), 25f - 2.5f * floor),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Cat"), 25f - 2.5f * floor),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Baby Piglet"), 10f + 2f * floor),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Dog Archor"), 10f + 1f*floor),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Dog"), 10f + 2f*floor),

                new Tuple<int, float>(NPCInfo.FindInfoByName("Trash"), 0.1f),

                // 그리폰 계열 
                new Tuple<int, float>(NPCInfo.FindInfoByName("Baby Griffon"), (floor>2) ? 65f-15f*floor : 0f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Griffon"), (floor>3) ? 14f : 0f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Enhanced Griffon"), (floor>3) ? 1f : 0f),

            };
            itemList = new Tuple<int, float>[] {
                // 물약 35%
                new Tuple<int, float>(ItemInfo.FindInfoById("Healing Potion"), 40f),
                // 음식 40%
                new Tuple<int, float>(ItemInfo.FindInfoById("Donut"), 40f),
                
                //장비 21%
                new Tuple<int, float>(ItemInfo.FindInfoById("Spear"), 3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Hobit Sword"), 3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Axe"), 3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Saber"), 3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Mage hat"), 3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Mage Robe"), 3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Cotton Gloves"), 3f),
                //고급장비 3%
                new Tuple<int, float>(ItemInfo.FindInfoById("Caeser Knuckle"), 1f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Wooden Bow"), 2f),

                //지팡이 0.9%
                new Tuple<int, float>(ItemInfo.FindInfoById("Fire Staff"), 0.3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Ice Staff"), 0.3f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Lightning Staff"), 0.3f),

                //0.1%
                new Tuple<int, float>(ItemInfo.FindInfoById("Heal Of Ring"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Bag"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Warp Scroll"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Fear Scroll"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Show Map Scroll"), 0.02f),
            };
        }
        else if (floor < 10)
        {
            npcList = new Tuple<int, float>[] {
                new Tuple<int, float>(NPCInfo.FindInfoByName("Brown Slime"), 5f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Green Slime"), 3f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Some Misc"), 0.2f + floor * 0.1f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Some Misc 2"), 0.2f + floor * 0.1f),
            };
            itemList = new Tuple<int, float>[] {
                new Tuple<int, float>(ItemInfo.FindInfoById("Healing Potion"), 0.1f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Mega Potion"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Donut"), 0.4f - 0.01f * floor),
                new Tuple<int, float>(ItemInfo.FindInfoById("Chocolate"), 0.05f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Spear"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Hobit Sword"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Axe"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Wooden Bow"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Saber"), 0.03f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Caeser Knuckle"), 0.001f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Fire Staff"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Ice Staff"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Lightning Staff"), 0.003f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Mage hat"), 0.04f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Bandana"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Mage Robe"), 0.04f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Leather Armor"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Shoes"), 0.05f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Red Shoes"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Cotton Gloves"), 0.04f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Heal Of Ring"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Bag"), 0.001f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Warp Scroll"), 0.0003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Fear Scroll"), 0.0003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Show Map Scroll"), 0.003f),
            };
        }
        else
        {
            npcList = new Tuple<int, float>[] {
                new Tuple<int, float>(NPCInfo.FindInfoByName("Floating Eye"), 3f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Rook"), 3f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Mimic"), 2f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Lich"), 1f + floor * 0.1f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Mira"), 1f + floor * 0.1f),
                new Tuple<int, float>(NPCInfo.FindInfoByName("Demon"), 1f + floor * 0.1f),
            };
            itemList = new Tuple<int, float>[] {
                new Tuple<int, float>(ItemInfo.FindInfoById("Healing Potion"), 0.02f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Mega Potion"), 0.05f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Donut"), 0.1f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Chocolate"), 0.05f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Food Set"), 0.02f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Spear"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Hobit Sword"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Axe"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Wooden Bow"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Caeser Knuckle"), 0.002f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Fire Staff"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Ice Staff"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Lightning Staff"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("HellFire"), 0.001f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Mage hat"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Bandana"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("300"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Mage Robe"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Leather Armor"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Green Chain"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Shoes"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Red Shoes"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Iro Shoes"), 0.01f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Leather Gloves"), 0.01f),

                new Tuple<int, float>(ItemInfo.FindInfoById("Heal Of Ring"), 0.003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Bag"), 0.001f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Warp Scroll"), 0.0003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Fear Scroll"), 0.0003f),
                new Tuple<int, float>(ItemInfo.FindInfoById("Show Map Scroll"), 0.003f),
            };
        }

        MakeNPCs(level);
        MakeItems(level, floor);
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

    private static void deco(Theme theme, Level level, int x, int y) {
        level.tiles[x][y].SetupDeco(theme.spriteDeco[Random.Range(0, theme.spriteDeco.Length)], 0 , 100000);
    }

    private static bool IsCollieded(RectInt a, RectInt b) {
        return 
            ((a.xMin <= b.xMin && b.xMin < a.xMax) ||
                (a.xMin <= b.xMax && b.xMax < a.xMax) ||
                (b.xMin <= a.xMin && a.xMin < b.xMax) ||
                (b.xMin <= a.xMax && a.xMax < b.xMax)) &&
            ((a.yMin <= b.yMin && b.yMin < a.yMax) ||
                (a.yMin <= b.yMax && b.yMax < a.yMax) ||
                (b.yMin <= a.yMin && a.yMin < b.yMax) ||
                (b.yMin <= a.yMax && a.yMax < b.yMax));
    }

    private static void makePortal(Theme theme, Level level, int x, int y) {
        level.tiles[x][y] =
                new MapTile(theme.spriteBase[0], 0,
                    Util.LoadSprite("DawnLike/Objects/Door0", 37),
                    (int)MapTile.EType.Portal,
                    10000);
    }
}
