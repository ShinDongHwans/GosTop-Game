using UnityEngine;

public class DroppedItem {
    public GameObject gameObject;
    public Vector2Int position;
    public Item item;

    public DroppedItem(int x, int y, Item item) {
        this.position = new Vector2Int(x, y);
        this.item = item;
    }

    public DroppedItem(Vector2Int v, Item item) : this(v.x, v.y, item) {}
}