using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme {
    public Sprite[] spriteBase;
    public Sprite[] spriteDoor;
    public Sprite[] spriteStair;
    public Sprite[] spriteDeco;

    public int wallOffset;

    public Theme(Sprite[] spriteBase, Sprite[] spriteDeco) {
        this.spriteBase = spriteBase;
        this.spriteDoor = new Sprite[] {
            Util.LoadSprite("DawnLike/Objects/Door", 0, 0),
            Util.LoadSprite("DawnLike/Objects/Door", 0, 1),
            Util.LoadSprite("DawnLike/Objects/Door", 1, 0),
            Util.LoadSprite("DawnLike/Objects/Door", 1, 1),
        };
        this.spriteStair = new Sprite[] {
            Util.LoadSprite("DawnLike/Objects/Tile", 8),
            Util.LoadSprite("DawnLike/Objects/Tile", 9),
        };
        this.spriteDeco = spriteDeco;
    }

    public Theme(Sprite[] spriteBase)
        : this(spriteBase, null) { }
}
