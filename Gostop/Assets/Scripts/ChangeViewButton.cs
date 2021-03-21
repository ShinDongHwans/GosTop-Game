using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeViewButton : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right
    }
    public GameObject changeViewButton;

    Direction direction;
    int targetPlayer;

    //------------------------------Functions-------------------------------------
    public void SetDirection(Direction dir)
    {
        direction = dir;
        return;
    }

    void GetRightSide()
    {
        targetPlayer = GameManager.instance.shownPlayer + 1;
        if (targetPlayer > GameManager.numberofPlayers) targetPlayer = 1;
        return;
    }

    void GetLeftSide()
    {
        targetPlayer = GameManager.instance.shownPlayer - 1;
        if (targetPlayer <= 0) targetPlayer = GameManager.numberofPlayers;
        return;
    }

    public void FindTargetSide()
    {
        if (direction == Direction.Right)
        {
            GetRightSide();
            changeViewButton.GetComponent<Text>().text = "player" + targetPlayer.ToString() + " -->"; 
        }
        else if (direction == Direction.Left)
        {
            GetLeftSide();
            changeViewButton.GetComponent<Text>().text = "<-- player" + targetPlayer.ToString();
        }
        return;
    }

    public void OnClick()
    {
        GameManager.instance.ChangeShownPlayer(targetPlayer);
    }
    //---------------------------------unity Cycle----------------------------
    void Start()
    {

    }

    void Update()
    {
        
    }
}
