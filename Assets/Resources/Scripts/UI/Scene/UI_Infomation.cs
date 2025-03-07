using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Infomation : UI_Base
{
    enum Sliders
    {
        Slider_EXP,
        Slider_HP,
        Slider_MP
    }

    enum Texts
    {
        Text_EXP,
        Text_HP,
        Text_MP,
        Text_LV
    }

    private void Update()
    {
        SetText();
        SetSliderRatio();
    }

    public override void Init()
    {
        BindSlider(typeof(Sliders));
        BindText(typeof(Texts));
    }

    public void SetSliderRatio()
    {
        float expRatio = GameManager.Inst.m_player.m_stat.Exp / (float)GameManager.Inst.m_player.m_stat.TotalExp;
        float hpRatio = GameManager.Inst.m_player.m_stat.Hp / (float)GameManager.Inst.m_player.m_stat.MaxHp;
        float mpRatio = GameManager.Inst.m_player.m_stat.Mp / (float)GameManager.Inst.m_player.m_stat.MaxMp;

        GetSlider((int)Sliders.Slider_EXP).value = expRatio;
        GetSlider((int)Sliders.Slider_HP).value = hpRatio;
        GetSlider((int)Sliders.Slider_MP).value = mpRatio;
    }

    public void SetText()
    {
        GetText((int)Texts.Text_EXP).text = $"{GameManager.Inst.m_player.m_stat.Exp} / {GameManager.Inst.m_player.m_stat.TotalExp}";
        GetText((int)Texts.Text_HP).text = $"{GameManager.Inst.m_player.m_stat.Hp} / {GameManager.Inst.m_player.m_stat.MaxHp}";
        GetText((int)Texts.Text_MP).text = $"{GameManager.Inst.m_player.m_stat.Mp} / {GameManager.Inst.m_player.m_stat.MaxMp}";
        GetText((int)Texts.Text_LV).text = $"Lv.{GameManager.Inst.m_player.m_stat.Level}";
    }
}