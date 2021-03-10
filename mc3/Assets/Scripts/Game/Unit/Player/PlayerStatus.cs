using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status {
    public static int startingJob;

    public override string GetName() { return "Player"; }

    public int level {
        get; internal set;
    }
    public int currentExp {
        get; internal set;
    }
    public int maxExp {
        get { return level * (int)(2 + Mathf.Log10(level)) * 10; }
    }

    /* Status */
    public UnitStatus baseStatus {
        get { return UnitStatus.CalculateStatusFromStat(stat); }
    }

    public UnitStatus realStatus {
        get {
            UnitStatus st = baseStatus;
            if(equip != null) {
                int w = currentWeaponIndex;
                foreach(var x in equip.ToArray()) {
                    if(x == null) continue;
                    else if(w == 1 && x.info is WeaponInfo) continue;
                    else if(w == 0 && x.info is SubWeaponInfo) continue;
                    st += x.status;
                }
            }
            return st;
        }
    }
    public override UnitStatus status {
        get { return realStatus; }
    }

    public UnitEquip equip;
    
    public int currentWeaponIndex {
        get {
            if(equip == null) return -1;
            var cwb = GameObject.Find("ChangeWeaponButton");
            if(cwb == null) return -1;
            return cwb.GetComponent<ChangeWeaponInfo>().which;
        }
    }

    public Item currentWeapon {
        get {
            var idx = currentWeaponIndex;
            switch(idx) {
                case 0: return equip.weapon;
                case 1: return equip.subWeapon;
                default: return null;
            }
        }
    }

    public AttackRange attackRange {
        get {
            var cw = currentWeapon;
            if(cw == null) {
                return new AttackRange(1, 1);
            } else if(cw.info is WeaponInfo) {
                return ((WeaponInfo)cw.info).attackRange;
            } else {
                return ((SubWeaponInfo)cw.info).attackRange;
            }
        }
    }

    //배고픔
    public float _hungry;
    public float hungry {
        get { return _hungry; }
        internal set { _hungry = Mathf.Min(MAX_HUNGRY, value); } }
    private const float MAX_HUNGRY = 100f;

    public string hungryMessage {
        get {
            var r = hungry / MAX_HUNGRY;
            if(r >= 0.75f) return "Full";
            else if(r >= 0.4f) return "Normal";
            else if(r >= 0.2f) return "Hungry";
            else return "Starving";
        }
    }

    public const int BAG_SIZE = 16;
    public int bag;
    public List<Item> inventory;

    public PlayerStatus() : base()
    {   
        if (0 <= startingJob && startingJob < JOB_DEFAULT_SETTINGS.Length)
        {
            StatDetermininationByJob(startingJob);
        }
        else
            throw new System.ArgumentException("job sdllection number is out of range 0~4");

        equip = new UnitEquip();
        bag = 1;
        inventory = new List<Item>(bag * 16);
        InitItemPackByJob(startingJob);

        level = 1;

        hp = status.maxHP;
        hungry = 100f;
        currentExp = 0;
    }

    public struct JobDefaultSettings {
        public string name;
        public UnitStat stat;
        public int[] resistances;
        public Sprite[] sprites;

        public JobDefaultSettings(string name, int power, int wisdom, int dex, int health, int[] resistances, int spriteIndex) {
            this.name = name;
            this.stat = new UnitStat(power, wisdom, dex, health);
            this.resistances = resistances;
            this.sprites = new Sprite[] {
                Util.LoadSprite("DawnLike/Characters/Player", 0, spriteIndex),
                Util.LoadSprite("DawnLike/Characters/Player", 1, spriteIndex),
            };
        }
    }

    public static JobDefaultSettings[] JOB_DEFAULT_SETTINGS = new JobDefaultSettings[] {
        new JobDefaultSettings("Adventurer", 10, 0, 7, 10, new int[] {0, 0, 0, 0, 0, 0, 30}, 4),
        new JobDefaultSettings("Magician", 5, 10, 7, 5, new int[] {15, 0, 0, 0, 0, 10, -10}, 6),
        new JobDefaultSettings("Thief", 7, 3, 10, 7, new int[] {0, 20, 10, 0, 0, 0, 0}, 26),
        new JobDefaultSettings("Archer", 7, 1, 7, 7, new int[] {0, 0, 10, 0, 0, 0, 0}, 18),
        new JobDefaultSettings("Smith", 10, 3, 5, 7, new int[] {0, -10, 0, 10, 0, 10, 0}, 53),
        new JobDefaultSettings("Summoner", 10, 10, 1, 1, new int[] {0, -10, 0, -10, 0, -10, 0}, 76),
    };

    private void StatDetermininationByJob(int job) {
        startingJob = job;
        stat = new UnitStat(JOB_DEFAULT_SETTINGS[job].stat);
        resistances = JOB_DEFAULT_SETTINGS[job].resistances;
    }

    //직업별로 아이템 추가
    public void InitItemPackByJob(int job)
    {
        if (job == 0)
        {
            equip.weapon = new Equipment(ItemInfo.FindInfoById("Hobit Sword")); 
            Item item;
            item = new Consumable(ItemInfo.FindInfoById("Soul Heart"));
            inventory.Add(item);
        }
        else if (job == 1) // 마법사
        {
            equip.weapon = new Equipment(Random.Range(10, 13));
            equip.subWeapon = new Equipment(Random.Range(21, 23));
        }
        else if (job == 2) // 도적
        {
            equip.weapon = new Equipment(ItemInfo.FindInfoById("Ninja Star"));
        }
        else if (job == 3) // 궁수
        {
            equip.weapon = new Equipment(ItemInfo.FindInfoById("Wooden Bow"));
        }
        else if (job == 4) // 대장장이
        {
            bag = 6;
            for(int i=0; i<ItemInfo.info.Length; i++)
            {
                Item item;
                if(ItemInfo.info[i] is EquipmentInfo)
                {
                    item = new Equipment(i, 5);
                } else
                {
                    item = new Consumable(i, 5);
                }
                inventory.Add(item);
            }
            LevelManager.themeFloor = new int[] { -1, 1, 2, 3 };
        }
        return;
    }

    public void OnTurnEnd() {
        _hp += 0.1f;
        if(_hp > status.maxHP) _hp = status.maxHP;
        hungry -= 0.3f;
        if(hungry < 0) hungry = 0;
        var r = hungry / MAX_HUNGRY;
        if(r <= 0.1f) Damaged(3);
        else if(r <= 0.3f) Damaged(1);

        var e = equip.ToArray();
        foreach(var x in e) {
            if(x != null)
                x.OnTurnEnd();
        }
        foreach(var x in inventory) {
            if(x != null)
                x.OnTurnEnd();
        }
    }

    public void OnLevelUp() {
        level++;
        if(startingJob == 0)
            stat += new UnitStat(3, 0, 1, 2);
        else if (startingJob == 1)
            stat += new UnitStat(1, 3, 1, 1);
        else  if (startingJob == 2)
            stat += new UnitStat(2, 1, 3, 1);
        else if (startingJob == 3)
            stat += new UnitStat(2, 1, 2, 2);
        else if (startingJob == 4)
            stat += new UnitStat(3, 0, 0, 3);
    }

    public void Eat(float gain)
    {
        if (hungry + gain > 100f)
            hungry = 100f;
        else
            hungry += gain;
    }


    public void GainExp(int d) {
        currentExp += d;
        while(currentExp >= maxExp) {
            currentExp -= maxExp;
            OnLevelUp();
        }
    }

    public bool Consume(int index)
    {
        if (!GameManager.instance.PassTurnOfPlayer(1)) return false;

        if (index < 0 || index >= inventory.Count || !(inventory[index] is Consumable))
            return false;
        var consumable = inventory[index] as Consumable;
        var itemInfo = ItemInfo.info[inventory[index].kind];
        int magic;
        if(itemInfo is PotionInfo) magic = ((PotionInfo)itemInfo).magic;
        else if(itemInfo is ScrollInfo) magic = ((ScrollInfo)itemInfo).magic;
        else return false;
        Magic.info[magic].Invoke(this);

        if(itemInfo is PotionInfo) Log.Make("You quaff " + itemInfo.name);
        else if(itemInfo is ScrollInfo) Log.Make("You read " + itemInfo.name);

        if (--consumable.count <= 0) {
            inventory.RemoveAt(index);
            var qs1 = GameObject.Find("QuickSlotItem1").GetComponent<QuickSlotItem1>();
            var qs2 = GameObject.Find("QuickSlotItem2").GetComponent<QuickSlotItem2>();
            if (qs1.quickItem == consumable) qs1.Initializing(1);
            if(qs2.quickItem == consumable) qs2.Initializing(2);
        }
        return true;
    }

}
