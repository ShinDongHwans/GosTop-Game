public class UnitEquip {
    public Item weapon;
    public Item subWeapon;
    public Item helmet;
    public Item armor;
    public Item shoes;
    public Item accessory1;
    public Item accessory2;
    public Item gloves;

    public Equipment[] ToArray() {
        return new Equipment[]{
            (Equipment)weapon, (Equipment)subWeapon,
            (Equipment)helmet, (Equipment)armor,
            (Equipment)shoes, (Equipment)gloves,
            (Equipment)accessory1, (Equipment)accessory2,
        };
    }
}