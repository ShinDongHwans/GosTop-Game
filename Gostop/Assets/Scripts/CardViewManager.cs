using UnityEngine;
using UnityEngine.UI;

public class CardViewManager : MonoBehaviour
{
    public GameObject cardObject;
    public Card card;
    public void ShowCardView(Card card)
    {
        this.card = card;
        cardObject.SetActive(true);
        if(card == null)
        {
            cardObject.GetComponent<Image>().sprite = Card.defaultimage;
        }
        else
        {
            cardObject.GetComponent<Image>().sprite = card.sprite;
        }
        return;
    }
    
    public Card RemoveCard()
    {
        cardObject.SetActive(false);
        return card;
    }
}
