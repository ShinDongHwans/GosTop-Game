using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    void Start() { }
    void Update() { }

    public void OnClick() {
        var menu = GameObject.Find("Menu(Clone)");
        if(menu != null) {
            Destroy(menu);
        } else {
            var menuObject = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/Menu"));
            menuObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
    }
}
