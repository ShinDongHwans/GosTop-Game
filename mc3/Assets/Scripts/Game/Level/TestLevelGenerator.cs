using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelGenerator {
    public static Level generate(Theme theme, int size, int upStairNum, int downStairNum) {
        var rects = new RectInt[size / 3 * 2];
        if(size < 8) size = 8;

        /* Generate Rects */
        int xMin = 0, xMax = 0, yMin = 0, yMax = 0;
        for(int k = 0; k < rects.Length; k++) {
            rects[k].width = Random.Range(2, 10);
            rects[k].height = Random.Range(2, 40 / rects[k].width);
            float r = Random.Range(1.0f, size);
            float th = (k + Random.Range(-1.0f, 1.0f)) * 2 * Mathf.PI / rects.Length;
            rects[k].x = (int)(r * Mathf.Cos(th));
            rects[k].y = (int)(r * Mathf.Sin(th));
            if(k == 0 || xMin > rects[k].xMin) xMin = rects[k].xMin;
            if(k == 0 || xMax < rects[k].xMax) xMax = rects[k].xMax;
            if(k == 0 || yMin > rects[k].yMin) yMin = rects[k].yMin;
            if(k == 0 || yMax < rects[k].yMax) yMax = rects[k].yMax;
        }

        int w = xMax - xMin + 2;
        int h = yMax - yMin + 2;
        var level = new Level(theme, w, h);
        
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                wall(theme, level, i, j);
            }
        }

        int groundCount = 0;

        for(int k = 0; k < rects.Length; k++) {
            for(int i = rects[k].xMin - xMin; i < rects[k].xMax - xMin; i++) {
                for(int j = rects[k].yMin - yMin; j < rects[k].yMax - yMin; j++) {
                    if(level.tiles[i][j].tileType == 0) groundCount++;
                    dig(theme, level, 1 + i, 1 + j);
                }
            }
        }

        /* Generate Stairs */
        var stairs = new int[upStairNum + downStairNum];
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
                if(level.tiles[i][j].tileType == 0) {
                    groundCount--;
                    for(int k = 0; k < stairs.Length; k++) {
                        if(groundCount == stairs[k]) {
                            if(k < upStairNum) {
                                level.upStairs[k] = new Vector2Int(i, j);
                                upStair(theme, level, i, j);
                                level.tiles[i][j].decoData = k;
                            } else {
                                level.downStairs[k - upStairNum] = new Vector2Int(i, j);
                                downStair(theme, level, i, j);
                                level.tiles[i][j].decoData = k - upStairNum;
                            }
                        }
                    }
                }
            }
        }

        /* Generate Passages */
        for(int k = 0; k < rects.Length; k++) {
            int l = (k + 1) % rects.Length;
            int x1 = Random.Range(rects[k].xMin, rects[k].xMax) - xMin + 1;
            int y1 = Random.Range(rects[k].yMin, rects[k].yMax) - yMin + 1;
            int x2 = Random.Range(rects[l].xMin, rects[l].xMax) - xMin + 1;
            int y2 = Random.Range(rects[l].yMin, rects[l].yMax) - yMin + 1;
            int dx = x1 < x2 ? 1 : -1;
            int dy = y1 < y2 ? 1 : -1;
            int x = x1, y = y1;
            bool doorFlag = Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2) < 3;
            while(x != x2 || y != y2) {
                if(x == x2) y += dy;
                else if(y == y2) x += dx;
                else if(Random.Range(0, 2) == 0) x += dx;
                else y += dy;
                if(!doorFlag && level.tiles[x][y].tileType != 0) {
                    door(theme, level, x, y);
                    doorFlag = true;
                /*} else if(doorFlag && level.tiles[x][y].tileType == 0) {
                    break; */
                } else if(!level.tiles[x][y].isStair) {
                    dig(theme, level, x, y);
                }
            }
        }

        /* Decoration */
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                if(level.tiles[i][j].type == 0) {
                    if(Random.Range(0f, 100f) > 95f) {
                        level.tiles[i][j].SetupDeco(
                            theme.spriteDeco[Random.Range(0, theme.spriteDeco.Length)],
                            0,
                            10
                        );
                    }
                    /* Generate Item */
                    var id = Random.Range(0, ItemInfo.info.Length);
                    float uniqueness = ItemInfo.info[id].uniqueness;
                    if (Random.Range(0f, 100f) - uniqueness > 0) {
                        Item item = ItemInfo.info[id] is EquipmentInfo?
                            (Item)new Equipment(id, Random.Range(0, 3)) : (Item)new Consumable(id);
                        level.items.Add(new DroppedItem(i, j, item));
                    }
                    /* Generate NPC */
                    if(Random.Range(0f, 100f) > 99.5f) {
                        level.npcs.Add(new NPCStatus(i, j, 0));
                    } else if(Random.Range(0f, 100f) > 98.5f) {
                        level.npcs.Add(new NPCStatus(i, j, 1));
                    }
                }
            }
        }

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
}
