using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Image spriteRenderer;

    public void OnClick()
    {
        spriteRenderer = GameObject.Find("FadeOut").GetComponent<Image>();
        StartCoroutine("RunFadeOut");
    }

    IEnumerator RunFadeOut()
    {
        Color color = spriteRenderer.color;
        while (color.a < 1.0f)
        {
            color.a += 0.01f;
            spriteRenderer.color = color;
            FadeInCharacter("Warrior");
            FadeInCharacter("Mage");
            FadeInCharacter("Archor");
            FadeInCharacter("Thief");
            FadeInCharacter("Smith");
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("PickCharacterWindow1");
    }

    private void FadeInCharacter(string str)
    {
        GameObject character = GameObject.Find(str);
        if (character != null)
        {
            SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            color.a = color.a - 0.01f;
            spriteRenderer.color = color;
        }
    }
}
