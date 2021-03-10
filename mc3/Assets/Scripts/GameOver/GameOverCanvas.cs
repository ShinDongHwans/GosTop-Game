using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        var textResult = GameObject.Find("TextResult");
        textResult.GetComponent<Text>().text = Report.result;
        var textReport = GameObject.Find("TextReport");
        textReport.GetComponent<Text>().text = Report.ToString();
    }

    public void OnClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
