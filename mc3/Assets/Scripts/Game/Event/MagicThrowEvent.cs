using UnityEngine;

public class MagicThrowEvent : Event {
    public GameObject gameObject;
    string name;
    Sprite sprite;
    public Vector2Int startPosition;
    public Vector2Int endPosition;
    public Vector2Int originalEndPosition;
    Vector2Int delta;
    float distance;
    float progressR;
    public float speed;
    public int atk;
    public float spriteAngle;

    public delegate void onFinish(MagicThrowEvent self);

    public MagicThrowEvent(string name, Sprite sprite, Vector2Int startPosition, Vector2Int endPosition, float speed, int atk, float spriteAngle = 0f) {
        this.name = name;
        this.sprite = sprite;
        this.startPosition = startPosition;
        var unit = GameManager.instance.GetNearestUnitOnLine(endPosition, startPosition);
        originalEndPosition = endPosition;
        progressR = 1f;
        var oriDist = (endPosition - startPosition).magnitude;
        if(unit == null) {
            this.endPosition = endPosition;
        } else {
            this.endPosition = unit.position;
            if(oriDist != 0) progressR = (this.endPosition - startPosition).magnitude / oriDist;
        }
        delta = this.endPosition - startPosition;
        distance = delta.magnitude;
        duration = distance / speed;
        this.atk = atk;
        this.spriteAngle = spriteAngle;
    }

    public override void OnStart() {
        gameObject = new GameObject();
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        gameObject.transform.Rotate(0, 0, Mathf.Rad2Deg * (Mathf.Atan2(delta.y, delta.x)) - spriteAngle);
        gameObject.transform.position = new Vector3(startPosition.x, startPosition.y, -1f);
    }
    public override void Step(float progress) {
        var sp = new Vector3(startPosition.x, startPosition.y, -1f);
        var ep = new Vector3(originalEndPosition.x, originalEndPosition.y, -1f);
        progress *= progressR;
        gameObject.transform.position = sp * (1 - progress) + ep * progress;
    }
    public override void OnFinish() {
        GameObject.Destroy(gameObject);
        if(atk > 0) {
            Status o = GameManager.instance.GetUnitAt(endPosition);
            if(o != null) {
                o.Damaged(atk - o.stat.wisdom);
                Log.Make(name + " hits " + o.GetName() + "!");
            }
        }
        OnHit();
    }

    public virtual void OnHit() { }
}
