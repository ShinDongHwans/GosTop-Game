using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {

    public Theme theme { get; internal set; }

    public int width { get; }
    public int height { get; }

    public int groundNumber;

    /* tiles[x][y] */
    public MapTile[][] tiles { get; }
    public bool[][] visitedMap;

    /* Extra Objects */
    public Vector2Int[] upStairs;
    public Vector2Int[] downStairs;
    public Vector2Int? portal;
    public List<NPCStatus> npcs;
    public List<DroppedItem> items;


    public Level(Theme theme, int width, int height) {
        this.theme = theme;
        this.width = width;
        this.height = height;
        this.tiles = new MapTile[width][];
        this.visitedMap = new bool[width][];
        for(int i = 0; i < width; i++) {
            this.tiles[i] = new MapTile[height];
            this.visitedMap[i] = new bool[height];
        }
        portal = null;
        npcs = new List<NPCStatus>();
        items = new List<DroppedItem>();
    }



    /* Level Utility Functions */

    public bool IsVectorInMap(int x, int y) {
        return 0 <= x && x < width && 0 <= y && y < height;
    }
    public bool IsVectorInMap(Vector2Int v) {
        return IsVectorInMap(v.x, v.y);
    }

    public NPCStatus GetNPCAt(Vector2Int v) {
        foreach(var i in npcs) {
            if(i.position == v) return i;
        }
        return null;
    }

    public List<DroppedItem> GetItemsAt(Vector2Int v) {
        var list = new List<DroppedItem>();
        foreach(var i in items) {
            if(i.position == v) list.Add(i);
        }
        return list;
    }


    /* Rendering */
    public Sprite[] prerendered { get; internal set; }
    public int preWidth { get; internal set; }
    public int preHeight { get; internal set; }

    public const int preS = 64; // 1024 / 16;

    public void Prerendering() {
        int w = 1 + (width - 1) / preS;
        int h = 1 + (height - 1) / preS;
        var tex = new Texture2D[w][];
        for(int i = 0; i < w; i++) {
            tex[i] = new Texture2D[h];
            for(int j = 0; j < h; j++) {
                tex[i][j] = new Texture2D(1024, 1024);
                tex[i][j].filterMode = FilterMode.Point;
            }
        }

        Debug.Log("Prerendering: " + w + "*" + h + " // " + width + "*" + height);
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                Sprite s = tiles[i][j].sprite;
                if(s == theme.spriteBase[1]) {
                    s = Util.LoadSprite("DawnLike/Objects/Wall", theme.wallOffset + GuessWallOffset(i, j));
                }
                DrawSpriteOnTexture(tex[i / preS][j / preS], s, i % preS, j % preS);
            }
        }

        var set = new Sprite[w * h];
        for(int i = 0; i < w * h; i++) {
            tex[i / h][i % h].Apply();
            set[i] = Sprite.Create(tex[i / h][i % h], new Rect(0, 0, 1024, 1024), new Vector2(0f, 0f), 16f);
        }

        prerendered = set;
        preWidth = w;
        preHeight = h;
    }

    private int GuessWallOffset(int x, int y) {
        bool[] wallMap = new bool[4];
        int cnt = 0;
        for(int i = 0; i < 4; i++) {
            int dx = ((i + 1) * 2 - 1) % 3 - 1;
            int dy = ((i + 1) * 2 - 1) / 3 - 1;
            if(x + dx < 0 || x + dx >= width || y + dy < 0 || y + dy >= height)
                wallMap[i] = true;
            else wallMap[i] = tiles[x + dx][y + dy].sprite == theme.spriteBase[1];
            if(wallMap[i]) cnt++;
        }
        switch(cnt) {
            case 4: return 47 - 29;
            case 3:
                if(!wallMap[3]) return 33 - 29;
                else if(!wallMap[1]) return 46 - 29;
                else if(!wallMap[2]) return 48 - 29;
                else return 61 - 29;
            case 2: case 1:
                if(!(wallMap[1] || wallMap[2])) return 44 - 29;
                else if(!(wallMap[0] || wallMap[3])) return 30 - 29;
                else if(!(wallMap[3] || wallMap[1])) return 29 - 29;
                else if(!(wallMap[3] || wallMap[2])) return 31 - 29;
                else if(!(wallMap[0] || wallMap[1])) return 59 - 29;
                else return 60 - 29;
            case 0: return 45 - 29;
            default: return 32 - 29;
        }
    }

    private void DrawSpriteOnTexture(Texture2D dst, Sprite src, int x, int y) {
        var t = src.texture;
        var r = src.rect;
        Debug.Log("r " + r);
        var colors = t.GetPixels((int)r.x, (int)r.y, 16, 16);
        dst.SetPixels(x * 16, y * 16, 16, 16, colors);
    }


    /* Shadow */
    public const int SHADOW_SIZE = 7;
    public const int SHADOW_SIZE2 = SHADOW_SIZE * 2 + 1;
    public bool[][] CalculateShadow(Vector2Int position) {
        bool[][] shadow = new bool[SHADOW_SIZE2][];
        for(int i = 0; i < SHADOW_SIZE2; i++) {
            shadow[i] = new bool[SHADOW_SIZE2];
        }
        shadow[SHADOW_SIZE][SHADOW_SIZE] = true;
        for(int i = 0; i < SHADOW_SIZE2; i++) {
            CastLight(shadow, new Vector2Int(i - SHADOW_SIZE, -SHADOW_SIZE), position);
            CastLight(shadow, new Vector2Int(i - SHADOW_SIZE, SHADOW_SIZE), position);
            CastLight(shadow, new Vector2Int(-SHADOW_SIZE, i - SHADOW_SIZE), position);
            CastLight(shadow, new Vector2Int(SHADOW_SIZE, i - SHADOW_SIZE), position);
        }
        visitedMap[position.x][position.y] = true;
        return shadow;
    }

    private void CastLight(bool[][] shadow, Vector2Int dir, Vector2Int origin) {
        int ax = Mathf.Abs(dir.x);
        int ay = Mathf.Abs(dir.y);
        Vector2 d;
        if(ay > ax) ax = ay;
        d = new Vector2((float)dir.x / ax, (float)dir.y / ax);
        Vector2 p = new Vector2(d.x, d.y);
        for(int i = 1; i < ax; i++) {
            int px = Mathf.RoundToInt(p.x);
            int x = origin.x + px;
            int py = Mathf.RoundToInt(p.y);
            int y = origin.y + py;
            visitedMap[x][y] = true;
            shadow[SHADOW_SIZE + (int)p.x][SHADOW_SIZE + (int)p.y] = true;
            if(!IsVectorInMap(x, y)) break;
            if(tiles[x][y].blocksView) break;
            
            p += d;
        }
    }


    public Status GetNearestUnitOnLine(Vector2Int target, Vector2Int origin) {
        return GetNearestUnitOnLine(target, origin, new Vector2Int(-1, -1));
    }
    public Status GetNearestUnitOnLine(Vector2Int target, Vector2Int origin, Vector2Int player) {
        List<Vector2Int> list = PositionsOnLine(target, origin);
        foreach(var p in list) {
            var unit = GetNPCAt(p);
            if(unit != null) return unit;
            if(player == p) return GameManager.instance.player;
        }
        return null;
    }

    private List<Vector2Int> PositionsOnLine(Vector2Int target, Vector2Int origin) {
        var delta = target - origin;
        int ax = Mathf.Abs(delta.x);
        int ay = Mathf.Abs(delta.y);
        Vector2 d;
        if(ay > ax) ax = ay;
        d = new Vector2((float)delta.x / ax, (float)delta.y / ax);
        Vector2 p = new Vector2(d.x, d.y);

        List<Vector2Int> list = new List<Vector2Int>();

        for(int i = 1; i < ax; i++) {
            int x = origin.x + Mathf.RoundToInt(p.x);
            int y = origin.y + Mathf.RoundToInt(p.y);
            list.Add(new Vector2Int(x, y));
            p += d;
        }
        return list;
    }



    private class Backtrack {
        public float dist;
        public Vector2Int last;

        public Backtrack(int dist, Vector2Int last) {
            this.dist = dist; this.last = last;
        }
        public Backtrack() : this(-1, new Vector2Int(-1, -1)) { }

        public bool Update(float dist, float d, Vector2Int from) {
            if(this.dist < 0 || this.dist > dist + d) {
                this.dist = dist + d;
                this.last = from;
                return true;
            }
            return false;
        }
    }
    public List<Vector2Int> FindPath(Vector2Int from, Vector2Int to, bool checkNPC = false) {
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        Backtrack[][] back = new Backtrack[width][];
        for(int i = 0; i < back.Length; i++) {
            back[i] = new Backtrack[height];
            for(int j = 0; j < back[i].Length; j++) {
                back[i][j] = new Backtrack();
            }
        }
        back[from.x][from.y] = new Backtrack(0, from);
        q.Enqueue(from);
        Vector2Int p = from;
        while(q.Count > 0) {
            p = q.Dequeue();
            float dist = back[p.x][p.y].dist;
            for(int i = 0; i < 9; i++) {
                int dx = i % 3 - 1;
                int dy = i / 3 - 1;
                float d = dx * dx + dy * dy;
                if(d == 2) d = 1.2f;
                if(d != 0 && IsVectorInMap(p.x + dx, p.y + dy) &&
                        (!tiles[p.x + dx][p.y + dy].blocksMove || tiles[p.x + dx][p.y + dy].isDoor) &&
                        (!checkNPC || GetNPCAt(new Vector2Int(p.x + dx, p.y + dy)) == null) &&
                        back[p.x + dx][p.y + dy].Update(dist, d, p)) {
                    q.Enqueue(new Vector2Int(p.x + dx, p.y + dy));
                }
            }
        }
        if(back[to.x][to.y].dist < 0) return null;

        List<Vector2Int> list = new List<Vector2Int>();
        p = to;
        while(p != back[p.x][p.y].last) {
            list.Add(p);
            p = back[p.x][p.y].last;
        }
        list.Reverse();
        return list;
    }


    public void MarkAllVisited() {
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                if(!tiles[i][j].blocksView) {
                    for(int dx = -1; dx <= 1; dx++) {
                        for(int dy = -1; dy <= 1; dy++) {
                            visitedMap[i + dx][j + dy] = true;
                        }
                    }
                }
            }
        }
    }

    public void BreakWalls(Vector2Int dest, Vector2Int origin) {
        Vector2Int dir = dest - origin;
        int ax = Mathf.Abs(dir.x);
        int ay = Mathf.Abs(dir.y);
        Vector2 d;
        if(ay > ax) ax = ay;
        d = new Vector2((float)dir.x / ax, (float)dir.y / ax);
        Vector2 p = new Vector2(d.x, d.y);
        bool flag = false;
        while(true) {
            int px = Mathf.RoundToInt(p.x);
            int x = origin.x + px;
            int py = Mathf.RoundToInt(p.y);
            int y = origin.y + py;
            if(x < 1 || x >= width - 1 || y < 1 || y >= height - 1) break;
            if(tiles[x][y].blocksMove) {
                flag = true;
                tiles[x][y].tileObject.GetComponent<SpriteRenderer>().sprite = theme.spriteBase[0];
                if(tiles[x][y].decoObject != null) {
                    GameObject.Destroy(tiles[x][y].decoObject);
                    tiles[x][y].decoObject = null;
                }
                tiles[x][y].tileType = 0;
            } else {
                if(flag) break;
            }
            p += d;
        }
    }
}
