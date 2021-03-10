using UnityEngine;
using UnityEngine.UI;

public class InvenMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        GameObject currentinventory = GameObject.Find("Inventory(Clone)");
        if ( currentinventory == null)
        {
            GameObject inventory = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/Inventory"));
            inventory.GetComponent<Inventory>().SetRef(1);
            inventory.GetComponent<Inventory>().ShowInvenPage(1);
            inventory.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        else
        {
            currentinventory.GetComponent<Inventory>().OnClick();
        }
    }
    
}
