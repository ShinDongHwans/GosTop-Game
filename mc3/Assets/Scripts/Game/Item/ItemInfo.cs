using UnityEngine;


public class ItemInfo {
    public string name;
    public string description;
    public Sprite sprite;
    public int magic;
    public float uniqueness;
         

    public static ItemInfo[] info = new ItemInfo[]{
        /* Enumerate Infos */
        /*Weapon*/
        //Sword(0 ~ 7)
        new WeaponInfo("Spear", "Spear",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[0],
            2, new UnitStatus(0, 0, 10, 0, 0f, 0f) , new AttackRange(2, 2), 98f, -1),
        new WeaponInfo("Moon Trident", "This weapon is from the moon... maybe",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[6],
            3, new UnitStatus(0, 0, 25, 0, 0.05f, 0f) , new AttackRange(1, 2), 98f, -1),
        new WeaponInfo("Hobit Sword", "Made by JW",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[7],
            0, new UnitStatus(0, 0, 9, 0, 0f, 0f) , new AttackRange(1, 1), 98f, -1),
        new WeaponInfo("Dark Blade", "Dark Power!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[12],
            0, new UnitStatus(0, 0, 21, 0, 0.1f, 0f) , new AttackRange(1, 1), 98f, -1),
        new WeaponInfo("Scythe", "Where are your soul?",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[26],
            0, new UnitStatus(0, 0, 11, 0, 0f, 0f) , new AttackRange(1, 2), 98f, -1),
        new WeaponInfo("Dungeon Breaker", "Jonna Sseda",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[32],
            3, new UnitStatus(0, 0, 16, 0, -0.05f, 0f) , new AttackRange(1, 1), 98f, -1),
        new WeaponInfo("Axe", "Axe",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[23],
            3, new UnitStatus(0, 0, 10, 3, 0.2f, 0f) , new AttackRange(1, 1), 98f, -1),
        new WeaponInfo("O Hammar", "Ohhhhhhh~",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/LongWep")[31],
            3, new UnitStatus(0, 0, 50, 0, -0.5f, 0f) , new AttackRange(1, 1), 98f, -1),
        

        //SubWeapon(8, 9)
        new SubWeaponInfo("Saber", "Jonna Yakhada",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/ShortWep")[7],
            1, new UnitStatus(0, 0, 7, 0, 0f, 0f) , new AttackRange(1, 1), 98f, -1),
        new SubWeaponInfo("Caeser Knuckle", "You can use Iron Claw",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Armor")[14],
            4, new UnitStatus(0, 0, 5, 0, 0f, 0.2f) , new AttackRange(1, 1), 99f, Magic.FindInfoByName("Iron Claw")),

        //Staff(10 ~ 16)
        new WeaponInfo("Fire Staff", "You can use Fire magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[3],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.9f, Magic.FindInfoByName("Fire Bolt")),
        new WeaponInfo("Ice Staff", "You can use Ice magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[0],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.9f, Magic.FindInfoByName("Ice Bolt")),
        new WeaponInfo("Lightning Staff", "You can use Lightning magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[4],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.9f, Magic.FindInfoByName("Lightning Bolt")),
        new WeaponInfo("Zeus Staff", "You can use ChainLightning",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[2],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.99f, Magic.FindInfoByName("ChainLightning")),
        new WeaponInfo("Tornado Staff", "You can use Tornado magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[12],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.99f, Magic.FindInfoByName("Tornado")),
         new WeaponInfo("Aqua Staff", "You can use HydroPump magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[11],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.99f, Magic.FindInfoByName("HydroPump")),
        new WeaponInfo("HellFire", "Burn the world!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[33],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.9f, Magic.FindInfoByName("Fireball")),
       
        //Bow(17 ~ 18)
        new WeaponInfo("Wooden Bow", "Shoot the enemy!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Ammo")[8],
            2, new UnitStatus(0, 0, 3, 0, 0.1f, 0), new AttackRange(3, 7), 99.8f, Magic.FindInfoByName("Arrow1")),
        new WeaponInfo("Iron Bow", "Shoot the enemy!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Ammo")[11],
            3, new UnitStatus(0, 0, 5, 0, 0.05f, 0), new AttackRange(3, 7), 99.8f, Magic.FindInfoByName("Arrow2")),

        //Ninja weapon(19 ~ 20)
        new WeaponInfo("Ninja Star", "You can be Ninja with this item!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Ammo")[21],
            2, new UnitStatus(0, 0, 5, 0, 0.05f, 0.1f), new AttackRange(1, 1), 99.9f, Magic.FindInfoByName("Throw Stone")),
        new WeaponInfo("Dart", "You can shoot dart",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Ammo")[20],
            2, new UnitStatus(0, 0, 3, 0, 0.2f, 0), new AttackRange(1, 1), 99.9f, Magic.FindInfoByName("Dart")),

        //Wand(21 ~ 23)
        new SubWeaponInfo("Teleport Wand", "Teleport someon to random place",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[5],
            2, new UnitStatus(0, 0, 1, 0, 0, 0), new AttackRange(1, 1), 99.5f, Magic.FindInfoByName("Teleport Short Distance")),
        new SubWeaponInfo("Full Wand", "You will always be full and happy",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Music")[2],
            2, new UnitStatus(3, 0, 0, 0, 0, -0.05f), new AttackRange(1, 1), 99.5f, Magic.FindInfoByName("Reduce Hunger1")),
        new SubWeaponInfo("Magician Wand", "You are a magician, We grant you",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[43],
            2, new UnitStatus(3, 0, 0, 0, 0, 0), new AttackRange(1, 1), 99.5f, Magic.FindInfoByName("Magic Bolt")),
         new SubWeaponInfo("Road Maker", "You can use Break Walls magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[27],
            2, new UnitStatus(3, 0, 0, 0, 0, 0), new AttackRange(1, 1), 99.5f, Magic.FindInfoByName("Break Walls")),
         new SubWeaponInfo("Finger Snap", "You can use Random magic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Wand")[28],
            2, new UnitStatus(3, 0, 0, 0, 0, 0), new AttackRange(1, 1), 99.5f, Magic.FindInfoByName("Random Magic")),
        
        
        /*Defensive*/
        //Helmet
        new HelmetInfo("Mage hat", "It increase your def a little bit",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Hat")[10],
            0, new UnitStatus(0, 0, 0, 2, 0f, 0), 99.1f),
        new HelmetInfo("King's Crown", "It's a traitor!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Hat")[14],
            1, new UnitStatus(5, 0, 3, 1, 0f, 0), 99.9f),
        new HelmetInfo("Bandana", "Feel Desert~",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Armor")[35],
            1, new UnitStatus(3, 0, 0, 0, 0f, 0.05f), 99.9f),
        new HelmetInfo("300", "This is Sparta~",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Hat")[4],
            1, new UnitStatus(0, 0, 0, 10, 0.1f, 0f), 99.9f),
        new HelmetInfo("Brave Knight", "It is used by acient knight...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Hat")[2],
            1, new UnitStatus(0, 0, 0, 20, 0f, -0.05f), 99.9f),

        //Armor
        new ArmorInfo("Leather Armor", "Made in KAIST",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Armor")[1],
            2, new UnitStatus(0, 0, 0, 4, 0f, 0), 99.2f),
        new ArmorInfo("Green Chain", "It has green aurora",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Armor")[3],
            4, new UnitStatus(0, 0, 0, 10, 0f, 0), 99.5f),
        new ArmorInfo("Mage Robe", "Do not use magic!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Armor")[26],
            1, new UnitStatus(0, 0, 0, 2, 0f, 0), 99.5f),
        new ArmorInfo("Barrel", "Rolling~ Rolling~",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Chest0")[9],
            3, new UnitStatus(0, 0, 0, 7, -0.05f, -0.05f), 99.5f),

        //Shoes
        new ShoesInfo("Shoes", "...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Boot")[0],
            1, new UnitStatus(0, 0, 0, 1, 0f, 0.05f), 99f),
        new ShoesInfo("Red Shoes", "It looks elegance",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Boot")[7],
            2, new UnitStatus(0, 0, 0, 3, 0f, 0.03f), 99.2f),
        new ShoesInfo("Iron Shoes", "Can you walk?",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Boot")[1],
            2, new UnitStatus(0, 0, 0, 7, 0f, -0.05f), 99.2f),
        new ShoesInfo("DarKnight Boots", "Good Def",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Boot")[8],
            2, new UnitStatus(0, 0, 0, 15, 0f, 0f), 99.2f),

        //Gloves
        new GlovesInfo("Cotton Gloves", "It is just deco.. you pick it up??",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Glove")[3],
            new UnitStatus(0, 0, 0, 1, 0f, 0), 98f),
        new GlovesInfo("Leather Gloves", "Elegant Gloves, awesome look and effect",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Glove")[1],
            new UnitStatus(0, 0, 1, 1, 0.05f, 0), 99f),
        new GlovesInfo("Iron Gloves", "Hard Gloves, You accuaracy will be down...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Armor")[17],
            new UnitStatus(0, 0, 0, 5, -0.05f, 0), 99f),

        //Accessory(6 ~ 7)
        new AccessoryInfo("God Of Ring", "The Absolute Ring JEUSES used",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Ring")[16],
            new UnitStatus(5, 1, 3, 3, 0.1f, 0.1f), 99f, Magic.FindInfoByName("Smite")),
        new AccessoryInfo("Heal Of Ring", "You can use Heal Masic",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Ring")[13],
            new UnitStatus(5, 0, 0, 0, 0f, 0f), 99f, Magic.FindInfoByName("Small Heal")),
        new AccessoryInfo("Black Pearl Amulet", "It's expensive",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Amulet")[0],
            new UnitStatus(10, 0, 0, 0, 0.05f, 0f), 99f, -1),
        new AccessoryInfo("Rainbow Pendant", "It is rainbow shining",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Amulet")[16],
            new UnitStatus(15, 5, 5, 5, 0.05f, 0.05f), 99f, Magic.FindInfoByName("Warp")),
        
        /*Item*/
        //Potion(8 ~ 9)
        new PotionInfo("Healing Potion", "Restore your health",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Potion")[0],
            98f, Magic.FindInfoByName("Heal Potion")),
        new PotionInfo("Mega Potion", "Restore your health enoughly",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Potion")[2],
            98f, Magic.FindInfoByName("Big Heal Potion")),
        new PotionInfo("Stranger", "Just use it!!",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Potion")[39],
            99.9f, Magic.FindInfoByName("Random Magic")),
        new PotionInfo("Legendary Mana Elixir", "Increase your wisdom permanently",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Potion")[27],
            99.99f, Magic.FindInfoByName("WisUpgrade")),

        //Food(10 ~ 11)
        new PotionInfo("Donut", "It smells good",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Food")[41],
            98f, Magic.FindInfoByName("Reduce Hunger2")),
        new PotionInfo("Chocolate", "It recovers HP and Hunger",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Food")[39],
            99f, Magic.FindInfoByName("BothRecover1")),
        new PotionInfo("Food Set", "It recovers HP and Hunger",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Food")[33],
            99f, Magic.FindInfoByName("BothRecover2")),
        new PotionInfo("Medic Kit", "It recovers HP and Hunger",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Food")[34],
            99f, Magic.FindInfoByName("BothRecover3")),

        //monstermeat
        new PotionInfo("Monster Meat1", "It smells...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Flesh")[0],
            99f, Magic.FindInfoByName("MonsterMeat")),
        new PotionInfo("Monster Meat2", "It smells...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Flesh")[3],
            99f, Magic.FindInfoByName("MonsterMeat")),
        new PotionInfo("Monster Meat3", "It smells...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Flesh")[16],
            99f, Magic.FindInfoByName("MonsterMeat")),
        new PotionInfo("Monster Meat4", "It smells...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Flesh")[15],
            99f, Magic.FindInfoByName("MonsterMeat")),
        new PotionInfo("Monster Meat5", "It smells...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Flesh")[11],
            99f, Magic.FindInfoByName("MonsterMeat")),
            
        new PotionInfo("Soul Heart", "It makes you stronger",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Flesh")[6],
            99f, Magic.FindInfoByName("BossMeat")),

        //Scroll()
        new ScrollInfo("Warp Scroll", "You can fly!! But...",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Scroll")[37],
            99.8f, Magic.FindInfoByName("Warp")),
        new ScrollInfo("Break Walls Scroll", "Break Walls",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Scroll")[36],
            99.99f, Magic.FindInfoByName("Break Walls")),
        new ScrollInfo("Show Map Scroll", "Show Map",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Scroll")[38],
            99.8f, Magic.FindInfoByName("Show Map")),
        new ScrollInfo("Fear Scroll", "Monsters will run away from you",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Scroll")[7],
            99.9f, Magic.FindInfoByName("Fear")),
        new ScrollInfo("Tornado Scroll", "Tornado Spell",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Scroll")[13],
            99.9f, Magic.FindInfoByName("Tornado")),
        
       

        /*Util*/
        //Bag()
        new ScrollInfo("Bag", "It increase your inventory size",
            Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Chest0")[12],
            99.8f, Magic.FindInfoByName("IncreaseInven")),
    };
    

    public static int FindInfoById(string name) {
        for(int i = 0; i < info.Length; i++) {
            if(info[i].name.Equals(name)) return i;
        }
        return -1;
    }

    public bool IsEquipment() {
        return this is EquipmentInfo;
    }

    public bool IsConsumable() {
        return !IsEquipment();
    }
}