using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int currentindex;
    int reference;
    // Start is called before the first frame update

    void Start()
    {
        GameObject.Find("BagImage").GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Chest1")[1];
    }

    private void OnDestroy()
    {
        GameObject.Find("BagImage").GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/DawnLike/Items/Chest0")[1];
    }

    public void ShowInvenPage(int index)
    {
        currentindex = index;
        GameObject.Find("TextCurrentPage").GetComponent<Text>().text = "" + index + "/" + GameManager.instance.player.bag;
        for (int i = 16 * (currentindex - 1); i < 16 * currentindex; i++)
        {
            string objectname = "InventoryButton" + (i % 16 + 1);
            GameObject itembutton = GameObject.Find(objectname);
            itembutton.GetComponent<InvenButton>().SetRef(reference);
            itembutton.GetComponent<InvenButton>().FindContent(i + 1);
        }
    }

    public void SetRef(int refer)
    {
        reference = refer;
    }

    public void OnRightClick()
    {
        var inven = GameObject.Find("Inventory(Clone)").GetComponent<Inventory>();
        int newIndex = (inven.currentindex) % GameManager.instance.player.bag + 1;
        inven.ShowInvenPage(newIndex);
    }

    public void OnLeftClick()
    {
        var inven = GameObject.Find("Inventory(Clone)").GetComponent<Inventory>();
        int newIndex = (inven.currentindex + GameManager.instance.player.bag - 2) % GameManager.instance.player.bag + 1;
        inven.ShowInvenPage(newIndex);
    }

    public void OnClick()
    {
        if (GameObject.Find("ItemInfoPage(Clone)"))
        {
            Destroy(GameObject.Find("ItemInfoPage(Clone)"));
        }
        Destroy(GameObject.Find("Inventory(Clone)"));
    }
}
