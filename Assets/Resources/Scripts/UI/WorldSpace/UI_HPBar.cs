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
        // ��ü�� Ű�� ���� �ٸ� �� �����Ƿ� Collider�� ���̸� �������� ó��
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y + 0.1f);
        // ��ü�� ȸ���ϸ� HPBar ���� ���� ȸ���ϹǷ� Camera�� rotation�� ��ġ���Ѽ� �׻� ȭ���� ������ ������ ó��(������ ����)
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
