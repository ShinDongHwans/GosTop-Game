using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Unit
{
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update() {
    }

    void LateUpdate() {
        if(sprites != null)
            spriteRenderer.sprite = sprites[(int)(Time.time * 2) % 2];
    }
}
