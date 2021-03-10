using UnityEngine;


public class Player : Unit {
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
