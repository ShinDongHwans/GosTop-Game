using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour {
    public static Log instance;

    class LogText {
        public float createdTime;
        public string message;
        public GameObject gameObject;
        public Image image;
        public Text text;
        
        public LogText(string message) {
            this.createdTime = Time.time;
            this.message = message;
        }

        public void SetGameObject(GameObject o) {
            gameObject = o;
            image = o.GetComponent<Image>();
            text = o.transform.GetChild(0).GetComponent<Text>();
        }
    }

    List<LogText> list;
    public int oldestShowingIndex;

    public const float duration = 5f;
    public const float fadeOutDuration = 0.2f;

    void Awake() {
        list = new List<LogText>();
        oldestShowingIndex = 0;
    }

    void Start() {
        instance = this;
    }

    void Update() {
        for(int k = oldestShowingIndex; k < list.Count; k++) {
            var i = list[k];
            var o = i.gameObject;
            var dt = Time.time - i.createdTime;
            if(dt <= duration) return;
            else if(dt > duration + fadeOutDuration) {
                Object.Destroy(o);
                oldestShowingIndex++;
            } else {
                var p = 1f - (dt - duration) / fadeOutDuration;
                i.image.color = new Color(0, 0, 0, p * 0.6f);
                i.text.color = new Color(1, 1, 1, p);
            }
        }
    }

    void LateUpdate() {
        var t = new GameObject();
        t.transform.SetParent(gameObject.transform);
        Destroy(t);
    }

    void Add(LogText item) {
        list.Add(item);
        item.SetGameObject(Instantiate(Resources.Load<GameObject>("Prefabs/Game/UI/LogTextBox")));
        item.gameObject.transform.SetParent(gameObject.transform, false);
        item.text.text = item.message;
    }

    public static void Make(string message) {
        var text = new LogText(message);
        if(instance) {
            instance.Add(text);
        } else {
            Debug.Log("No Log Area");
        }
    }
}
