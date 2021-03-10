using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int recheck = 0;
    public int quitcheck = 0;
    // Start is called before the first frame update
    void Start() {
        var textPow = GameObject.Find("TextPow");
        var textWis = GameObject.Find("TextWis");
        var textDex = GameObject.Find("TextDex");
        var textHel = GameObject.Find("TextHel");

        textPow.GetComponent<Text>().text = "Pow " + GameManager.instance.player.stat.power;
        textWis.GetComponent<Text>().text = "Wis " + GameManager.instance.player.stat.wisdom;
        textDex.GetComponent<Text>().text = "Dex " + GameManager.instance.player.stat.dex;
        textHel.GetComponent<Text>().text = "Hel " + GameManager.instance.player.stat.health;
    }

    // Update is called once per frame
    public void OnClickMenu() {
        Destroy(GameObject.Find("Menu(Clone)"));
    }

    public void OnClickQuitButton() {
        if (quitcheck == 1)
            Application.Quit();
        else
        {
            Log.Make("Really? If you want to quit, press one more");
            quitcheck = 1;
        }
    }

    public void OnClickReStartButton()
    {
        if (recheck == 1)
            ReallyChange();
        else
        {
            Log.Make("Really? If you want to restart, press one more");
            recheck = 1;
        }
    }

    private void ReallyChange()
    {
        SceneManager.LoadScene("PickCharacterWindow1");
    }
}
