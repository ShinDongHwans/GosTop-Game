using UnityEngine;

public class WoodlandTheme {
    

    public const string tilePath = "Sprites/DawnLike/Objects/Floor";
    public const int tileIndex = 103;
    public const string wallPath = "Sprites/DawnLike/Objects/Wall";
    public const int wallIndex = 71;
    public const int wallOffset = 68;
    public const string deepPath = "Sprites/DawnLike/Objects/Tile";
    public const int deepIndex = 1;

    public const string decoPath = "Sprites/DawnLike/Objects/Decor0";

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
                Sprite[] spritesDeco = new Sprite[] {
                    ground0[0],
                    ground1[0],
                    ground0[1],
                    ground1[1],
                    ground0[8],
                    ground1[8],
                    ground0[9],
                    ground1[9],
                    ground0[44],
                    ground1[44],
                    tree0[19],
                    tree1[19],
                };

                _instance = new Theme(spritesBase, spritesDeco);
                _instance.wallOffset = wallOffset;
            }
            return _instance;
        }
    }
}