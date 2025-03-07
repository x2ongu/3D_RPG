using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
        HPText,
        BossNameText
    }

    Stat _stat;

    void Update()
    {
        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHPRatio(ratio);
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        GetObject((int)GameObjects.BossNameText).GetComponent<TextMeshProUGUI>().text = "ÇØ°ñ ±¤Àü»ç";

        _stat = GetComponentInParent<Stat>();
    }

    public void SetHPRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
        GetObject((int)GameObjects.HPText).GetComponent<TextMeshProUGUI>().text = _stat.Hp + " / " + _stat.MaxHp;
    }
}
