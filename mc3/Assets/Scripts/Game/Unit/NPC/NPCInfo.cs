using UnityEngine;

public class NPCInfo {
    /* Basic Information */
    public string name;
    public string description;
    public Sprite[] sprites;
    public NPCStatus.ERelationship relationship;
    /* Ability */
    public UnitStat stat;
    public int[] resistances;
    public AttackRange attackRange;
    public int[] magics;
    public int exp;
    public Tuple<int, float>[] dropList;
    

    /* Constructor */
    public NPCInfo(
            string name, /* 이름 */
            string desc, /* 설명 */
            string spriteName, int spriteIndex, /* Sprite 경로 (Sprites/ 생략, 맨 마지막 0/1 생략) */
            NPCStatus.ERelationship relationship,
            UnitStat stat, /* 기본 Stat */
            int[] resistances, /* 기본 저항 */
            AttackRange attackRange, /* 근접 공격 범위 */
            int[] magics, /* 마법 */
            int exp,
            Tuple<int, float>[] dropList = null) { 
        this.name = name;
        this.description = desc;
        this.sprites = new Sprite[]{
            Util.LoadSprite(spriteName, 0, spriteIndex),
            Util.LoadSprite(spriteName, 1, spriteIndex),
        };
        this.relationship = relationship;
        this.stat = stat;
        this.resistances = resistances;
        this.attackRange = attackRange;
        this.magics = magics == null ? new int[]{} : magics;
        this.exp = exp;
        this.dropList = dropList;
    }

    public bool HasMagic(int index) {
        foreach(var x in magics) {
            if(x == index) return true;
        }
        return false;
    }

    public bool HasMagic(string s) {
        foreach(var x in magics) {
            if(Magic.info[x].name.Equals(s)) return true;
        }
        return false;
    }


    public static NPCInfo[] info = {
        /* 1-5 */
        new NPCInfo("Mouse", "The thing on your desk",
            "DawnLike/Characters/Rodent", 5,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(1, 10, 10, 8), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{ }, 3,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.2f) }),

        new NPCInfo("Cat", "It is searching the mouse",
            "DawnLike/Characters/Cat", 0,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(2, 10, 9, 9), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{ }, 3,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat2"), 0.2f) }),

        new NPCInfo("Trash", "Suck!!",
            "DawnLike/Characters/Slime", 15,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(1, 10, 20, 5), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{ }, 1,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Stranger"), 1f) }),

        new NPCInfo("Baby Griffon", "Quite Week",
            "DawnLike/Characters/Avian", 66,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(4, 10, 14, 12), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat5"), 0.3f) }),

        new NPCInfo("Baby Piglet", "More Week",
            "DawnLike/Characters/Quadraped", 0,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(1, 10, 9, 14), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.5f) }),

        new NPCInfo("Dog", "Wal Wal",
            "DawnLike/Characters/Dog", 0,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(2, 10, 11, 10), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 4,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat2"), 0.3f) }),
            
        new NPCInfo("Dog Archor", "Wal Wal Shoot!",
            "DawnLike/Characters/Dog", 11,
            NPCStatus.ERelationship.HostileArchor,
            new UnitStat(2, 10, 10, 5), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(3, 4), new int[]{}, 6,
<<<<<<< HEAD
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Healing Potion"), 0.9f) ,  new Tuple<int, float>(ItemInfo.FindInfoById("Wooden Bow"), 0.05f) }),
=======
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat2"), 0.3f) }),
>>>>>>> dc46092fcf43b00674485ef50d9b5fbe383e1011

        new NPCInfo("Griffon", "5F Boss",
            "DawnLike/Characters/Avian", 67,
            NPCStatus.ERelationship.HostileMagicRandomSeldom,
            new UnitStat(5, 5, 17, 20), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{
                Magic.FindInfoByName("Tornado"),}, 12,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat2"), 0.3f), new Tuple<int, float>(ItemInfo.FindInfoById("Tornado Scroll"), 0.05f), new Tuple<int, float>(ItemInfo.FindInfoById("Soul Heart"), 0.05f) }),

       new NPCInfo("Enhanced Griffon", "If you meet... Godd Luck!",
            "DawnLike/Characters/Avian", 69,
            NPCStatus.ERelationship.HostileMagicRandomOften,
            new UnitStat(6, 10, 18, 25), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{
                Magic.FindInfoByName("Tornado"),}, 18,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat2"), 0.3f), new Tuple<int, float>(ItemInfo.FindInfoById("Tornado Scroll"), 0.1f), new Tuple<int, float>(ItemInfo.FindInfoById("Soul Heart"), 0.1f) }),
      

        /* 6-10 */
        new NPCInfo("Brown Slime", "Quite Danger",
            "DawnLike/Characters/Slime", 1,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(4, 0, 10, 12), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),

        new NPCInfo("Green Slime", "More Danger",
            "DawnLike/Characters/Slime", 3,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(6, 0, 10, 6), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),

        new NPCInfo("Some Misc", "Looks Danger",
            "DawnLike/Characters/Misc", 18,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 0, 10, 15), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),

        new NPCInfo("Some Misc 2", "Looks Danger",
            "DawnLike/Characters/Misc", 13,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(6, 0, 10, 18), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),

        /* 11-15 */
        new NPCInfo("Floating Eye", "Looks Week",
            "DawnLike/Characters/Elemental", 30,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(6, 10, 12, 16), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat5"), 0.1f) }),

        new NPCInfo("Rook", "Looks Week",
            "DawnLike/Characters/Elemental", 0,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(10, 10, 0, 30), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5),

        new NPCInfo("Mimic", "Danger",
            "DawnLike/Characters/Elemental", 38,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(6, 10, 10, 18), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 5),

        new NPCInfo("Lich", "A magician",
            "DawnLike/Characters/Undead", 32,
            NPCStatus.ERelationship.HostileMagicRandomOften,
            new UnitStat(1, 10, 10, 15), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 5), new int[]{
                Magic.FindInfoByName("Fire Bolt"),
                Magic.FindInfoByName("Ice Bolt"),
                Magic.FindInfoByName("Lightning Bolt"),
                Magic.FindInfoByName("Small Heal"),
            }, 10),

        new NPCInfo("Mira", ".",
            "DawnLike/Characters/Undead", 13,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(7, 0, 20, 18), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 5), new int[]{}, 10),

        new NPCInfo("Demon", "For Development",
            "DawnLike/Characters/Misc", 9,
            NPCStatus.ERelationship.HostileMagic1,
            new UnitStat(10, 15, 15, 30), new int[]{0,0,0,0,0,0,0},
            new AttackRange(1, 2), new int[]{
                Magic.FindInfoByName("Fire Bolt"),
                Magic.FindInfoByName("Small Heal"),
            }, 10,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),


        /* s1:1-5 */
        new NPCInfo("Unicorn", "Which has horn",
            "DawnLike/Characters/Quadraped", 28,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Snake", "Yikkkk",
            "DawnLike/Characters/Reptile", 37,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 24,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Dark Sheep", "Which has horn",
            "DawnLike/Characters/Quadraped", 26,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Deer", "Which has horn",
            "DawnLike/Characters/Quadraped", 42,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Elephant", "Which has ",
            "DawnLike/Characters/Quadraped", 44,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Plant", "Which has ",
            "DawnLike/Characters/Plant", 28,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(35, 10, 28, 77), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35),
        new NPCInfo("Plant Humanoid", "Which has ",
            "DawnLike/Characters/Plant", 29,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35),

        /* s2:1-5 */
        new NPCInfo("Red Jagguer", "Which has ",
            "DawnLike/Characters/Reptile", 1,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Redusa", "",
            "DawnLike/Characters/Reptile", 41,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35),
        new NPCInfo("Cute Snake", "",
            "DawnLike/Characters/Reptile", 63,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Lava Turtle", "",
            "DawnLike/Characters/Reptile", 62,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Red Dragon", "",
            "DawnLike/Characters/Reptile", 27,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Fire Elemental", "",
            "DawnLike/Characters/Elemental", 27,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35),

        /* s3:1-5 */
        new NPCInfo("Something Blue", "",
            "DawnLike/Characters/Aquatic", 18,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Snow Pig", "",
            "DawnLike/Characters/Quadraped", 47,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Glacier", "",
            "DawnLike/Characters/Reptile", 14,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Godurum", "",
            "DawnLike/Characters/Elemental", 33,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35),
        new NPCInfo("Ice Dragon", "",
            "DawnLike/Characters/Reptile", 28,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35,
            new Tuple<int, float>[]{ new Tuple<int, float>(ItemInfo.FindInfoById("Monster Meat1"), 0.1f) }),
        new NPCInfo("Ice Elemental", "",
            "DawnLike/Characters/Elemental", 13,
            NPCStatus.ERelationship.Hostile,
            new UnitStat(5, 10, 2, 50), new int[]{-10,-10,-10,-10,-10,-10,-10},
            new AttackRange(1, 1), new int[]{}, 35),
    };

    public static int FindInfoByName(string name) {
        for(int i = 0; i < info.Length; i++) {
            if(info[i].name.Equals(name)) return i;
        }
        return -1;
    }
}