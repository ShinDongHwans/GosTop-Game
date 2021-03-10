using UnityEngine;

public class AccessoryInfo : EquipmentInfo {
    public AccessoryInfo(string inputname, string desc, Sprite inputsprite, UnitStatus unitStatus, float unique, int magic)
    {
        this.name = inputname;
        this.description = desc;
        this.sprite = inputsprite;
        this.status = unitStatus;
        this.weight = 0;
        this.uniqueness = unique;
        this.magic = magic;
    }
}