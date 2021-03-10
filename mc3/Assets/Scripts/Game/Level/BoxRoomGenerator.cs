using UnityEngine;

public class BoxRoomGenerator {
    public static Level generate(Theme theme, int width, int height) {
        var level = new Level(theme, width, height);
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {
                int type;
                int sprite;
                int deco = -1;
                var d = Mathf.Min(new int[]{i, width - i - 1, j, height - j - 1});
                switch(d) {
                    case 0:
                        sprite = 2;
                        type = (int)MapTile.EType.BlockView |
                            (int)MapTile.EType.BlockMove;
                        break;
                    case 1:
                        sprite = 1;
                        type = (int)MapTile.EType.BlockView |
                            (int)MapTile.EType.BlockMove |
                            (int)MapTile.EType.BlockFly;
                        break; 
                    default:
                        sprite = 0;
                        type = 0;
                        if(Random.Range(0.0f, 10.0f) >= 5.0f) {
                            deco = Random.Range(0, theme.spriteDeco.Length);
                        }
                        break;
                }
                
                if(deco >= 0) {
                    level.tiles[i][j] =
                        new MapTile(theme.spriteBase[sprite], type, theme.spriteDeco[deco], 0, 1);
                } else {
                    level.tiles[i][j] =
                        new MapTile(theme.spriteBase[sprite], type);
                }
            }
        }
        return level;
    }
}
