using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject handCard;

    Card card;
    int index;

    public void SetIndex(int i)
    {
        index = i;
        return;
    }

    public void ShowCard(bool visualable)
    {
        if (card == null)
        {
            handCard.GetComponent<Image>().sprite = Card.defaultimage;
        }
        else if (visualable == true)
        {
            handCard.GetComponent<Image>().sprite = card.sprite;
        } 
        else
        {
            handCard.GetComponent<Image>().sprite = Card.backimage;
        }
    }

    public void SetHandCard(Card card)
    {
        this.card = card;
        ShowCard(GameManager.instance.gState == GameManager.GameState.MyTurn);
        return;
    }

    public void OnClick()
    {
        if (card != null && GameManager.instance.gState == GameManager.GameState.MyTurn)
        {
            GameManager.instance.playersList[GameManager.instance.whoseTurn - 1].RemoveithHand(index);
            GameManager.instance.floor.GetComponent<FloorManager>().AddCardOnFloorFromHand(card);
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
