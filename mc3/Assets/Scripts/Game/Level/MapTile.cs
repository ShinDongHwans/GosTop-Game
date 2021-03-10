using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile {
    // Tile Type (using as bitmask)
    public enum EType {
        None = 0, /* Nothing */
        BlockMove = 1, /* cannot walk through */
        BlockFly = 2, /* cannot fly over */
        BlockView = 4, /* cannot see through */
        Door = 8,
        UpStair = 16,
        DownStair = 32,
        Portal = 64,
    };

    public const int TYPE_WALL =
        (int)EType.BlockMove | (int)EType.BlockFly |
        (int)EType.BlockView;
    public const int TYPE_PIT = 
        (int)EType.BlockMove;

    public GameObject tileObject, decoObject;

    /* Tile */
    public Tile tile;

    /* Tile Sprite and Type */
    public Sprite sprite;
    public int tileType;

    /* Deco Sprite and Type */
    public Sprite deco;
    public int decoType;
    /* Deco HP */
    public int decoHP;
    public int decoData;

    /* Combined Type */
    public int type {
        get { return tileType | decoType; }
    }

    public bool blocksMove { get { return (type & (int)EType.BlockMove) != 0; }}
    public bool blocksFly { get { return (type & (int)EType.BlockFly) != 0; }}
    public bool blocksView { get { return (type & (int)EType.BlockView) != 0; }}
    public bool isDoor { get { return (type & (int)EType.Door) != 0; }}
    public bool isStair { get { return (type & ((int)EType.UpStair | (int)EType.DownStair)) != 0; }}
    public bool isUpStair { get { return (type & (int)EType.UpStair) != 0; }}
    public bool isDownStair { get { return (type & (int)EType.DownStair) != 0; }}
    public bool isPortal { get { return (type & (int)EType.Portal) != 0; }}
    public int portalDestination { get { return isPortal? decoData : -1; } }
    public bool isTransport { get { return isStair || isPortal; }}

    /* Information */
    public int tagI;

    public MapTile(Sprite sprite, int tileType, Sprite deco, int decoType, int decoHP) {
        Setup(sprite, tileType, deco, decoType, decoHP);
    }

    public MapTile(Sprite sprite, int type)
        : this(sprite, type, null, 0, -1) {
    }

    public void Setup(Sprite sprite, int tileType, Sprite deco, int decoType, int decoHP) {
        this.sprite = sprite;
        this.tileType = tileType;
        SetupDeco(deco, decoType, decoHP);
    }

    public void Setup(Sprite sprite, int tileType) {
        Setup(sprite, tileType, null, 0, -1);
    }

    public void SetupDeco(Sprite deco, int decoType, int decoHP) {
        this.deco = deco;
        this.decoType = decoType;
        this.decoHP = decoHP;
        this.decoData = 0;
    }
}
