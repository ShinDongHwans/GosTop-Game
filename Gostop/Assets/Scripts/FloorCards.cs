using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCards : MonoBehaviour
{
    static float flySpeed = 3;

    public GameObject[] floorCards = new GameObject[3];
    public GameObject flyingCard;


    public int numberOfCardsOnTHisFloor;
    int season;
    Vector3 initPositionOfFlyingCard;
    public Vector3 initPositionOfThisFloor;
    Vector3 distanceOfFlyingAndFloorCards;
    Vector3 distanceOfFloorCardsAndOwnCards;
    //----------------------Functions----------------------

    //-------------------basic functions-------------------
    public void SetSeason(int i)
    {
        season = i;
        return;
    }
    public void Initializing()
    {
        initPositionOfFlyingCard = flyingCard.transform.position;
        initPositionOfThisFloor = floorCards[1].transform.position;
        distanceOfFlyingAndFloorCards = initPositionOfThisFloor - initPositionOfFlyingCard;
        distanceOfFloorCardsAndOwnCards = initPositionOfThisFloor - GameManager.instance.floor.GetComponent<FloorManager>().topCard.transform.position;

        numberOfCardsOnTHisFloor = 0;
    }

    //----------------card adding functions----------------
    public void ResetFlyingCard()
    {
        flyingCard.transform.position = initPositionOfFlyingCard;
        flyingCard.SetActive(false);
        return;
    }

    void SetCardToTopSlot(Card card)
    {
        floorCards[numberOfCardsOnTHisFloor].GetComponent<CardViewManager>().ShowCardView(card);
        numberOfCardsOnTHisFloor++;
        return;
    }

    Card RemoveCardFromTopSlot()
    {
        if(numberOfCardsOnTHisFloor == 0)
        {
            Debug.Log("Errored: Can not remove!, there is no card on " + season.ToString() + "th floor");
            return null;
        }
        numberOfCardsOnTHisFloor--;
        floorCards[numberOfCardsOnTHisFloor].SetActive(false);
        return floorCards[numberOfCardsOnTHisFloor].GetComponent<CardViewManager>().RemoveCard();                                                                                                                
    }

    void GetNTopCards(int n)
    {
        while (n-- != 0)
        {
            GameManager.instance.ObtainCardbynthPlayer(RemoveCardFromTopSlot(), GameManager.instance.whoseTurn);
        }
        return;
    }

    public FloorManager.PlayResult AddCardToThisFloorFromMainDeck(Card inputCard)
    {
        if (inputCard.cardSeason == season){
            if(GameManager.instance.whoseTurn == 0)
            {
                SetCardToTopSlot(inputCard);
                return FloorManager.PlayResult.기본;
            }
            else
            {
                if (numberOfCardsOnTHisFloor == 0)
                {
                    SetCardToTopSlot(inputCard);
                    return FloorManager.PlayResult.기본;
                }
                else if(numberOfCardsOnTHisFloor == 3)
                {
                    GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                    GetNTopCards(3);
                    return FloorManager.PlayResult.따닥;
                }
                else
                {
                    GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                    GetNTopCards(1);
                    return FloorManager.PlayResult.기본;
                }
            }
        }
        else{
            Debug.Log("Erroed: Season not matched by drawn card");
            return FloorManager.PlayResult.Error;
        }
    }

    public void AddCardToThisFloorFromHand(Card inputCard)
    {
        if (inputCard.cardSeason == season)
        {
            StartCoroutine(FlyingCard(inputCard, distanceOfFlyingAndFloorCards));
        }
        else
        {
            Debug.Log("Erroed: Season not matched");
        }
    }

    public FloorManager.PlayResult AddCardToThisFloorWithDrawing(Card inputCard)
    {
        Card cardFromMainDeck = GameManager.instance.PopMainDeck(GameManager.top);
        int cardSeason = inputCard.cardSeason;
        switch (numberOfCardsOnTHisFloor)
        {
            case (0):
                if(cardFromMainDeck == null)
                {
                    SetCardToTopSlot(inputCard);
                    return FloorManager.PlayResult.기본;
                }
                else
                {
                    if(cardSeason == cardFromMainDeck.cardSeason)
                    {
                        GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                        GameManager.instance.ObtainCardbynthPlayer(cardFromMainDeck, GameManager.instance.whoseTurn);
                        return FloorManager.PlayResult.쪽;
                    }
                    else
                    {
                        SetCardToTopSlot(inputCard);
                        GameManager.instance.floor.GetComponent<FloorManager>().AddCardOnFloorFromMainDeck(cardFromMainDeck);
                        return FloorManager.PlayResult.기본;
                    }
                }
            case (1):
                if(cardFromMainDeck == null)
                {
                    GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                    GetNTopCards(1);
                    return FloorManager.PlayResult.기본;
                }
                else
                {
                    if (cardFromMainDeck.cardSeason == season)
                    {
                        SetCardToTopSlot(inputCard);
                        SetCardToTopSlot(cardFromMainDeck);
                        return FloorManager.PlayResult.뻑;
                    }
                    else
                    {
                        GameManager.instance.floor.GetComponent<FloorManager>().AddCardOnFloorFromMainDeck(cardFromMainDeck);
                        GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                        GetNTopCards(1);
                        return FloorManager.PlayResult.기본;
                    }
                }
            case (2):
                if(cardFromMainDeck == null)
                {
                    GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                    GameManager.instance.ObtainCardbynthPlayer(cardFromMainDeck, GameManager.instance.whoseTurn);
                    return FloorManager.PlayResult.기본;
                }
                else
                {
                    if(cardFromMainDeck.cardSeason == season)
                    {
                        GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                        GameManager.instance.ObtainCardbynthPlayer(cardFromMainDeck, GameManager.instance.whoseTurn);
                        GetNTopCards(2);
                        return FloorManager.PlayResult.따닥;
                    }
                    else
                    {
                        GameManager.instance.floor.GetComponent<FloorManager>().AddCardOnFloorFromMainDeck(cardFromMainDeck);
                        GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                        GetNTopCards(1);
                        return FloorManager.PlayResult.기본;
                    }
                }
            case (3):
                if(cardFromMainDeck != null)
                {
                    GameManager.instance.floor.GetComponent<FloorManager>().AddCardOnFloorFromMainDeck(cardFromMainDeck);
                }
                GameManager.instance.ObtainCardbynthPlayer(inputCard, GameManager.instance.whoseTurn);
                GetNTopCards(3);
                return FloorManager.PlayResult.따닥;
            default:
                Debug.Log("Errored: there are too many cards on " + season.ToString() + " floor");
                return FloorManager.PlayResult.Error;
        }
    }

    IEnumerator FlyingCard(Card card, Vector3 destination)
    {
        flyingCard.GetComponent<CardViewManager>().ShowCardView(card);
        GameManager.instance.gState = GameManager.GameState.Animation;
        while (flyingCard.transform.position.y < (initPositionOfFlyingCard+destination).y)
        {
            flyingCard.transform.position += destination * GameManager.animInterval * flySpeed;
            yield return new WaitForSeconds(GameManager.animInterval);
        }
        if (GameManager.instance.floor.GetComponent<FloorManager>().DrawMainCard(card, season) == false)
        {
            ResetFlyingCard();
            GameManager.instance.floor.GetComponent<FloorManager>().SetPlayResult(AddCardToThisFloorWithDrawing(card));
            GameManager.instance.NextTurn();
            GameManager.instance.gState = GameManager.GameState.MyTurn;
        }
    }
    //---------------------Unity Cycle---------------------
    private void Awake()
    {
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
