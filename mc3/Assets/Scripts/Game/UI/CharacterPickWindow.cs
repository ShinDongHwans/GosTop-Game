using UnityEngine;
using UnityEngine.UI;

public class CharacterPickWindow : MonoBehaviour
{
    public void OnWarriorClick()
    {
        if (Check()) return;
        var canvas = GameObject.Find("Canvas");
        GameObject jobinfoPage = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/JobInfoPage"));
        GameObject jobName = GameObject.Find("JobName");
        GameObject jobinfoScript = GameObject.Find("JobScript");
        
        Text name = jobName.GetComponent<Text>();
        Text script = jobinfoScript.GetComponent<Text>();

        name.text = "Warrior";
        script.text = "* 1 Sword\n\n* High Pow Status\n\n* High Hel Status";
        GameObject.Find("ItemInfoButton1").GetComponent<GameStartOrDelete>().jobnumber = 0;
        show(jobinfoPage, canvas);
    }

    public void OnMageick()
    {
        if (Check()) return;
        var canvas = GameObject.Find("Canvas");
        GameObject jobinfoPage = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/JobInfoPage"));
        GameObject jobName = GameObject.Find("JobName");
        GameObject jobinfoScript = GameObject.Find("JobScript");

        Text name = jobName.GetComponent<Text>();
        Text script = jobinfoScript.GetComponent<Text>();

        GameObject.Find("ItemInfoButton1").GetComponent<GameStartOrDelete>().jobnumber = 1;
        name.text = "Mage";
        script.text = "* 1 Random Staff\n\n* 1 Random Wand\n\n* High Int Status\n\n* Low Durability";
        show(jobinfoPage, canvas);
    }

    public void OnArchorClick()
    {
        if (Check()) return;
        var canvas = GameObject.Find("Canvas");
        GameObject jobinfoPage = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/JobInfoPage"));
        GameObject jobName = GameObject.Find("JobName");
        GameObject jobinfoScript = GameObject.Find("JobScript");

        Text name = jobName.GetComponent<Text>();
        Text script = jobinfoScript.GetComponent<Text>();

        GameObject.Find("ItemInfoButton1").GetComponent<GameStartOrDelete>().jobnumber = 3;
        name.text = "Archor";
        script.text = "* 1 Bow\n\n* High Dex Status";
        show(jobinfoPage, canvas);
    }

    public void OnThiefClick()
    {
        if (Check()) return;
        var canvas = GameObject.Find("Canvas");
        GameObject jobinfoPage = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/JobInfoPage"));
        GameObject jobName = GameObject.Find("JobName");
        GameObject jobinfoScript = GameObject.Find("JobScript");

        Text name = jobName.GetComponent<Text>();
        Text script = jobinfoScript.GetComponent<Text>();

        GameObject.Find("ItemInfoButton1").GetComponent<GameStartOrDelete>().jobnumber = 2;
        name.text = "Thief";
        script.text = "* 1 Ninja Star\n\n* High Dex Status";
        show(jobinfoPage, canvas);
    }

    public void OnSmithClick()
    {
        if (Check()) return;
        var canvas = GameObject.Find("Canvas");
        GameObject jobinfoPage = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/JobInfoPage"));
        GameObject jobName = GameObject.Find("JobName");
        GameObject jobinfoScript = GameObject.Find("JobScript");

        Text name = jobName.GetComponent<Text>();
        Text script = jobinfoScript.GetComponent<Text>();

        GameObject.Find("ItemInfoButton1").GetComponent<GameStartOrDelete>().jobnumber = 4;
        name.text = "Smith";
        script.text = "* 3 Upgrade Scrolls\n\n* 3 Random Recipes\n\n* Inventory is big\n\n* High Str Status";
        show(jobinfoPage, canvas);
    }

    public bool Check()
    {
        if (GameObject.Find("JobInfoPage(Clone)") != null) return true;
        else return false;
    }

    private void show(GameObject showed, GameObject canvas)
    {
        HideCharacter("Warrior");
        HideCharacter("Mage");
        HideCharacter("Archor");
        HideCharacter("Thief");
        HideCharacter("Smith");
        showed.transform.SetParent(canvas.transform, false);
        showed.transform.position = canvas.transform.position;
    }

    public void Delete()
    {
        Destroy(GameObject.Find("JobInfoPage(Clone)"));
        Show();
    }

    private void HideCharacter(string str)
    {
        GameObject character = GameObject.Find(str);
        if (character != null)
        {
            SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }

    private void Show()
    {
        FadeOutCharacter("Warrior");
        FadeOutCharacter("Mage");
        FadeOutCharacter("Archor");
        FadeOutCharacter("Thief");
        FadeOutCharacter("Smith");
    }

    private void FadeOutCharacter(string str)
    {
        GameObject character = GameObject.Find(str);
        if (character != null)
        {
            SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;
        }
    }
}
