using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public GameObject textObject;
    public Image image;
    public Text text;

    private float toastedTime;
    private float anim = 0.1f;
    private float _duration = 2f;
    public float duration {
        get { return _duration; }
        set { 
            _duration = value;
            anim = 0.2f / _duration;
        }
    }

    void Start() {
        toastedTime = Time.time;
        image = gameObject.GetComponent<Image>();
        textObject = transform.GetChild(0).gameObject;
        text = textObject.GetComponent<Text>();
        alphaFunction(0);
    }

    // Update is called once per frame
    void Update() {
        var progress = (Time.time - toastedTime) / duration;
        if(progress >= 1f) {
            Destroy(gameObject);
        } else {
            alphaFunction(progress);
        }
    }

    void alphaFunction(float progress) {
        float alpha = 1f;
        if(progress < anim) alpha = progress / anim;
        else if(progress > 1f - anim) alpha = (1f - progress) / anim;
        if(alpha < 0) alpha = 0;
        else if(alpha > 1) alpha = 1;
        image.color = new Color(0, 0, 0, alpha * 0.5f);
        text.color = new Color(1, 1, 1, alpha);
    }
    public static void Make(string message, float duration = 2f) {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/Toast"));
        obj.GetComponent<Toast>().duration = duration;
        GameObject text = obj.transform.Find("Text").gameObject;
        obj.GetComponent<Toast>().textObject = text;
        text.GetComponent<Text>().text = message;
        obj.transform.SetParent(canvas.transform, false);
    }
}
