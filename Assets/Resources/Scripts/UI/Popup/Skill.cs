using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : UI_PopUp
{
    [Header("# Info")]
    [SerializeField]
    private SkillData[] m_skillDatas;
    [SerializeField]
    private GameObject m_gridSetting;
    [SerializeField]
    private GameObject m_slotPrefab;

    private UI_Slot_Skill[] m_slots;

    public override void Init()
    {
        for (int i = 0; i < m_skillDatas.Length; i++)
        {
            Instantiate(m_slotPrefab, m_gridSetting.transform);
        }

        m_slots = m_gridSetting.GetComponentsInChildren<UI_Slot_Skill>();

        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].m_skillData = m_skillDatas[i];
            m_slots[i].SetInfo();
        }
    }
}