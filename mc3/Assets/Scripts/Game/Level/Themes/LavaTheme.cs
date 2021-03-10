using UnityEngine;

public class LavaTheme {
    

    public const string tilePath = "Sprites/DawnLike/Objects/Floor";
    public const int tileIndex = 336;
    public const int tileOffset = 320;
    public const string wallPath = "Sprites/DawnLike/Objects/Wall";
    public const int wallIndex = 305;
    public const int wallOffset = 273;
    public const string deepPath = "Sprites/DawnLike/Objects/Tile";
    public const int deepIndex = 5;

    private static Theme _instance;
    public static Theme instance {
        get{
            if(_instance == null) {
                Sprite[] spritesBase = new Sprite[3] {
                    Resources.LoadAll<Sprite>(tilePath)[tileIndex],
                    Resources.LoadAll<Sprite>(wallPath)[wallIndex],
                    Resources.LoadAll<Sprite>(deepPath)[deepIndex]
                };

                Sprite[] ground0 = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Ground0");
                Sprite[] ground1 = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Ground1");
                Sprite[] tree0 = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Tree0");
                Sprite[] tree1 = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Tree1");
                Sprite[] decor0 = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Decor0");
                Sprite[] spritesDeco = new Sprite[] {
                    tree0[57],
                    tree1[57],
                    ground0[14],
                    ground0[15],
                    ground0[22],
                    ground0[23],
                    ground1[14],
                    ground1[15],
                    ground1[22],
                    ground1[23],
                    decor0[104],
                    decor0[105],
                };

                _instance = new Theme(spritesBase, spritesDeco);
                _instance.wallOffset = wallOffset;
            }
            return _instance;
        }
    }
}