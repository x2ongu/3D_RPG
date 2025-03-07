using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : UI_PopUp
{
    public Inventory m_inven;

    public ItemData[] m_itemDatas;
    [SerializeField]
    private GameObject m_gridSetting;
    [SerializeField]
    private GameObject m_slotPrefab;

    private UI_Slot_Shop[] m_slots;

    public override void Init()
    {
        m_closeButton = m_inven.m_closeButton;

        for (int i = 0; i < m_itemDatas.Length; i++)
        {
            Instantiate(m_slotPrefab, m_gridSetting.transform);
        }

        m_slots = m_gridSetting.GetComponentsInChildren<UI_Slot_Shop>();

        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i].m_itemData = m_itemDatas[i];
            m_slots[i].SetInfo();
        }
    }
}
