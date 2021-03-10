using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TurnPageButton : MonoBehaviour
{
    private Image spriteRenderer;

    public void OnClick1()
    {
        spriteRenderer = GameObject.Find("FadeOut").GetComponent<Image>();
        StartCoroutine("RunFadeOut1");
    }

    public void OnClick2()
    {
        spriteRenderer = GameObject.Find("FadeOut").GetComponent<Image>();
        StartCoroutine("RunFadeOut2");
    }
    IEnumerator RunFadeOut1()
    {
        Color color = spriteRenderer.color;
        while (color.a < 1.0f)
        {
            color.a += 0.05f;
            spriteRenderer.color = color;
            FadeInCharacter("Warrior");
            FadeInCharacter("Mage");
            FadeInCharacter("Archor");
            FadeInCharacter("Thief");
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("PickCharacterWindow2");
    }
    IEnumerator RunFadeOut2()
    {
        Color color = spriteRenderer.color;
        print(color.a);
        while (color.a < 1.0f)
        {
            color.a += 0.05f;
            spriteRenderer.color = color;
            FadeInCharacter("Smith");
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene("PickCharacterWindow1");
    }

    private void FadeInCharacter(string str)
    {
        GameObject character = GameObject.Find(str);
        SpriteRenderer spriteRenderer = character.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = color.a - 0.05f;
        spriteRenderer.color = color;
    }
}
