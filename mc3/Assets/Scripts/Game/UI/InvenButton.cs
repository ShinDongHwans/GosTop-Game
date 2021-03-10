using UnityEngine;
using UnityEngine.UI;

public class InvenButton : MonoBehaviour
{
    public int index;
    Item item = null;
    int reference;


    public void FindContent(int i)
    {
        if (GameManager.instance.player.inventory.Count > i - 1)
        {
            item = GameManager.instance.player.inventory[i - 1];
        } else {
            item = null;
        }
        index = i;
        ShowItem();
    }

    public void ShowItem()
    {
        string imagename = "InvenImage" + ((index-1)%16+1);
        GameObject imageview = GameObject.Find(imagename);
        Image image = imageview.GetComponent<Image>();
        string textname = "ItemText" + ((index - 1) % 16 + 1);
        GameObject textview = GameObject.Find(textname);
        var text = textview.GetComponent<Text>();
        if (item != null)
        {
            image.sprite = item.info.sprite;
            Consumable maybeconsum = item as Consumable;
            
            if (maybeconsum != null)
            {
                text.text = maybeconsum.count.ToString();
            }
            else
            {
                Equipment maybeequip = item as Equipment;
                if (maybeequip != null)
                {
                    text.text = "+" + maybeequip.upgrade.ToString();
                }
            }
        }
        else
        {
            image.sprite = Resources.Load<Sprite>("Sprites/DawnLike/GUI/ItemButtonDefault");
            text.text = "";
        }
    }


    public void SetRef(int refer)
    {
        reference = refer;
    }

    public void OnClick()
    {
        if (item != null)
        {
            var canvas = GameObject.Find("Canvas");
            GameObject iteminfoPage = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/ItemInfoPage"));
            GameObject iteminfoImage = GameObject.Find("ItemImage");
            GameObject iteminfoName = GameObject.Find("ItemName");
            GameObject iteminfoScript = GameObject.Find("ItemScript");
            GameObject iteminfoButton1 = GameObject.Find("ItemInfoButton1");
            GameObject iteminfoText1 = GameObject.Find("ItemInfoText1");
            GameObject iteminfoButton2 = GameObject.Find("ItemInfoButton2");
            GameObject iteminfoText2 = GameObject.Find("ItemInfoText2");

            Image image = iteminfoImage.GetComponent<Image>();
            Text name = iteminfoName.GetComponent<Text>();
            Text script = iteminfoScript.GetComponent<Text>();
            Button button1 = iteminfoButton1.GetComponent<Button>();
            Text text1 = iteminfoText1.GetComponent<Text>();
            Button button2 = iteminfoButton2.GetComponent<Button>();
            Text text2 = iteminfoText2.GetComponent<Text>();

            image.sprite = item.info.sprite;
            name.text = item.info.name;
            script.text = item.info.description;
            button1.GetComponent<ItemInfoButton>().SetRef(reference);
            button1.GetComponent<ItemInfoButton>().NoticeIndex(index - 1);
            button1.GetComponent<ItemInfoButton>().GetItem(item);
            button2.GetComponent<ItemInfoButton>().SetRef(reference);
            button2.GetComponent<ItemInfoButton>().NoticeIndex(index - 1);
            button2.GetComponent<ItemInfoButton>().GetItem(item);

            if (reference == 1)
            {
                Consumable maybeconsum = item as Consumable;
                if (maybeconsum != null)
                {
                    text1.text = "Use";
                }
                else
                {
                    Equipment maybeequip = item as Equipment;
                    if (maybeequip != null)
                    {
                        text1.text = "Equip";
                    }
                    else
                        Destroy(iteminfoButton1);
                }
                text2.text = "Drop";
            }
            else if(reference == 2||reference == 4)
            {
                Consumable maybeconsum = item as Consumable;
                if (maybeconsum != null)
                {
                    text1.text = "Set";
                    text2.text = "Back";
                }
                else
                {
                    Equipment maybeequip = item as Equipment;
                    if (maybeequip != null)
                    {
                        Destroy(iteminfoPage);
                        return;
                    }
                }
            }

            iteminfoPage.transform.SetParent(canvas.transform, false);
            iteminfoPage.transform.position = canvas.transform.position;
        }
    }
}
