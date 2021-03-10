﻿using UnityEngine;

public class HelmetInfo : EquipmentInfo
{
    public HelmetInfo(string inputname, string desc, Sprite inputsprite, int inputweight, UnitStatus unitStatus, float unique)
    {
        this.name = inputname;
        this.description = desc;
        this.sprite = inputsprite;
        this.status = unitStatus;
        this.weight = inputweight;
        this.uniqueness = unique;
        this.magic = -1;
    }
}
