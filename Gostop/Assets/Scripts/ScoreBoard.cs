using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    static int defaultScoreTextSize = 25;
    static int scoreBoardMoveDirect = -1;
    int topBanner = 0;
    int topShownPlayer
    {
        get { return GameManager.instance.whoseTurn; }
    }
    public GameObject[] scoresBoard = new GameObject[GameManager.maxNumberofPlayers];
    
    public int index;
    int maxIndex
    {
        get
        {
            if (GameManager.maxNumberofPlayers != 1)  
                return ((GameManager.numberofPlayers - 1) / (GameManager.maxNumberofPlayers - 1));
            return GameManager.numberofPlayers - 1;
        }
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
        if (n <= 0)
        {
            scoresBoard[i].GetComponent<Text>().text = "";
            return;
        }
        scoresBoard[i].GetComponent<Text>().text = "Player" + n.ToString() + ": " + (GameManager.instance.GetnthPlayerScore(n)).ToString();
        scoresBoard[i].GetComponent<Text>().fontSize = defaultScoreTextSize;
        return;
    }

    //----------------------public Function----------------------
    public void RotateShownScoreBoard()
    {
        ShownthPlayerScoreToithBox(rotationNumbers[index * (GameManager.maxNumberofPlayers-1)], (1 + topBanner)%GameManager.maxNumberofPlayers);
        if (GameManager.maxNumberofPlayers == 3 && rotationNumbers.Length > index * 2 + 1)
        {
            ShownthPlayerScoreToithBox(rotationNumbers[index * (GameManager.maxNumberofPlayers - 1) + 1], (2 + topBanner) % GameManager.maxNumberofPlayers);
        }
        else if((2 + topBanner) % GameManager.maxNumberofPlayers != topBanner)
        {
            ShownthPlayerScoreToithBox(-1, (2 + topBanner) % GameManager.maxNumberofPlayers);
        }       
        index+=scoreBoardMoveDirect;
        if (index > maxIndex) index = 0;
        else if (index < 0) index = maxIndex;
        return;
    }

    public void ChangeTurn()
    {
        MakeRotationNumbers();
        index = 0;
        ShownthPlayerScoreToithBox(topShownPlayer, topBanner);
        RotateShownScoreBoard();
    }

    public void Initializing()
    {
        for (int i = 0; i < GameManager.maxNumberofPlayers; i++)
        {
            scoresBoard[i].SetActive(true);
        }
    }

    [SerializeField]

    private void Awake()
    {
        
    }
}
