using UnityEngine;

public class GlovesInfo : EquipmentInfo
{
    public GlovesInfo(string inputname, string desc, Sprite inputsprite, UnitStatus unitStatus, float unique)
    {
        this.name = inputname;
        this.description = desc;
        this.sprite = inputsprite;
        this.magic = -1;
        this.weight = 0;
        this.uniqueness = unique;
        this.status = unitStatus;
    }
}