using UnityEngine;
using UnityEngine.UI;

public class QuickSlotAcc : MonoBehaviour
{
    public Item quickAcc;
    public Sprite sprite;

    public void WhenTouch()
    {
        if(quickAcc != null)
            if(ItemInfo.info[quickAcc.kind]!= null) {
                if (((AccessoryInfo)quickAcc.info).magic != -1) {
                    GameManager.instance.TryInvokeItem(quickAcc);
                }
            }
    }

    public void Initializing(int i)
    {
        quickAcc = null;
        sprite = Resources.Load<Sprite>("Sprites/DawnLike/GUI/QuickSlotDefault");
        GameObject.Find("QuickAccImage" + i).GetComponent<Image>().sprite = sprite;
    }

    public void SelectCurrentEquip(Item item, int i)
    {
        quickAcc = item;
        sprite = ItemInfo.info[item.kind].sprite;
        string imagename = "QuickAccImage" + i;
        GameObject.Find(imagename).GetComponent<Image>().sprite = sprite;
    }
}
