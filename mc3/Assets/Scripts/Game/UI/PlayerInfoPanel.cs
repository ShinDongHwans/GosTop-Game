using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour {
    GameObject textHp, textLv, textExp, textPlayerLv, textHunger;
    GameObject[] hpBar;

    Text tTextHp, tTextLv, tTextExp, tTextPlayerLv, tTextHunger;
    Image[] iHpBar;

    void Start() {
        textHp = GameObject.Find("TextHp");
        textLv = GameObject.Find("TextLv");
        textExp = GameObject.Find("TextExp");
        textPlayerLv = GameObject.Find("TextPlayerLv");
        textHunger = GameObject.Find("TextHunger");

        tTextHp = textHp.GetComponent<Text>();
        tTextLv = textLv.GetComponent<Text>();
        tTextExp = textExp.GetComponent<Text>();
        tTextPlayerLv = textPlayerLv.GetComponent<Text>();
        tTextHunger = textHunger.GetComponent<Text>();

        hpBar = new GameObject[] {
            GameObject.Find("HPBar0"),
            GameObject.Find("HPBar1"),
            GameObject.Find("HPBar2"),
            GameObject.Find("HPBar3"),
        };
        iHpBar = new Image[] {
            hpBar[0].GetComponent<Image>(),
            hpBar[1].GetComponent<Image>(),
            hpBar[2].GetComponent<Image>(),
            hpBar[3].GetComponent<Image>(),
        };
    }
    void LateUpdate() {
        var player = GameManager.instance.player;

        tTextLv.text = "Floor " + (LevelManager.instance.currentLevelIndex + 1);
        tTextPlayerLv.text = "Lv." + player.level;
        tTextExp.text = "Exp " + player.currentExp;
        tTextHp.text = "HP " + player.hp;
        tTextHunger.text = "Hunger  " + player.hungryMessage;

        SetHpGaugeBar((float)player.hp / player.status.maxHP);
    }

    void SetHpGaugeBar(float ratio) {
        var t = (int)(64 * ratio);
        for(int i = 0; i < 4; i++) {
            int rat;
            if(t < 0) {
                iHpBar[i].enabled = false;
            } else {
                if(t >= 16) rat = 15;
                else rat = t % 16;
                iHpBar[i].enabled = true;
                iHpBar[i].sprite = Util.LoadSprite("DawnLike/GUI/gauge_6", 15 - rat);
            }
            t -= 16;
        }
    }
}
