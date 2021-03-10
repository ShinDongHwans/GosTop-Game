using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnButtonScript : MonoBehaviour
{
    public void Wait()
    {
        GameManager.instance.PassTurnOfPlayer(1);
    }
}
