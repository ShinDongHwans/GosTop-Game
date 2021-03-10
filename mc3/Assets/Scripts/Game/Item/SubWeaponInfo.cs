using UnityEngine;

public class SubWeaponInfo : EquipmentInfo {


    public AttackRange attackRange;

    public SubWeaponInfo(string inputname, string desc, Sprite inputsprite, int weight, UnitStatus unitStatus, AttackRange range, float unique, int magic)
    {
        this.name = inputname;
        this.description = desc;
        this.sprite = inputsprite;
        this.magic = magic;
        this.weight = weight;
        this.status = unitStatus;
        this.uniqueness = unique;
        this.attackRange = range;
    }
}