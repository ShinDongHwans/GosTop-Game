using UnityEngine;

public class UnitStatus {
    public int maxHP;
    public int str;
    public int atk;
    public int def;
    public float agi;
    public float acc;

    public UnitStatus(int maxHP, int str, int atk, int def, float agi, float acc) {
        this.maxHP = maxHP;
        this.str = str;
        this.atk = atk;
        this.def = def;
        this.agi = agi;
        this.acc = acc;
    }

    public UnitStatus() : this(0,0,0,0,0,0) {}


    public static UnitStatus CalculateStatusFromStat(UnitStat stat) {
        var status = new UnitStatus();
        status.maxHP = stat.health * 2;
        status.str = stat.power;
        status.atk = stat.power / 5;
        status.def = stat.health / 5;
        status.agi = 0.05f + stat.dex * Mathf.Log(stat.dex)/4600;
        status.acc = 0.80f + stat.dex / 200f;
        return status;
    }

    public static UnitStatus CalculateStatusFromStatNPC(UnitStat stat)
    {
        var status = new UnitStatus();
        status.maxHP = stat.health * 2;
        status.atk = stat.power;
        status.def = stat.health;
        status.agi = 0.05f + stat.dex*stat.dex/2000;
        status.acc = 0.7f + stat.dex/100f - stat.power/1000f;
        return status;
    }

    public static UnitStatus operator +(UnitStatus a, UnitStatus b) {
        return new UnitStatus(
            a.maxHP + b.maxHP,
            a.str + b.str,
            a.atk + b.atk,
            a.def + b.def,
            a.agi + b.agi,
            a.acc + b.acc
        );
    }


    public float CalculateAttackHitProbabilty(UnitStatus target)
    {
        /* Bound In 0 - 1 */
        float v = this.acc - target.agi;
        return v;
    }

    public bool TryAttackHitText(UnitStatus target) {
        float probability = CalculateAttackHitProbabilty(target);
        return Random.Range(0f, 1f) <= probability;
    }
     
    public int CalculateAttackDamage(UnitStatus target) {
        int v = this.atk^2 / target.def;
        if(v < 1) v = 1;
        return v;
    }

    public int CalculateAttackedDamage(int atk)
    {    
        int v = atk^2 / this.def;
        if(v < 1) v = 1;
        return v;
    }
}