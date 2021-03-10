using UnityEngine;
using UnityEngine.UI;

public class UseMagicButton : MonoBehaviour {
    Item currentWeapon;
    int currentMagic;
    // Start is called before the first frame update
    void Start() {
        SelectCurrentMagic();
    }

    public void SelectCurrentMagic() {
        currentWeapon = GameObject.Find("ChangeWeaponButton").GetComponent<ChangeWeaponInfo>().currentweapon;
        if (currentWeapon != null) {
            currentMagic = ItemInfo.info[currentWeapon.kind].magic;
        } else {
            currentMagic = -1;
        }
        ShowMagicButton();
    }

    public void OnClick() {
        if (currentMagic != -1) {
            GameManager.instance.TryInvokeItem(currentWeapon);
        }
    }

    public void ShowMagicButton() {
        GameObject imageview = GameObject.Find("SelectedMagicImage");
        Image image = imageview.GetComponent<Image>();
        if(currentMagic < 0) {
            image.sprite = Resources.Load<Sprite>("Sprites/DawnLike/GUI/DefaultImage");
        } else if(currentMagic < Magic.infoPhysicalMaxIndex) {
            image.sprite = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Effect0")[141];
        } else {
            image.sprite = Resources.LoadAll<Sprite>("Sprites/DawnLike/Objects/Effect0")[147];
        }
    }
}
