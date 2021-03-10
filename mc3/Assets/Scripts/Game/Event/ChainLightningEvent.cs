using UnityEngine;
using System.Collections.Generic;

public class ChainLightningEvent : Event
{
    public GameObject gameObject;
    public GameObject[] sprites;
    Vector2Int position;
    int atk;
    Status invoker;
    // Start is called before the first frame update
    int[] findorder = {12, 7, 11, 17, 13, 6, 8, 16, 18, 2, 10, 22, 14, 5, 1, 3, 9, 19, 23, 21, 15, 0, 4, 24, 20};
    List<int> npcindex = new List<int>();
    List<int> lightningsprite = new List<int>();
    Vector2Int pastpositions;
    int index = 0;

    public ChainLightningEvent(Vector2Int position, int atk, Status invoker)
    {
        this.position = position;
        this.pastpositions = position;
        this.atk = atk;
        this.invoker = invoker;
        duration = 1f;
    }

    public override void OnStart()
    {
        gameObject = new GameObject();
        gameObject.transform.position = new Vector3(position.x, position.y, -1f);
        sprites = new GameObject[25];
        foreach (int i in findorder)
        {
            var dx = i % 5 - 2;
            var dy = 2 - i / 5;
            sprites[i] = new GameObject();
            var renderer = sprites[i].AddComponent<SpriteRenderer>();
            sprites[i].transform.position = new Vector3(dx, dy, 0);
            sprites[i].transform.SetParent(gameObject.transform, false);

            Vector2Int enemyposition = position + new Vector2Int(dx, dy);
            
            var unit = GameManager.instance.GetUnitAt(enemyposition);
            if (unit != null)
            {
                npcindex.Add(i);
                int sx = enemyposition.x - pastpositions.x;
                int sy = enemyposition.y - pastpositions.y;
                if(sx >= 1)
                {
                    if(sy >= 1)
                    {
                        renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 82);
                        lightningsprite.Add(82);
                    }
                    else if(sy <= -1)
                    {
                        renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 81);
                        lightningsprite.Add(81);
                    }
                    else
                    {
                        renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 80);
                        lightningsprite.Add(80);
                    }
                }
                else if(sx == 0)
                {
                    renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 79);
                    lightningsprite.Add(79);
                }
                else
                {
                    if (sy >= 1)
                    {
                        renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 81);
                        lightningsprite.Add(81);
                    }
                    else if (sy <= -1)
                    {
                        renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 82);
                        lightningsprite.Add(82);
                    }
                    else
                    {
                        renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 80);
                        lightningsprite.Add(80);
                    }
                }
                pastpositions = enemyposition;
                if (i == 12)
                    renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, 126);
                else renderer.enabled = false;
                index++;
            }
        }
    }

    public override void Step(float progress)
    {
        var spr = 126;
        SpriteRenderer renderer1 = sprites[12].GetComponent<SpriteRenderer>();
        renderer1.enabled = true;
        renderer1.sprite = Util.LoadSprite("DawnLike/Objects/Effect", progress > 0.2f && progress < 0.8f ? 0 : 1, spr);
        renderer1.color = new Color(1, 1, 1, Mathf.Min(1, 8 * Mathf.Min(progress, 1f - progress)));

        if (progress < 0.2f)
        {
            for(int i = 1; i< 5; i++)
            {
                if (npcindex.Contains(findorder[i]))
                {
                    var renderer = sprites[findorder[i]].GetComponent<SpriteRenderer>();
                    renderer.enabled = true;
                }
            }
        }
        else if(progress < 0.4f)
        {
            for (int i = 5; i < 9; i++)
            {
                if (npcindex.Contains(findorder[i]))
                {
                    var renderer = sprites[findorder[i]].GetComponent<SpriteRenderer>();
                    renderer.enabled = true;
                }
            }
        }
        else if (progress < 0.6f)
        {
            for (int i = 9; i < 13; i++)
            {
                if (npcindex.Contains(findorder[i]))
                {
                    var renderer = sprites[findorder[i]].GetComponent<SpriteRenderer>();
                    renderer.enabled = true;
                }
            }
        }
        else if (progress < 0.8f)
        {
            for (int i = 13; i < 21; i++)
            {
                if (npcindex.Contains(findorder[i]))
                {
                    var renderer = sprites[findorder[i]].GetComponent<SpriteRenderer>();
                    renderer.enabled = true;
                }
            }
        }
        else if (progress < 1f)
        {
            for (int i = 21; i < 25; i++)
            {
                if (npcindex.Contains(findorder[i]))
                {
                    var renderer = sprites[findorder[i]].GetComponent<SpriteRenderer>();
                    renderer.enabled = true;
                }
            }
        }

    }

    public override void OnFinish()
    {
        foreach (int i in findorder)
        {
            var dx = i % 5 - 2;
            var dy = 2 - i / 5;
            Vector2Int enemyposition = position + new Vector2Int(dx, dy);
            var unit = GameManager.instance.GetUnitAt(enemyposition);
            if (unit != null && unit != invoker)
            {
                unit.Damaged(atk);
                Log.Make("Chain hits " + unit.GetName() + "!!");
            }
        }
        GameObject.Destroy(gameObject);
    }
}
