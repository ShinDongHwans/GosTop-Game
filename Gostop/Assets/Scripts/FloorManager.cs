using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static int numberOfSeasons = 12;
    static float flySpeed = 2f;
    public enum PlayResult
    {
        기본,
        쪽,
        뻑,
        싹쓸이,
        따닥,
        Error
    }

    public GameObject[] floors = new GameObject[numberOfSeasons];
    public GameObject topCard;
    Vector3 initPositionOftopCard;

    public PlayResult playResult;

    //----------------------------Functions-------------------------
    public void SetPlayResult(PlayResult pResult)
    {
        playResult = pResult;
        return;
    }

    public bool FloorNullCheck()
    {
        bool output = true;
        for(int i=0;i< numberOfSeasons; i++)
        {
            output &= (floors[i].GetComponent<FloorCards>().numberOfCardsOnTHisFloor == 0);
        }
        return output;
    }

    public void AddCardOnFloor(Card card)
    {
        floors[card.cardSeason - 1].GetComponent<FloorCards>().AddCardToThisFloorWithDrawing(card);
        return;
    }

    public void AddCardOnFloorFromHand(Card card)
    {
        floors[card.cardSeason - 1].GetComponent<FloorCards>().AddCardToThisFloorFromHand(card);
        return;
    }

    public void AddCardOnFloorFromMainDeck(Card card)
    {
        floors[card.cardSeason - 1].GetComponent<FloorCards>().AddCardToThisFloorFromMainDeck(card);
        return;
    }

    public bool DrawMainCard(Card inputCard, int season)
    {
        if (GameManager.instance.mainDeck.Count == 0)
            return false;
        Card topCard = GameManager.instance.topCard;
        StartCoroutine(FlyingMainCard(topCard, inputCard, 
            floors[topCard.cardSeason-1].GetComponent<FloorCards>().initPositionOfThisFloor-initPositionOftopCard, season));
        return true;
    }

    void ResetTopCardObject()
    {
        topCard.transform.position = initPositionOftopCard;
        topCard.SetActive(false);
        return;
    }

    IEnumerator FlyingMainCard(Card card, Card inputCard, Vector3 destination, int season)
    {
        topCard.GetComponent<CardViewManager>().ShowCardView(card);
        GameManager.instance.gState = GameManager.GameState.Animation;
        int i = 0;
        while (i++ < 1/(GameManager.animInterval*flySpeed))
        {
            topCard.transform.position += destination * GameManager.animInterval * flySpeed;
            yield return new WaitForSeconds(GameManager.animInterval);
        }
        ResetTopCardObject();
        floors[season - 1].GetComponent<FloorCards>().ResetFlyingCard();
        GameManager.instance.floor.GetComponent<FloorManager>().SetPlayResult(floors[season-1].GetComponent<FloorCards>().AddCardToThisFloorWithDrawing(inputCard));
        
        GameManager.instance.NextTurn();
        GameManager.instance.showindow.GetComponent<RemainCard>().ShowRemainNumberOfCaradAtMainDeck();
        GameManager.instance.gState = GameManager.GameState.MyTurn;
    }

    public void Initializing()
    {
        for (int i = 0; i < numberOfSeasons; i++)
        {
            floors[i].GetComponent<FloorCards>().SetSeason(i + 1);
            floors[i].GetComponent<FloorCards>().Initializing();
        }
        initPositionOftopCard = topCard.transform.position;
    }
    //-----------------------Unity Cycle------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
