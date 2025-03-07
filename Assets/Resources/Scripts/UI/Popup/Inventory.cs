using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : UI_PopUp
{
    [Header("# Info")]
    [SerializeField]
    private GameObject m_gridSetting;
    [SerializeField]
    private TextMeshProUGUI m_goldText;

    [SerializeField]
    private UI_QuickSlot_Item m_itemQuickSlot1;
    [SerializeField]
    private UI_QuickSlot_Item m_itemQuickSlot2;

    //[HideInInspector]
    public UI_Slot_Inven[] m_slots;

    private void Update()
    {
        m_goldText.text = string.Format("{0:#,###}", GameManager.Inst.m_player.m_stat.Gold);
    }

    public void SetSlot()
    {
        m_slots = m_gridSetting.GetComponentsInChildren<UI_Slot_Inven>();
    }

    public UI_Slot_Inven FindQuestItem(string name)
    {
        for (int i = 0; i < m_slots.Length; i++)
        {
            if (m_slots[i].m_itemData == null)
                continue;

            if (string.Equals(m_slots[i].m_itemData.name, name))
                return m_slots[i];
        }

        return null;
    }

    public void AcquireItem(ItemData item, int count = 1)
    {
        if (ItemData.ItemType.Gold != item.m_itemType)
        {
            if (ItemData.ItemType.Equipment != item.m_itemType)
            {
                for (int i = 0; i < m_slots.Length; i++)
                {
                    if (m_slots[i].m_itemData != null)
                    {
                        if (m_slots[i].m_itemData.m_itemName == item.m_itemName)
                        {
                            m_slots[i].AddItem(item, count);

                            if (ItemData.ItemType.Consumable == item.m_itemType)
                                SetQuickSlot(m_slots[i]);

                            return;
                        }
                    }
                }
            }

            for (int i = 0; i < m_slots.Length; i++)
            {
                if (m_slots[i].m_itemData == null)
                {
                    m_slots[i].AddItem(item, count);
                    return;
                }
            }
        }
        else
        {
            SetGold((GoldData)item);
        }
    }

    public void SetQuickSlot(UI_Slot_Inven invenSlot)
    {
        SetQuickSlot(m_itemQuickSlot1, invenSlot);
        SetQuickSlot(m_itemQuickSlot2, invenSlot);
    }

    private void SetQuickSlot(UI_QuickSlot_Item quickSlot, UI_Slot_Inven invenSlot)
    {
        if (quickSlot.m_itemData == null)
            return;

        if (invenSlot.m_itemData.m_itemName == quickSlot.m_itemData.m_itemName)
        {
            quickSlot.m_invenSlot = invenSlot;
        }
    }

    private void SetGold(GoldData gold)
    {
        int value = Random.Range(gold.m_minGold, gold.m_maxGold + 1);
        GameManager.Inst.m_player.m_stat.Gold += value;
    }
}
