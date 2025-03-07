using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Slot_Quest : MonoBehaviour
{
    [SerializeField]
    Data.Quest m_quest;

    [SerializeField]
    TextMeshProUGUI m_text;

    public void SetSlot(Data.Quest quest)
    {
        m_quest = quest;
        m_text.text = m_quest.QuestName;
    }

    public void OnClickButton()
    {
        GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().SetQuestInfo(m_quest);
        GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().GiveUpButtonSwitch(!m_quest.Completed);
    }
}
