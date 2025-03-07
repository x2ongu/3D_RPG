using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_QuickSlot_Skill : UI_Drag_Skill
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
    [SerializeField]
    private bool m_isCoolDown = false;

    public void SetInfomation()
    {
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.Skill_Key_Text).text = SetString(m_inputKey.ToString());
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
        if (eventData.button != PointerEventData.InputButton.Left || GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        // 마지막 Drag위치가 UI가 아니라면
        if (m_skillData.IsNull() == false && !EventSystem.current.IsPointerOverGameObject())
            ClearSlot();

        base.OnEndDragSlot(eventData);
    }

    protected override void OnDropSlot(PointerEventData eventData)
    {
        if (GameManager.Inst.m_player.m_isAttackMode == true)
            return;

        UI_Slot dragSlot = UI_Drag_Slot.instance.m_slot;

        if (dragSlot.IsNull() == false)
        {
            // 해당 Script를 Drag해 본인 Script에 Drop할 경우 return
            if (dragSlot == this)
                return;

            if ((dragSlot is UI_Slot_Skill) == true)            // dragSlot이 Skill 창에서 온 경우
            {
                ChangeSkill(dragSlot as UI_Slot_Skill);
            }
            else if ((dragSlot is UI_QuickSlot_Skill) == true)  // dragSlot이 Quick Slot 창에서 온 경우
            {
                ExchangeSkill(dragSlot as UI_QuickSlot_Skill);
            }
            else                                                // dragSlot이 위 조건 외에 다른 창에서 온 경우 return
                return;
        }
    }

    // Quick Slot에 Skill이 없을 경우 Skill 등록
    private void ChangeSkill(UI_Slot_Skill skillSlot)
    {
        if (m_isCoolDown)
            return;

        SetSkill(skillSlot.m_skillData);
    }

    private void ExchangeSkill(UI_QuickSlot_Skill skillQuickSlot)
    {
        if (m_isCoolDown || skillQuickSlot.m_isCoolDown)
            return;

        SkillData temp = this.m_skillData;

        SetSkill(skillQuickSlot.m_skillData);

        if (temp != null)
        {
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_QuickSlot_Skill>().SetSkill(temp);
        }
        else
        {
            UI_Drag_Slot.instance.m_slot.GetComponent<UI_QuickSlot_Skill>().ClearSlot();
        }
    }

    public void SetSkill(SkillData skill)
    {
        if (skill.m_skillType == SkillType.FinalSkill)
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
        GetImage((int)Images.Skill_Cooldown_Image).fillAmount = 1;
        GetImage((int)Images.Skill_Cooldown_Image).gameObject.SetActive(false);
        SetColor(0);
        m_skillData = null;
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