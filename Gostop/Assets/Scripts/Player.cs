using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public List<Card> hand;
    public List<Card> under;
    public List<Card>[] underCards;
    public string[] names = new string[] {"피: ", "쌍피: ", "광: ", "고도리: ", "열끝: ", "홍단: ", "청단: ", "단:"};
    public int point = 0;
    public int bgwang = 0;
    public int chodan = 0;

    public Player()
    {
        this.hand = new List<Card>();
        this.under = new List<Card>();
        this.point = 0;
        this.bgwang = 0;
    }

    public void RemoveithHand(int i)
    {
        hand.RemoveAt(i);
        return;
    }

    public void printDeck()
    {
        for(int i = 0; i < hand.Count; i++)
        {

        }
    }

    public void GetCardtoHand(Card card)
    {
        hand.Add(card);
    }

    public void GetCardtoUnder(Card card)
    {
        under.Add(card);
        int cType = card.cardType;
        //피
        if (cType <= 1)
        {
            underCards[0].Add(card);
        }
        //광
        else if (cType == 2)
        {
            underCards[1].Add(card);
            if (card.CardSeasonCheck(11))
                bgwang++;
        }
        //단
        else if (cType >= 5)
        {
            underCards[2].Add(card);
            if (card.CardTypeCheck(7) && card.CardSeasonCheck(11))
                chodan++;
        }
        //그림
        else
        {
            underCards[3].Add(card);
        }
    }

    public int CalculatePoint()
    {
        int output = 0;
        int[] numofCard = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        foreach(Card i in under)
        {
            numofCard[i.cardType]++;
        }

        //피 계산
        int numofpi = numofCard[0] + 2 * numofCard[1];
        output +=  numofpi > 9 ? numofpi - 9 : 0;

        //광 계산
        if (numofCard[2] == 3)
        {
            if (bgwang == 1)
                output += 2;
            else
                output += 3;
        }
        else if (numofCard[2] == 4)
            output += 4;
        else if (numofCard[2] == 5)
            output += 15;

        //고도리 계산
        if(numofCard[3] == 3)
            output += 5;

        // 열끝 계산
        output += numofCard[4] > 4 ? numofCard[4] - 4 : 0;

        // 홍단 계산
        output += numofCard[5] == 3 ? 3 : 0;


        // 청단 계산
        output += numofCard[6] == 3 ? 3 : 0;

        //초단 계산
        if (numofCard[7] >= 3)
        {
            if (chodan == 0 ||numofCard[7]==4)
                output += 3;
        }

        // 단 계산
        int numofflag = numofCard[5] + numofCard[6] + numofCard[7];
        output += numofflag  > 4? numofflag - 4 : 0;

        return output;
    }

    public int GetScore()
    {
        return CalculatePoint();
    }

    public void Initializing()
    {
        hand = new List<Card>();
        under = new List<Card>();
        underCards = new List<Card>[4] { new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>() };
        point = 0;
        bgwang = 0;
        chodan = 0;
    }
}
