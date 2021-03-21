using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainCard : MonoBehaviour
{
    public GameObject remainNumberWindow;
    // Start is called before the first frame update

    public void ShowRemainNumberOfCaradAtMainDeck()
    {
        remainNumberWindow.GetComponent<Text>().text = "남은 카드\n" + GameManager.instance.mainDeck.Count.ToString();
        return;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
