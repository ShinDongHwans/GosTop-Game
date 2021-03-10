using UnityEngine;
using UnityEngine.UI;

public class QuickSlotItem1 : MonoBehaviour
{
    public Consumable quickItem;
    public Sprite sprite;


    public void WhenTouch()
    {
        if (quickItem != null)
        {
            int i;
            for(i = 0; i < GameManager.instance.player.inventory.Count; i++)
            {
                if (GameManager.instance.player.inventory[i] == quickItem) break;
            }
            if(i < GameManager.instance.player.inventory.Count)
                GameManager.instance.player.Consume(i);
        }
        else
        {
            GameObject currentinven = GameObject.Find("Inventory(Clone)");
            if (currentinven == null)
            {
                GameObject inventory = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/Inventory"));
                inventory.GetComponent<Inventory>().SetRef(2);
                inventory.GetComponent<Inventory>().ShowInvenPage(1);
                inventory.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
            else
                currentinven.GetComponent<Inventory>().OnClick();
        }
    }

    public void Initializing(int i)
    {
        quickItem = null;
        sprite = Resources.Load<Sprite>("Sprites/DawnLike/GUI/QuickSlotDefault");
        ShowImage(GameObject.Find("QuickSlotImage" + i).GetComponent<Image>());
    }

    public void ShowImage(Image image)
    {
        image.sprite = sprite;
    }

    public void SetItem(Item item, int i)
    {
        quickItem = item as Consumable;
        sprite = ItemInfo.info[item.kind].sprite;
    }
}
