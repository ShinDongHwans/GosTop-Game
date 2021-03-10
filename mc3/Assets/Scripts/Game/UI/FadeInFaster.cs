using UnityEngine;

public class FadeInFaster : MonoBehaviour
{
    public UnityEngine.UI.Image fade;
    float fades = 1.0f;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (fades > 0.01f && time >= 1f)
        {
            fades -= 0.05f;
            fade.color = new Color(0, 0, 0, fades);
            FadeOutCharacter("Warrior");
            FadeOutCharacter("Mage");
            FadeOutCharacter("Archor");
            FadeOutCharacter("Thief");
            FadeOutCharacter("Smith");
        }
        else if (fades <= 0.0f)
        {
            time = 0;
        }
    }
    
    private void FadeOutCharacter(string str)
    {
        GameObject character = GameObject.Find(str);
        if (character != null)
        {
            SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            color.a = 1 - fades;
            spriteRenderer.color = color;
        }
    }
}
