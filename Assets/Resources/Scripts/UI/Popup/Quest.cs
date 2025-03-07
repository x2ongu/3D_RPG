using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : UI_PopUp
{
    enum Texts
    {
        Text_QuestName,
        Text_QuestExplain,
        Text_Exp,
        Text_Gold
    }

    enum Buttons
    {
        Button_CurrentQuest,
        Button_CompleteQuest,
        Button_GiveUp
    }

    enum Objects
    {
        CurrentQuest_Grid,
        CompleteQuest_Grid,
        Quest_Info_Grid,
        Reward_Exp,
        Reward_Gold

    }

    public GameObject CurrentQuest_List
    {
        get
        {
            BindObject(typeof(Objects));
            return GetObject((int)Objects.CurrentQuest_Grid); 
        }
    }

    public GameObject CompleteQuest_List
    {
        get
        {
            BindObject(typeof(Objects));
            return GetObject((int)Objects.CompleteQuest_Grid);
        }
    }

    private GameObject m_questSlot;
    public GameObject QuestSlot { get { return m_questSlot; } set { m_questSlot = value; } }

    private Data.Quest m_questInInfo;

    public override void Init()
    {
        GameManager.Inst.m_quest.QuestUpdated += SetQuestUpdated;
    }

    private void OnEnable()
    {
        ClearInfo();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_GiveUp).gameObject.SetActive(false);

        CompleteQuest_List.SetActive(false);
    }

    private void OnDisable()
    {
        CurrentQuest_List.SetActive(true);
        CompleteQuest_List.SetActive(true);

        GetButton((int)Buttons.Button_CurrentQuest).interactable = true;
        GetButton((int)Buttons.Button_CompleteQuest).interactable = true;
    }

    public void SetQuestInfo(Data.Quest quest)
    {
        ClearInfo();

        GetText((int)Texts.Text_QuestName).text = quest.QuestName;
        GetText((int)Texts.Text_QuestExplain).text = GameManager.Inst.m_quest.SetQuestExplainText(quest, quest.Completed);

        GetObject((int)Objects.Reward_Exp).SetActive(true);
        GetObject((int)Objects.Reward_Gold).SetActive(true);
        GetText((int)Texts.Text_Exp).text = quest.RewardExp.ToString() + "Exp";
        GetText((int)Texts.Text_Gold).text = quest.RewardGold.ToString() + "G";

        m_questInInfo = quest;
    }

    public void Button_CurrentQuest()
    {
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_CurrentQuest).interactable = false;
        GetButton((int)Buttons.Button_CompleteQuest).interactable = true;

        CurrentQuest_List.SetActive(true);
        CompleteQuest_List.SetActive(false);
    }

    public void Button_CompleteQuest()
    {
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_CurrentQuest).interactable = true;
        GetButton((int)Buttons.Button_CompleteQuest).interactable = false;

        CurrentQuest_List.SetActive(false);
        CompleteQuest_List.SetActive(true);
    }

    public void Button_GiveUp()
    {
        GameManager.Inst.m_quest.CurrentQuest.Accepted = false;
        GameManager.Inst.m_quest.ChangeQuestProgress(Define.Quest.NotStarted);

        for (int i = 0; i < GameManager.Inst.m_quest.CurrentQuest.QuestCategory.Length; i++)
        {
            if (string.Equals(GameManager.Inst.m_quest.CurrentQuest.QuestCategory[i], "Kill"))
            {
                Data.KillQuest killQuest = Managers.Data.KillQuestDict[GameManager.Inst.m_quest.CurrentQuest.Key[i]];
                killQuest.Count = 0;
            }
        }

        Destroy(m_questSlot);

        ClearInfo();

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_GiveUp).gameObject.SetActive(false);
    }

    public void GiveUpButtonSwitch(bool key)
    {
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_GiveUp).gameObject.SetActive(key);
    }

    private void ClearInfo()
    {
        m_questInInfo = null;

        BindText(typeof(Texts));
        GetText((int)Texts.Text_QuestName).text = "";
        GetText((int)Texts.Text_QuestExplain).text = "";

        BindObject(typeof(Objects));
        GetObject((int)Objects.Reward_Exp).SetActive(false);
        GetObject((int)Objects.Reward_Gold).SetActive(false);
    }

    private void SetQuestUpdated()
    {
        if (m_questInInfo != GameManager.Inst.m_quest.CurrentQuest || m_questInInfo == null)
            return;

        BindText(typeof(Texts));
        GetText((int)Texts.Text_QuestExplain).text = GameManager.Inst.m_quest.SetQuestExplainText(GameManager.Inst.m_quest.CurrentQuest);
    }
}