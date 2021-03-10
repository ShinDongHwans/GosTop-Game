using UnityEngine;

public class ScrollInfo : ItemInfo {

    public ScrollInfo(string name, string description, Sprite sprite, float unique, int magic) {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
        this.uniqueness = unique;
        this.magic = magic;
    }
}