using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEvent : Event {
    public GameObject gameObject;
    public GameObject[] sprites;
    int type;
    Vector2Int position;
    int atk;

    public static int[] spriteOffset = {0, 1, 2, 6, 7, 8, 12, 13, 14};

    public delegate void onFinish(MagicThrowEvent self);

    public ExplosionEvent(int type, Vector2Int position, int atk) {
        this.type = type;
        this.position = position;
        this.atk = atk;
        duration = 0.5f;
    }

    public override void OnStart() {
        gameObject = new GameObject();
        gameObject.transform.position = new Vector3(position.x, position.y, -1f);
        sprites = new GameObject[9];
        for(int i = 0; i < 9; i++) {
            var dx = i % 3 - 1;
            var dy = 1 - i / 3;
            var spr = type * 3 + spriteOffset[i];
            sprites[i] = new GameObject();
            var renderer = sprites[i].AddComponent<SpriteRenderer>();
            renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", 0, spr);
            sprites[i].transform.position = new Vector3(dx, dy, 0);
            sprites[i].transform.SetParent(gameObject.transform, false);
            
            var unit = GameManager.instance.GetUnitAt(position + new Vector2Int(dx, dy));
            if(unit != null) {
                unit.Damaged(atk);
                Log.Make("Explosion hits " + unit.GetName() + "!!");
            }
        }
    }
    public override void Step(float progress) {
        for(int i = 0; i < 9; i++) {
            var spr = type * 3 + spriteOffset[i];
            var renderer = sprites[i].GetComponent<SpriteRenderer>();
            renderer.sprite = Util.LoadSprite("DawnLike/Objects/Effect", progress > 0.2f && progress < 0.8f ? 0 : 1, spr);
            renderer.color = new Color(1, 1, 1, Mathf.Min(1, 8 * Mathf.Min(progress, 1f - progress)));
        }
    }
    public override void OnFinish() {
        GameObject.Destroy(gameObject);
    }
}
