using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHandShown : MonoBehaviour
{
    public static int maxNumberofHandCardSlots = 14;
    
    int maxNumberofHandCards
    {
        get { return GameManager.numberofInitHandCard[GameManager.numberofPlayers]; }
    }

    public GameObject[] handCards = new GameObject[maxNumberofHandCardSlots];
    List<Card> mainPlayerHandCards;


    //---------------------------Functions---------------------------------

    public void AlarmChangeHand()
    {
        int i = 0;
        mainPlayerHandCards = GameManager.instance.GetnthPlayersHand(GameManager.instance.shownPlayer);
        for (; i < mainPlayerHandCards.Count; i++)
        {
            handCards[i].GetComponent<HandCard>().SetHandCard(mainPlayerHandCards[i]);
        }
        for (; i < maxNumberofHandCards; i++)
        {
            handCards[i].GetComponent<HandCard>().SetHandCard(null);
        }
        return;
    }
    
    public void Initializing()
    {
        for (int i = 0; i < maxNumberofHandCards; i++)
        {
            handCards[i].GetComponent<HandCard>().SetIndex(i);
            handCards[i].SetActive(true);
        }
    }

    //-----------------------------Unity Cycle---------------------------
    void Awake()
    {
        
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
