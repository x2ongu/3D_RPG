using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_QuickSlot_FinalSkill : UI_Drag_Skill
{
    enum Images
    {
        Skill_Icon,
        Skill_Cooldown_Image
    }

    enum Texts
    {
        Skill_Cooldown_Text,
        Skill_Key_Text
    }

    [HideInInspector]
    public KeyCode m_inputKey;

    private float m_time = 0f;
    private bool m_isCoolDown = false;

    public void SetInfomation()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.Skill_Key_Text).text = SetString(GameManager.Inst.m_quickSlot.m_finalSkillKey.ToString());
        GetText((int)Texts.Skill_Cooldown_Text).text = "";
        GetImage((int)Images.Skill_Cooldown_Image).fillAmount = 0;
        GetImage((int)Images.Skill_Cooldown_Image).gameObject.SetActive(false);

        SetColor(0);

        SetEventHandler();
    }

    public void InputKey()
    {
        if (m_skillData == null || m_isCoolDown == true)
            return;

        if (GameManager.Inst.m_player.m_stat.Mp >= m_skillData.m_mp)
        {
            GameManager.Inst.m_player.m_stat.Mp -= m_skillData.m_mp;
            GameManager.Inst.m_player.m_animEvent.m_anim.Play(m_skillData.m_animName);
            StartCoroutine(CoolDown());
        }
    }

    protected override void OnBeginDragSlot(PointerEventData eventData)
    {
        if (m_isCoolDown == true || GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        base.OnBeginDragSlot(eventData);
    }

    protected override void OnEndDragSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        // 마지막 Drag위치가 UI가 아니라면
        if (m_skillData.IsNull() == false && !EventSystem.current.IsPointerOverGameObject())
        {
            if (m_isCoolDown == false)
                ClearSlot();
        }

        base.OnEndDragSlot(eventData);
    }

    protected override void OnDropSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        UI_Slot dragSlot = UI_Drag_Slot.instance.m_slot;

        if (dragSlot.IsNull() == false)
        {
            if (dragSlot == this)
                return;

            if ((dragSlot is UI_Slot_Skill) == true)
            {
                if (m_isCoolDown == false)
                    ChangeSkill(dragSlot as UI_Slot_Skill);
            }
            else
                return;
        }
    }

    private void ChangeSkill(UI_Slot_Skill skillSlot)
    {
        SetSkill(skillSlot.m_skillData);

        //if (skillSlot is UI_Drag_Skill)
        //    (skillSlot as UI_Drag_Skill).ClearSlot();
    }

    public void SetSkill(SkillData skill)
    {
        if (skill.m_skillType != SkillType.FinalSkill)
            return;

        m_skillData = skill;
        GetImage((int)Images.Skill_Icon).sprite = skill.m_icon;

        SetColor(1);
    }

    public override void SetColor(float alpha)
    {
        Color color = GetImage((int)Images.Skill_Icon).color;
        color.a = alpha;
        GetImage((int)Images.Skill_Icon).color = color;
    }

    public override void ClearSlot()
    {
        m_skillData = null;
        GetImage((int)Images.Skill_Cooldown_Image).fillAmount = 1;
        GetImage((int)Images.Skill_Cooldown_Image).gameObject.SetActive(false);
        SetColor(0);
    }

    private IEnumerator CoolDown()
    {
        float tick = 1f / m_skillData.m_coolTime;
        float t = 0;

        m_isCoolDown = true;
        GetImage((int)Images.Skill_Cooldown_Image).gameObject.SetActive(true);
        GetImage((int)Images.Skill_Cooldown_Image).fillAmount = 1f;
        GetText((int)Texts.Skill_Cooldown_Text).text = Mathf.CeilToInt(m_skillData.m_coolTime).ToString();

        while (GetImage((int)Images.Skill_Cooldown_Image).fillAmount > 0f)
        {
            m_time += Time.deltaTime;
            float displayedTime = m_skillData.m_coolTime - m_time;

            GetImage((int)Images.Skill_Cooldown_Image).fillAmount = Mathf.Lerp(1, 0, t);
            t += Time.deltaTime * tick;

            if (displayedTime <= 1f)
            {
                GetText((int)Texts.Skill_Cooldown_Text).text = displayedTime.ToString("F1");
            }
            else
            {
                GetText((int)Texts.Skill_Cooldown_Text).text = Mathf.CeilToInt(displayedTime).ToString();
            }

            yield return null;
        }

        m_time = 0f;
        m_isCoolDown = false;
        GetText((int)Texts.Skill_Cooldown_Text).text = "";
        GetImage((int)Images.Skill_Cooldown_Image).gameObject.SetActive(false);
    }
}