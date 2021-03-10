using UnityEngine;

public class PotionInfo : ItemInfo {

    public PotionInfo(string name, string description, Sprite sprite, float unique, int magic) {
        this.name = name;
        this.description = description;
        this.sprite = sprite;
        this.uniqueness = unique;
        this.magic = magic;
    }
}