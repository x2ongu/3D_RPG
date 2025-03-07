using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar,
    }

    Stat _stat;

    private void Update()
    {
        Transform parent = transform.parent;
        // 객체의 키가 각자 다를 수 있으므로 Collider의 높이를 기준으로 처리
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y + 0.1f);
        // 객체가 회전하면 HPBar 또한 같이 회전하므로 Camera의 rotation과 일치시켜서 항상 화면을 보도록 지정해 처리(빌보드 형식)
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHPRatio(ratio);
    }

    private void OnEnable()
    {
        BindObject(typeof(GameObjects));
    }

    public override void Init()
    {
        BindObject(typeof(GameObjects));

        _stat = transform.parent.GetComponent<Stat>();
    }

    public void SetHPRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
