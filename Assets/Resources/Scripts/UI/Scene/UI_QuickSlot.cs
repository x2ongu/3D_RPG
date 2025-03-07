using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuickSlot : MonoBehaviour
{
    [Header("# Base Skill")]
    public UI_QuickSlot_Skill[] m_skillSlot;
    public KeyCode[] m_skillKey = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };

    [Header("# Final Skill")]
    public UI_QuickSlot_FinalSkill m_finalSkillSlot;
    public KeyCode m_finalSkillKey = KeyCode.F;

    [Header("# Consumable Item")]
    public UI_QuickSlot_Item[] m_itemSlot;
    public KeyCode[] m_itemKey = { KeyCode.Alpha1, KeyCode.Alpha2 };


    private void Awake()
    {
        SetQuickSlotKey();
    }

    public void InputSkillQuickSlot()
    {
        InputBaseSkill(0);
        InputBaseSkill(1);
        InputBaseSkill(2);
        InputBaseSkill(3);

        InputFinalSkill();
    }

    public void InputItemQuickSlot()
    {
        InputItem(0);
        InputItem(1);
    }

    private void InputBaseSkill(int i)
    {
        if (Input.GetKeyDown(m_skillKey[i]))
        {
            m_skillSlot[i].InputKey();
            GameManager.Inst.m_player.m_animEvent.m_skillData = m_skillSlot[i].m_skillData;
        }
    }

    private void InputFinalSkill()
    {
        if (Input.GetKeyDown(m_finalSkillKey))
        {
            m_finalSkillSlot.InputKey();
        }
    }

    private void InputItem(int i)
    {
        if (Input.GetKeyDown(m_itemKey[i]))
        {
            m_itemSlot[i].InputKey();
        }
    }

    private void SetQuickSlotKey()
    {
        m_skillSlot = GetComponentsInChildren<UI_QuickSlot_Skill>();
        m_finalSkillSlot = GetComponentInChildren<UI_QuickSlot_FinalSkill>();
        m_itemSlot = GetComponentsInChildren<UI_QuickSlot_Item>();

        // 일반 스킬 키 셋팅
        for (int i = 0; i < m_skillSlot.Length; i++)
        {
            m_skillSlot[i].m_inputKey = m_skillKey[i];
            m_skillSlot[i].SetInfomation();
        }

        m_finalSkillSlot.m_inputKey = m_finalSkillKey;
        m_finalSkillSlot.SetInfomation();

        for(int i = 0; i < m_itemSlot.Length; i++)
        {
            m_itemSlot[i].m_inputKey = m_itemKey[i];
            m_itemSlot[i].SetInfomation();
        }
    }
}
