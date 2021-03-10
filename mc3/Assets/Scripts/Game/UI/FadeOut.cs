using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image fadeout;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Fadeout()
    {
        fadeout = GameObject.Find("FadeOut").GetComponent<Image>();
        StartCoroutine("RunFadeOut");
    }

    IEnumerator RunFadeOut()
    {
        Color color = fadeout.color;
        while (color.a < 1.0f)
        {
            color.a += 0.01f;
            fadeout.color = color;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
