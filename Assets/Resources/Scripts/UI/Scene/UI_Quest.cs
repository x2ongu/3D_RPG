using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Quest : UI_Base
{
    enum Images
    {
        Image_QuestName,
        Image_QuestExplain
    }

    enum Texts
    {
        Text_QuestName,
        Text_QuestExplain
    }

    [SerializeField]
    private GameObject m_questUI;

    private void Awake()
    {
        m_questUI.SetActive(false);
        GameManager.Inst.m_quest.QuestUpdated -= SetQuestUpdated;
        GameManager.Inst.m_quest.QuestUpdated += SetQuestUpdated;
        GameManager.Inst.m_quest.QuestProgressd -= SetQuestUI;
        GameManager.Inst.m_quest.QuestProgressd += SetQuestUI;
    }

    private void SetQuestUI(Define.Quest quest)
    {
        switch (quest)
        {
            case Define.Quest.WorkInProgress:
                UI_ActiveTrue();
                break;
            case Define.Quest.CanBeCompleted:
                UI_ActiveTrue();
                break;
            default:
                UI_ActiveFalse();
                break;
        }
    }

    private void UI_ActiveFalse()
    {
        if (m_questUI == null)
            return;

        m_questUI.SetActive(false);
    }

    private void UI_ActiveTrue()
    {
        if (m_questUI == null)
            return;

        m_questUI.SetActive(true);

        BindText(typeof(Texts));
        GetText((int)Texts.Text_QuestName).text = GameManager.Inst.m_quest.CurrentQuest.QuestName;
        GetText((int)Texts.Text_QuestExplain).text = GameManager.Inst.m_quest.SetQuestExplainText(GameManager.Inst.m_quest.CurrentQuest);
    }

    private void SetQuestUpdated()
    {
        BindText(typeof(Texts));
        GetText((int)Texts.Text_QuestExplain).text = GameManager.Inst.m_quest.SetQuestExplainText(GameManager.Inst.m_quest.CurrentQuest);
    }
}
