using UnityEngine;

public class SnowTheme {
    

    public const string tilePath = "Sprites/DawnLike/Objects/Floor";
    public const int tileIndex = 302;
    public const int tileOffset = 282;
    
    public const string wallPath = "Sprites/DawnLike/Objects/Wall";
    public const int wallIndex = 193;
    public const int wallOffset = 190;
    public const string deepPath = "Sprites/DawnLike/Objects/Pit0";
    public const int deepIndex = 52;

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
                    tree0[83],
                    tree0[89],
                    tree0[211],
                    ground0[16],
                    ground1[16],
                    ground0[17],
                    ground1[17],
                    ground0[18],
                    ground1[18],
                    ground0[9],
                    ground1[9],
                    ground0[44],
                    ground1[44],
                };

                _instance = new Theme(spritesBase, spritesDeco);
                _instance.wallOffset = wallOffset;
            }
            return _instance;
        }
    }
}