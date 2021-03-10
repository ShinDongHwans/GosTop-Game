using UnityEngine;

public class WeaponInfo : EquipmentInfo 
{

    public AttackRange attackRange;

    public WeaponInfo(string name, string desc, Sprite sprite, int weight, UnitStatus unitStatus, AttackRange range, float unique, int magic) 
    {
        this.name = name;
        this.description = desc;
        this.sprite = sprite;
        this.magic = magic;
        this.weight = weight;
        this.status = unitStatus;
        this.uniqueness = unique;
        this.attackRange = range;
    }
}