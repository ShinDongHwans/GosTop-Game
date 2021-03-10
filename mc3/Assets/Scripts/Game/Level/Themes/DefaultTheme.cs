using UnityEngine;

public class DefaultTheme {
    

    public const string tilePath = "Sprites/DawnLike/Objects/Floor";
    public const int tileIndex = 96;
    public const int tileOffset = 80;
    public const string wallPath = "Sprites/DawnLike/Objects/Wall";
    public const int wallIndex = 32;
    public const int wallOffset = 29;
    public const string deepPath = "Sprites/DawnLike/Objects/Tile";
    public const int deepIndex = 1;

    private const string decoPath = "Sprites/DawnLike/Objects/Decor0";

    private static Theme _instance;
    public static Theme instance {
        get{
            if(_instance == null) {
                Sprite[] spritesBase = new Sprite[3] {
                    Resources.LoadAll<Sprite>(tilePath)[tileIndex],
                    Resources.LoadAll<Sprite>(wallPath)[wallIndex],
                    Resources.LoadAll<Sprite>(deepPath)[deepIndex]
                };

                Sprite[] deco = Resources.LoadAll<Sprite>(decoPath);
                Sprite[] spritesDeco = new Sprite[] {
                    deco[100],
                    deco[101],
                    deco[94],
                    deco[95],
                    deco[134],
                    deco[30],
                    deco[31]
                };

                _instance = new Theme(spritesBase, spritesDeco);
                _instance.wallOffset = wallOffset;
            }
            return _instance;
        }
    }
}