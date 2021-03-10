using UnityEngine;
using System.Collections.Generic;

public class CentralMoveEvent : Event
{
    public GameObject gameObject;
    public GameObject[] sprites;
    Vector2Int position;
    List<int> firstwave = new List<int> { 1, 3, 5, 7};
    List<int> secondwave = new List<int> { 0, 2, 6, 8};
    int atk;
    Status invoker;
    // Start is called before the first frame update

    public CentralMoveEvent( Vector2Int position, int atk, Status invoker)
    {
        this.position = position;
        this.atk = atk;
        this.invoker = invoker;
        duration = 1f;
    }

    public override void OnStart()
    {
        gameObject = new GameObject();
        gameObject.transform.position = new Vector3(position.x, position.y, -1f);
        sprites = new GameObject[9];
        for (int i = 0; i < 9; i++)
        {
            var dx = i % 3 - 1;
            var dy = 1 - i / 3;
            var spr = 0;
            if(i == 4)
                spr = 132;
            else
                spr = 131;
            sprites[i] = new GameObject();
            var renderer = sprites[i].AddComponent<SpriteRenderer>();
            renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, spr);
            sprites[i].transform.position = new Vector3(dx, dy, 0);
            sprites[i].transform.SetParent(gameObject.transform, false);
            if (i != 4) renderer.enabled = false;
        }
    }

    public override void Step(float progress)
    {
        if(0.2f < progress && progress < 0.4f)
        {
            TurnOn(firstwave);
        }
        else if(0.4f < progress && progress < 0.6f)
        {
            TurnOff(firstwave);
            TurnOn(secondwave);
        }
        else if (0.6f < progress && progress < 0.8f)
        {
            TurnOff(secondwave);
            TurnOn(firstwave);
        }
        else if (0.8f < progress && progress < 1f)
        {
            TurnOff(firstwave);
            TurnOn(secondwave);
        }
    }

    public override void OnFinish()
    {
        for (int i = 0 ; i< 9; i++)
        {
            var dx = i % 3 - 1;
            var dy = 1 - i / 3;
            Vector2Int enemyposition = position + new Vector2Int(dx, dy);
            var unit = GameManager.instance.GetUnitAt(enemyposition);
            if (unit != null && unit != invoker)
            {
                unit.Damaged(atk);
                Log.Make("Tornado hits " + unit.GetName() + "!!");
            }
        }
        GameObject.Destroy(gameObject);
    }

    public void TurnOn(List<int> tolist)
    {
        foreach (int i in tolist)
        {
            var renderer = sprites[i].GetComponent<SpriteRenderer>();
            renderer.enabled = true;
        }
    }

    public void TurnOff(List<int> tolist)
    {
        foreach (int i in tolist)
        {
            var renderer = sprites[i].GetComponent<SpriteRenderer>();
            renderer.enabled = false;
        }
    }
}
