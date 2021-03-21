using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int maxNumberofPlayers = 3;
    public static int numberofPlayers = 2;
    public static int[] numberofInitHandCard = new int[9] { -1, -1, 10, 7, 5, 4, 4, 3, 3 };
    public static float animInterval = 1 / 100f;
    public static int top = 0;
    static int scoreBoardChageCycle = 3;
    static int turnMoveDirection = 1;
    public static int[] endScore = new int[9] { -1, -1, 7, 3, 1, 1, 1, 1, 1};
    public static int shuffleTIme = 10;

    public enum GameState
    {
        PreParing,
        MyTurn,
        EnemyView,
        Animation,
        Finish
    }

    //public static int numberofInitGroundCard = 8;
    public int numberofInitGroundCard
    {
        get { return Card.defaultDeck.Length - 2 * numberofInitHandCard[numberofPlayers] * numberofPlayers; }
    }
    float timer;

    public GameObject hand;
    public GameObject scoreBoard;
    public GameObject floor;
    public GameObject ownCards;
    public GameObject showindow;
    public GameObject[] ChangeViewButtons = new GameObject[2];

    public Player[] playersList;
    public List<Card> mainDeck;
    public Card topCard
    {
        get { return mainDeck[top]; }
    }

    public int totalTurn;
    public int whoseTurn;
    public int shownPlayer;
    public GameState gState;
    bool needRotation;

    //---------------------Functions----------------------------
    public GameManager() : base()
    {
        instance = this;
    }

    public int GetnthPlayerScore(int n)
    {
        return playersList[n - 1].GetScore();
    }

    public bool GameFinishChecker()
    {
        if (mainDeck.Count == 0) return true;
        for(int i = 0; i < numberofPlayers; i++)
        {
            if (playersList[i].GetScore() >= endScore[numberofPlayers]) return true;
        }
        return false;
    }

    public void ChangeShownPlayer(int i)
    {
        shownPlayer = i;
        ownCards.GetComponent<OwnCardManager>().VisualChange();
        if (shownPlayer != whoseTurn) gState = GameState.EnemyView;
        else gState = GameState.MyTurn;
        hand.GetComponent<MyHandShown>().AlarmChangeHand();
        ChangeViewButtons[0].GetComponent<ChangeViewButton>().FindTargetSide();
        ChangeViewButtons[1].GetComponent<ChangeViewButton>().FindTargetSide();
        return;
    }

    public List<Card> GetnthPlayersHand(int n)
    {
        return playersList[n - 1].hand;
    }

    public void ObtainCardbynthPlayer(Card card, int n)
    {
        playersList[n - 1].GetCardtoUnder(card);
        return;
    }

    public Card PopMainDeck(int index)
    {
        if(mainDeck.Count == 0)
        {
            return null;
        }
        Card output = mainDeck[index];
        mainDeck.RemoveAt(index);
        return output;
    }

    public void NextTurn()
    {
        if (GameFinishChecker())
        {
            gState = GameState.Finish;
            Debug.Log("Game Finished");
        }
        totalTurn++;
        whoseTurn += turnMoveDirection;
        if(whoseTurn > numberofPlayers)
        {
            whoseTurn = 1;
        }
        else if (whoseTurn < 1)
        {
            whoseTurn = numberofPlayers;
        }
        ChangeShownPlayer(whoseTurn);
        scoreBoard.GetComponent<ScoreBoard>().ChangeTurn();
        return;
    }

    public void Shuffle()
    {
        int x;
        int i = 0;
        while (i++<mainDeck.Count)
        {
            x = Random.Range(0, mainDeck.Count);
            mainDeck.Add(PopMainDeck(x));
        }
    }

    void AllPlayerReset()
    {
        for (int i = 0; i < numberofPlayers; i++)
        {
            playersList[i].Initializing();
        }
        return;
    }

    void ResetMainDeck()
    {
        mainDeck = new List<Card>();
        for (int i = 0; i < Card.defaultDeck.Length; i++)
        {
            mainDeck.Add(Card.defaultDeck[i]);
        }
        for (int i = 0; i < shuffleTIme; i++) Shuffle();
    }

    public void NewGameStart()
    {
        //---------------------set to default----------------------
        totalTurn = 0;
        AllPlayerReset();
        ResetMainDeck();
        //---------------------Card Spreading-----------------------
        whoseTurn = 0;
        Card poppedCard = null;
        for (int i = 0; i < numberofPlayers; i++)
        {
            for(int j = 0; j < numberofInitHandCard[numberofPlayers]; j++)
            {
                poppedCard = PopMainDeck(top);
                if (poppedCard == null)
                {
                    Debug.Log("Errored: there are too few cards to spread them to all players properly.");
                    return;
                }
                playersList[i].GetCardtoHand(poppedCard);
            }
        }
        for(int i=0; i< numberofInitGroundCard; i++)
        {
            poppedCard = PopMainDeck(top);
            if (poppedCard == null)
            {
                Debug.Log("Errored: there are too few cards to spread them to ground properly.");
                return;
            }
            floor.GetComponent<FloorManager>().AddCardOnFloorFromMainDeck(poppedCard);
        }
    }

    public void Initializing()
    {
        floor.GetComponent<FloorManager>().Initializing();
        NewGameStart();
        scoreBoard.GetComponent<ScoreBoard>().Initializing();
        hand.GetComponent<MyHandShown>().Initializing();
        ownCards.GetComponent<OwnCardManager>().ReSetTotalManager();
        showindow.GetComponent<RemainCard>().ShowRemainNumberOfCaradAtMainDeck();
        NextTurn();
    }
    

    [SerializeField]
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Start()
    {
        gState = GameState.PreParing;
        playersList = new Player[numberofPlayers];
        ChangeViewButtons[0].GetComponent<ChangeViewButton>().SetDirection(ChangeViewButton.Direction.Left);
        ChangeViewButtons[1].GetComponent<ChangeViewButton>().SetDirection(ChangeViewButton.Direction.Right);
        needRotation = numberofPlayers > maxNumberofPlayers;
        for (int i = 0; i < numberofPlayers; i++)
        {
            playersList[i] = new Player();
        }
        Initializing();
    }

    private void Update()
    {
        if (needRotation && gState != GameState.PreParing)
        {
            timer += Time.deltaTime;
            if (timer > scoreBoardChageCycle)
            {
                timer = 0;
                scoreBoard.GetComponent<ScoreBoard>().RotateShownScoreBoard();
            }
        }
    }
}
