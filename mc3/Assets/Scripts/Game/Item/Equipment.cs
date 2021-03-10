using UnityEngine;

public class Equipment : Item {
    public int upgrade;
    public int maxcharge
    {
        get
        {
            return 3 + (upgrade/2);
        }
    }

    public int charge;
    public int leftTurnsForRecoverCharge;
    public const int turnsForRecoverCharge = 10;

    public Equipment(int kind, int upgrade) {
        this.kind = kind;
        this.upgrade = upgrade;
        this.charge = maxcharge;
    }

    public Equipment(int kind) : this(kind, 0) {}

    public UnitStatus status {
        get {
            UnitStatus st = ((EquipmentInfo)info).status;
            if(info is WeaponInfo) {
                st += new UnitStatus(0, upgrade, upgrade, 0, 0, 0.01f * upgrade);
            } else {
                st += new UnitStatus(upgrade, 0, 0, upgrade, 0.01f * upgrade, 0);
            }
            return st;
        }
    }

    public override string GetName() {
        return "+" + upgrade + " " + info.name;
    }

    public override void OnTurnEnd() {
        if(charge < maxcharge) {
            if(--leftTurnsForRecoverCharge <= 0) {
                charge = Mathf.Min(charge + 1, maxcharge);
                leftTurnsForRecoverCharge = turnsForRecoverCharge;
            }
        }
    }
}