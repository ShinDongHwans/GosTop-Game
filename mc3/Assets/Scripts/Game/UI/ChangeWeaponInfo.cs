using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponInfo : MonoBehaviour
{
    public Item currentweapon ;
    private int _which = -1;
    public int which {
        get { return _which; }
        internal set { _which = value; } }
    

    void Start()
    {
        ChooseAnyWeapon();
    }

    public void Equipped()
    {
        if(which == -1) ChooseAnyWeapon();
        else ChooseWeapon(which);
    }

    public void UnEquipped()
    {
        ChooseAnyWeapon();
    }

    public void ChangeWeapon()
    {
        if(which == -1) ChooseAnyWeapon();
        else ChooseWeapon(1 - which);
    }

    public void ShowCurrentWeapon()
    {
        GameObject imageview = GameObject.Find("SelectedWeaponImage");
        Image image = imageview.GetComponent<Image>();
        if (which != -1 && currentweapon.kind != 0)
        {
            image.sprite = currentweapon.info.sprite;
        }
        else
        {
            image.sprite = Resources.Load<Sprite>("Sprites/DawnLike/GUI/DefaultImage") ;
        }
    }


    private bool ChooseWeapon(int newWhich) {
        var e = GameManager.instance.player.equip;
        Item mainWeapon = e.weapon;
        Item subWeapon = e.subWeapon;
        currentweapon = null;
        if(newWhich == 0) {
            if(mainWeapon == null) return false;
            currentweapon = mainWeapon;
        } else if(newWhich == 1){
            if(subWeapon == null) return false;
            currentweapon = subWeapon;
        }
        which = newWhich;
        ShowCurrentWeapon();
        GameObject.Find("MagicButton").GetComponent<UseMagicButton>().SelectCurrentMagic();
        return true;
    }
    private void ChooseAnyWeapon() { var t = ChooseWeapon(0) || ChooseWeapon(1) || ChooseWeapon(-1); }
}
