using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    static int defaultScoreTextSize = 25;
    static int scoreBoardMoveDirect = 1;
    public GameObject[] scoresBoard = new GameObject[GameManager.maxNumberofPlayers];
    
    public int index;
    int maxIndex
    {
        get { return (GameManager.numberofPlayers / 2);  }
    }
    public int[] rotationNumbers;


    //---------------------Functions----------------------------
    private void MakeRotationNumbers()
    {
        rotationNumbers = new int[GameManager.numberofPlayers - 1];
        for (int i = 1, j = 0; i <= GameManager.numberofPlayers; i++)
        {
            if (i != GameManager.instance.shownPlayer)
                rotationNumbers[j++] = i;
        }
        return;
    }

    private void ShownthPlayerScoreToithBox(int n, int i)
    {
        GameManager gameManager = GameManager.instance;
        if (n == -1)
        {
            scoresBoard[i].GetComponent<Text>().text = "";
            return;
        }
        scoresBoard[i].GetComponent<Text>().text = "Player" + n.ToString() + ": " + (gameManager.GetnthPlayerScore(n)).ToString();
        scoresBoard[i].GetComponent<Text>().fontSize = defaultScoreTextSize;
        return;
    }

    //----------------------public Function----------------------
    public void RotateShownScoreBoard()
    {
        ShownthPlayerScoreToithBox(rotationNumbers[index * 2], 1);
        if (rotationNumbers.Length > index * 2 + 1)
        {
            ShownthPlayerScoreToithBox(rotationNumbers[index * 2 + 1], 2);
        }
        else
        {
            ShownthPlayerScoreToithBox(-1, 2);
        }
        index+=scoreBoardMoveDirect;
        if (index >= maxIndex) index = index%maxIndex;
        else if (index < 0) index = maxIndex - 1;
    }

    public void ChangeTurn()
    {
        MakeRotationNumbers();
        index = 0;
        ShownthPlayerScoreToithBox(GameManager.instance.whoseTurn, 0);
        RotateShownScoreBoard();
    }

    public void Initializing()
    {
        int validNum = GameManager.numberofPlayers > GameManager.maxNumberofPlayers ? GameManager.maxNumberofPlayers : GameManager.numberofPlayers;
        for (int i = 0; i < validNum; i++)
        {
            scoresBoard[i].SetActive(true);
        }
    }

    [SerializeField]

    private void Awake()
    {
        
    }
}
