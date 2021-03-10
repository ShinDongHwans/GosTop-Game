using UnityEngine;
using System.Collections.Generic;

public class JetPumpEvent : Event
{
    public GameObject gameObject;
    public GameObject[] sprites;
    Vector2Int position;
    List<int> firstwave = new List<int> { 6, 7, 8, 11, 13, 16, 17, 18 };
    List<int> secondwave = new List<int> { 0, 1, 2, 3, 4, 5, 9, 10, 14, 15, 19, 20, 21, 22, 23, 24};
    int atk;
    Status invoker;
    // Start is called before the first frame update

    public JetPumpEvent(Vector2Int position, int atk, Status invoker)
    {
        this.position = position;
        this.atk = atk;
        this.invoker = invoker;
        duration = 0.9f;
    }

    public override void OnStart()
    {
        gameObject = new GameObject();
        gameObject.transform.position = new Vector3(position.x, position.y, -1f);
        sprites = new GameObject[25];
        for (int i = 0; i < 25; i++)
        {
            var dx = i % 5 - 2;
            var dy = 2 - i / 5;
            var spr = 0;
            if (i == 12)
                spr = 147;
            else if(firstwave.Contains(i))
                spr = 121;
            else if (secondwave.Contains(i))
                spr = 122;
            sprites[i] = new GameObject();
            var renderer = sprites[i].AddComponent<SpriteRenderer>();
            renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, spr);
            sprites[i].transform.position = new Vector3(dx, dy, 0);
            sprites[i].transform.SetParent(gameObject.transform, false);
            if(i!=12) renderer.enabled = false;
            else
            {
                var unit = GameManager.instance.GetUnitAt(position);
                if (unit != null && unit != invoker)
                {
                    unit.Damaged(atk);
                    Log.Make("HydroPump hits " + unit.GetName() + "!!");
                }
            }
        }
    }

    public override void Step(float progress)
    {
        if(0.3f < progress && progress <= 0.6f)
        {
            foreach (int i in firstwave)
            {
                var renderer = sprites[i].GetComponent<SpriteRenderer>();
                renderer.enabled = true;
                renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", progress > 0.2f && progress < 0.8f ? 0 : 1, 121);
                renderer.color = new Color(1, 1, 1, Mathf.Min(1, 8 * Mathf.Min(progress, 1f - progress)));
            }
        }
        else if(0.6f < progress && progress < 0.9f)
        {
            foreach (int i in firstwave)
            {
                var renderer = sprites[i].GetComponent<SpriteRenderer>();
                renderer.enabled = false;
            }
            foreach (int i in secondwave)
            {
                var renderer = sprites[i].GetComponent<SpriteRenderer>();
                renderer.enabled = true;
                renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", progress > 0.2f && progress < 0.8f ? 0 : 1, 122);
                renderer.color = new Color(1, 1, 1, Mathf.Min(1, 8 * Mathf.Min(progress, 1f - progress)));
            }
        }

    }

    public override void OnFinish()
    {
        DealEnemy(12, atk);
        foreach (int i in firstwave)
            DealEnemy(i, atk / 2);
        foreach (int i in secondwave)
            DealEnemy(i, atk);
        GameObject.Destroy(gameObject);
    }

    public void DealEnemy(int i, int attack)
    {
        var dx = i % 5 - 2;
        var dy = 2 - i / 5;
        Vector2Int enemyposition = position + new Vector2Int(dx, dy);
        var unit = GameManager.instance.GetUnitAt(enemyposition);
        if (unit != null && unit != invoker)
        {
            unit.Damaged(attack);
            Log.Make("HydroPump hits " + unit.GetName() + "!!");
        }
    }
}
