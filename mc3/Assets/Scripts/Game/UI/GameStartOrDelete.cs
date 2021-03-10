using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartOrDelete : MonoBehaviour
{
    public int jobnumber;

    public void OnStartClick()
    {
        PlayerStatus.startingJob = jobnumber;
        SceneManager.LoadScene("TestScene");
    }
}
