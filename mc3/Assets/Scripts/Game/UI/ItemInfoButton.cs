using UnityEngine;
using UnityEngine.UI;

public class ItemInfoButton : MonoBehaviour
{
    int reference;
    int index;
    Item item;
    // Start is called before the first frame update

    public void SetRef(int refer)
    {
        reference = refer;
    }

    public void GetItem(Item getitem)
    {
       item = getitem;
    }

    public void NoticeIndex(int getindex)
    {
        index = getindex;
    }

    public void OnClickPositive()
    {
        if(reference == 1) // 인벤창을 통해 들어온 것
        {
            Consumable maybeconsum = item as Consumable;
            if (maybeconsum != null)
            {
                if(GameManager.instance.player.Consume(index))
                {
                    ExitInven();
                } else {
                    Toast.Make("It's not your turn", 0.5f);
                }
            }
            else
            {
                if(item.info is WeaponInfo)
                {
                    ChangeEquip(1);
                }
                else if (item.info is SubWeaponInfo)
                {
                    ChangeEquip(2);
                }
                else if (item.info is HelmetInfo)
                {
                    ChangeEquip(3);
                }
                else if (item.info is ArmorInfo)
                {
                    ChangeEquip(4);
                }
                else if (item.info is ShoesInfo)
                {
                    ChangeEquip(5);
                }
                else if (item.info is GlovesInfo)
                {
                    ChangeEquip(6);
                }
                else if (item.info is AccessoryInfo)
                {
                    ChangeEquip(7);
                }
            }
        }
        else if(reference == 2) // 단축키 창을 통해 들어온것
        {
            ChangeQuickSlot(1);
        }
        else if(reference == 3) // 장비창을 통해 들어온것
        {
            ExitInfoPage();
        }
        else if (reference == 4) // 단축키 창을 통해 들어온것
        {
            ChangeQuickSlot(2);
        }
    }

    public void OnClickNegative()
    {

        if (reference == 1)
        {
            if(GameManager.instance.DropItem(index))
            {
                ExitInven();
            } else {
                Toast.Make("It's not your turn", 0.5f);
            }
        }
        else if(reference == 2 || reference == 4)
        {
            ExitInfoPage();
        }
        else if(reference == 3)
        {
            PlayerStatus player = GameManager.instance.player;
            if (player.inventory.Count < player.bag * PlayerStatus.BAG_SIZE)
            {
                if (GameManager.instance.PassTurnOfPlayer(1))
                {
                    UnitEquip equiped = player.equip;
                    if (equiped.weapon == item)
                    {
                        player.inventory.Add(item);
                        equiped.weapon = null;
                        GameObject.Find("EquipInvenWeapon").GetComponent<WeaponInvenButton>().SelectCurrentEquip();
                        GameObject.Find("ChangeWeaponButton").GetComponent<ChangeWeaponInfo>().UnEquipped();
                    }
                    else if (equiped.subWeapon == item)
                    {
                        player.inventory.Add(item);
                        equiped.subWeapon = null;
                        GameObject.Find("EquipInvenSubWeapon").GetComponent<SubWeaponInvenButton>().SelectCurrentEquip();
                        GameObject.Find("ChangeWeaponButton").GetComponent<ChangeWeaponInfo>().UnEquipped();
                    }
                    else if (equiped.helmet == item)
                    {
                        player.inventory.Add(item);
                        equiped.helmet = null;
                        GameObject.Find("EquipInvenHelmet").GetComponent<HelmetInvenButton>().SelectCurrentEquip();
                    }
                    else if (equiped.armor == item)
                    {
                        player.inventory.Add(item);
                        equiped.armor = null;
                        GameObject.Find("EquipInvenArmor").GetComponent<ArmorInvenButton>().SelectCurrentEquip();
                    }
                    else if (equiped.shoes == item)
                    {
                        player.inventory.Add(item);
                        equiped.shoes = null;
                        GameObject.Find("EquipInvenShoes").GetComponent<ShoesInvenButton>().SelectCurrentEquip();
                    }
                    else if (equiped.gloves == item)
                    {
                        player.inventory.Add(item);
                        equiped.gloves = null;
                        GameObject.Find("EquipInvenGloves").GetComponent<GlovesInvenButton>().SelectCurrentEquip();
                    }
                    else if (equiped.accessory1 == item)
                    {
                        player.inventory.Add(item);
                        equiped.accessory1 = null;
                        GameObject.Find("EquipInvenAcc1").GetComponent<Acc1InvenButton>().SelectCurrentEquip();
                        GameObject.Find("QuickSlotAcc1").GetComponent<QuickSlotAcc>().Initializing(1);
                    }
                    else if (equiped.accessory2 == item)
                    {
                        player.inventory.Add(item);
                        equiped.accessory2 = null;
                        GameObject.Find("EquipInvenAcc2").GetComponent<Acc2InvenButton>().SelectCurrentEquip();
                        GameObject.Find("QuickSlotAcc2").GetComponent<QuickSlotAcc>().Initializing(2);
                    }
                    ExitInven();
                }
            }
            else
            {
                Toast.Make("Bag is full!");
                ExitInfoPage();
            }
        }
    }


    public void ChangeQuickSlot(int i)
    {
        string quickslotname = "QuickSlotItem" + i;
        string quickslotimagename = "QuickSlotImage" + i;
        if(i == 1)
        {
            QuickSlotItem1 quickslotscript = GameObject.Find(quickslotname).GetComponent<QuickSlotItem1>();
            quickslotscript.SetItem(item, index);
            quickslotscript.ShowImage(GameObject.Find(quickslotimagename).GetComponent<Image>());
        }
        else
        {
            QuickSlotItem2 quickslotscript = GameObject.Find(quickslotname).GetComponent<QuickSlotItem2>();
            quickslotscript.SetItem(item, index);
            quickslotscript.ShowImage(GameObject.Find(quickslotimagename).GetComponent<Image>());
        }
        ExitInven();
    }

    public void ChangeEquip(int i)
    {
        if (GameManager.instance.PassTurnOfPlayer(1))
        {
            PlayerStatus player = GameManager.instance.player;
            UnitEquip unitEquip = player.equip;
            if (i == 1)
            {
                Item pastitem = unitEquip.weapon;
                unitEquip.weapon = item;
                player.inventory.RemoveAt(index);
                if (pastitem != null)
                {
                    player.inventory.Add(pastitem);
                }
                GameObject changeweaponbutton = GameObject.Find("ChangeWeaponButton");
                changeweaponbutton.GetComponent<ChangeWeaponInfo>().Equipped();
                GameObject equipslot = GameObject.Find("EquipInvenWeapon");
                equipslot.GetComponent<WeaponInvenButton>().SelectCurrentEquip();
            }
            else if (i == 2)
            {
                Item pastitem = unitEquip.subWeapon;
                unitEquip.subWeapon = item;
                player.inventory.RemoveAt(index);
                if (pastitem != null)
                {
                    player.inventory.Add(pastitem);
                }
                GameObject changeweaponbutton = GameObject.Find("ChangeWeaponButton");
                changeweaponbutton.GetComponent<ChangeWeaponInfo>().Equipped();
                GameObject equipslot = GameObject.Find("EquipInvenSubWeapon");
                equipslot.GetComponent<SubWeaponInvenButton>().SelectCurrentEquip();
            }
            else if (i == 3)
            {
                Item pastitem = unitEquip.helmet;
                unitEquip.helmet = item;
                player.inventory.RemoveAt(index);
                if (pastitem != null)
                {
                    player.inventory.Add(pastitem);
                }
                GameObject equipslot = GameObject.Find("EquipInvenHelmet");
                equipslot.GetComponent<HelmetInvenButton>().SelectCurrentEquip();
            }
            else if (i == 4)
            {
                Item pastitem = unitEquip.armor;
                unitEquip.armor = item;
                player.inventory.RemoveAt(index);
                if (pastitem != null)
                {
                    player.inventory.Add(pastitem);
                }
                GameObject equipslot = GameObject.Find("EquipInvenArmor");
                equipslot.GetComponent<ArmorInvenButton>().SelectCurrentEquip();
            }
            else if (i == 5)
            {
                Item pastitem = unitEquip.shoes;
                unitEquip.shoes = item;
                player.inventory.RemoveAt(index);
                if (pastitem != null)
                {
                    player.inventory.Add(pastitem);
                }
                GameObject equipslot = GameObject.Find("EquipInvenShoes");
                equipslot.GetComponent<ShoesInvenButton>().SelectCurrentEquip();
            }
            else if (i == 6)
            {
                Item pastitem = unitEquip.gloves;
                unitEquip.gloves = item;
                player.inventory.RemoveAt(index);
                if (pastitem != null)
                {
                    player.inventory.Add(pastitem);
                }
                GameObject equipslot = GameObject.Find("EquipInvenGloves");
                equipslot.GetComponent<GlovesInvenButton>().SelectCurrentEquip();
            }
            else if (i == 7)
            {
                Item pastacc1 = unitEquip.accessory1;
                Item pastacc2 = unitEquip.accessory2;
                if (pastacc1 == null)
                {
                    unitEquip.accessory1 = item;
                    player.inventory.RemoveAt(index);
                    GameObject equipslot = GameObject.Find("QuickSlotAcc1");
                    equipslot.GetComponent<QuickSlotAcc>().SelectCurrentEquip(item, 1);
                }
                else if (pastacc2 == null)
                {
                    unitEquip.accessory2 = item;
                    player.inventory.RemoveAt(index);
                    GameObject equipslot = GameObject.Find("QuickSlotAcc2");
                    equipslot.GetComponent<QuickSlotAcc>().SelectCurrentEquip(item, 2);
                }
                else
                {
                    unitEquip.accessory1 = item;
                    player.inventory.RemoveAt(index);
                    player.inventory.Add(pastacc1);
                    GameObject equipslot = GameObject.Find("QuickSlotAcc1");
                    equipslot.GetComponent<QuickSlotAcc>().SelectCurrentEquip(item, 1);
                }
            }
            ExitInven();
        }
    }

    public void ExitInven()
    {
        GameObject.Find("Inventory(Clone)").GetComponent<Inventory>().OnClick();
    }

    public void ExitInfoPage()
    {
        Destroy(GameObject.Find("ItemInfoPage(Clone)"));
    }
}
