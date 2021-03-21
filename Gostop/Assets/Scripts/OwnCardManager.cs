using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnCardManager : MonoBehaviour
{
    static int numberOfCardFields = 4;
    public GameObject cardField;
    public GameObject[] cardFields = new GameObject[numberOfCardFields];
    public GameObject[] cards;
    bool totalManager = false;
    int totalNumber;
    int kind;
    //----------------------Functions--------------------
    
    void Initializing(int k)
    {
        kind = k;
        totalNumber = Card.totalNumberOfEachKinds[kind];
        cards = new GameObject[totalNumber];
        for(int i = 0; i < totalNumber; i++)
        {
            cards[i] = GameObject.Find("GroundCard" + kind + " (" + i + ")");
        }
    }

    public void ReSetTotalManager()
    {
        totalManager = true;
        for(int i = 0; i < numberOfCardFields; i++)
        {
            cardFields[i].GetComponent<OwnCardManager>().Initializing(i);
        }
    }
    
    public void ShowMyCardwithKind()
    {
        if (!totalManager)
        {
            int i = 0;
            List<Card> shownPlayerUnderwithKind = GameManager.instance.playersList[GameManager.instance.shownPlayer - 1].underCards[kind];
            int numberOfTotalCardOnUnderwithKind = shownPlayerUnderwithKind.Count;
            for (i = 0; i < numberOfTotalCardOnUnderwithKind; i++)
            {
                cards[i].GetComponent<CardViewManager>().ShowCardView(shownPlayerUnderwithKind[i]);
                cards[i].SetActive(true);
            }
            for (; i < totalNumber; i++)
            {
                cards[i].GetComponent<CardViewManager>().ShowCardView(null);
                cards[i].SetActive(false);
            }
            return;
        }
    }

    public void VisualChange()
    {
        if (totalManager)
        {
            for(int i = 0; i < numberOfCardFields; i++)
            {
                cardFields[i].GetComponent<OwnCardManager>().ShowMyCardwithKind();
            }
        }
    }

    //--------------------Unity Cycle--------------------
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
