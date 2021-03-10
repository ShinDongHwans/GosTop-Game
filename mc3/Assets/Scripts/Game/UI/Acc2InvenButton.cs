using UnityEngine;
using UnityEngine.UI;

public class Acc2InvenButton : MonoBehaviour
{

    public Item currentequip= null;


    void Start()
    {
        SelectCurrentEquip();
    }

    public void SelectCurrentEquip()
    {
        currentequip = GameManager.instance.player.equip.accessory2;
        ShowCurrentEquip();
    }

    // Update is called once per frame
    void Update()
    {
        ShowCurrentEquip();
    }

    public void ShowCurrentEquip()
    {
        GameObject imageview = GameObject.Find("Acc2Image");
        Image image = imageview.GetComponent<Image>();
        GameObject textview = GameObject.Find("Acc2Text");
        var text = textview.GetComponent<Text>(); if (currentequip != null)
        {
            Equipment myequip = currentequip as Equipment;
            image.sprite = ItemInfo.info[currentequip.kind].sprite;
            text.text = "+" + myequip.upgrade.ToString();
        }
        else
        {
            image.sprite = Resources.Load<Sprite>("Sprites/DawnLike/GUI/EquipButtonDefault");
            text.text = "";
        }
    }

    public void OnClick()
    {
        if(currentequip != null)
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

            image.sprite = ItemInfo.info[currentequip.kind].sprite;
            name.text = ItemInfo.info[currentequip.kind].name;
            script.text = ItemInfo.info[currentequip.kind].description;
            button1.GetComponent<ItemInfoButton>().SetRef(3);
            button1.GetComponent<ItemInfoButton>().GetItem(currentequip);
            button2.GetComponent<ItemInfoButton>().SetRef(3);
            button2.GetComponent<ItemInfoButton>().GetItem(currentequip);

            text1.text = "Back";
            text2.text = "UnEquip";

            iteminfoPage.transform.SetParent(canvas.transform, false);
            iteminfoPage.transform.position = canvas.transform.position;
        }
    }
}
