using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private Data.Quest m_currentQuest;
    public Data.Quest CurrentQuest { get { return m_currentQuest; } set { m_currentQuest = value; } }

    [SerializeField]
    private Define.Quest m_questProgress;
    public Define.Quest QuestProgress { get { return m_questProgress; } set { m_questProgress = value; } }

    public event Action QuestUpdated;
    public event Action<Define.Quest> QuestProgressd;

    [HideInInspector]
    public GameObject m_acceptedSlot;
    private GameObject m_worldSpaceIcon;
    private GameObject m_miniMapIcon;

    public bool questSkip = false;

    private void Start()
    {
        //GetCurrentQuest();
    }

    public void GetCurrentQuest(int questID = -1, bool accepted = false)
    {
        Dictionary<int, Data.Quest> dict = Managers.Data.QuestDict;
        m_currentQuest = null;

        if (questID == -1)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                Data.Quest quest = dict[i + 1];

                if (quest.Completed == false)
                {
                    m_currentQuest = quest;
                    ChangeQuestProgress(Define.Quest.NotStarted);

                    return;
                }
            }
        }
        else if (questID == 0)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                Data.Quest quest = dict[i + 1];

                if (quest != null)
                {
                    quest.Accepted = true;
                    quest.Completed = true;

                    GameObject slot = Managers.Resource.Instantiate("UI/SubItem/Quest_Slot", GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().CompleteQuest_List.transform);
                    slot.GetComponent<UI_Slot_Quest>().SetSlot(quest);
                }
            }
        }
        else
        {
            for (int i = 0; i < dict.Count; i++)
            {
                Data.Quest quest = dict[i + 1];

                if (quest.QuestID == questID)
                {
                    m_currentQuest = quest;
                    quest.Accepted = accepted;

                    if (m_currentQuest.Accepted)
                    {
                        m_acceptedSlot = Managers.Resource.Instantiate("UI/SubItem/Quest_Slot", GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().CurrentQuest_List.transform);
                        m_acceptedSlot.GetComponent<UI_Slot_Quest>().SetSlot(GameManager.Inst.m_quest.CurrentQuest);

                        ChangeQuestProgress(Define.Quest.WorkInProgress);
                    }
                    else
                        ChangeQuestProgress(Define.Quest.NotStarted);

                    return;
                }

                quest.Accepted = true;
                quest.Completed = true;

                GameObject slot = Managers.Resource.Instantiate("UI/SubItem/Quest_Slot", GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().CompleteQuest_List.transform);
                slot.GetComponent<UI_Slot_Quest>().SetSlot(quest);
            }
        }        

        if (m_currentQuest == null)
            ChangeQuestProgress(Define.Quest.Finished);
    }

    public void ChangeQuestProgress(Define.Quest questProgress)
    {
        SetIcon(questProgress);
        QuestProgressd?.Invoke(questProgress);
        QuestProgress = questProgress;
    }

    public void SetQuestNPC()
    {
        if (m_currentQuest == null)
            return;

        if (m_currentQuest.Accepted == false)
        {
            if (string.Equals(GameManager.Inst.m_player.m_interManager.NearestNPC.name, m_currentQuest.QuestStartNPC))
            {
                GameManager.Inst.m_player.m_interManager.NearestNPC.GetComponent<INPC>().DoInteraction(true);
                return;
            }
        }
        else if (m_currentQuest.Accepted == true && m_currentQuest.Completed == false)
        {
            if (m_questProgress != Define.Quest.CanBeCompleted && questSkip == false)
                return;

            if (string.Equals(GameManager.Inst.m_player.m_interManager.NearestNPC.name, m_currentQuest.QuestEndNPC))
            {
                GameManager.Inst.m_player.m_interManager.NearestNPC.GetComponent<INPC>().DoInteraction(true);
                return;
            }
        }
    }    

    public string SetQuestExplainText(Data.Quest quest, bool justExplain = false)
    {
        string text = "";

        for (int i = 0; i < quest.QuestCategory.Length; i++)
        {
            text += quest.QuestExplain[i];

            if (!justExplain)
            {
                if (string.Equals(quest.QuestCategory[i], "Kill"))
                {
                    Data.KillQuest killQuest = Managers.Data.KillQuestDict[quest.Key[i]];
                    text += $" ({killQuest.Count}/{killQuest.GoalCount})";
                }
                else if (string.Equals(quest.QuestCategory[i], "Collect"))
                {
                    Data.CollectQuest collectQuest = Managers.Data.CollectQuestDict[quest.Key[i]];
                    text += $" ({collectQuest.Count}/{collectQuest.GoalCount})";
                }
            }            

            if (i + 1 < quest.QuestCategory.Length)
                text += "\n";
        }

        return text;
    }

    public void KillQuestProcess(GameObject enemy = null, int count = 0)
    {
        if (m_currentQuest == null)
            return;

        if (m_currentQuest.Accepted == false)
            return;

        for (int i = 0; i < m_currentQuest.QuestCategory.Length; i++)
        {
            string category = m_currentQuest.QuestCategory[i];
            int key = m_currentQuest.Key[i];

            if (string.Equals(category, "Kill"))
            {
                Data.KillQuest killQuest = Managers.Data.KillQuestDict[key];

                if (enemy == null)  // 그냥 최초에 로드 시 사용함
                {
                    killQuest.Count = count;

                    if (killQuest.Count >= killQuest.GoalCount)
                    {
                        killQuest.Count = killQuest.GoalCount;
                        killQuest.Completed = true;
                    }

                    QuestUpdated?.Invoke();
                }
                else if (string.Equals(killQuest.TargetName, enemy.name))   // 적 처치마다 적용
                {
                    if (killQuest.Count < killQuest.GoalCount)
                    {
                        killQuest.Count += 1;

                        if (killQuest.Count >= killQuest.GoalCount)
                        {
                            killQuest.Count = killQuest.GoalCount;
                            killQuest.Completed = true;
                        }

                        QuestUpdated?.Invoke();
                    }
                }
            }
        }

        CheckQuestCompletion();
    }

    public void CollectQuestProcess(ItemData item = null, int count = 0)
    {
        if (m_currentQuest == null)
            return;

        if (m_currentQuest.Accepted == false)
            return;

        for (int i = 0; i < m_currentQuest.QuestCategory.Length; i++)
        {
            string category = m_currentQuest.QuestCategory[i];
            int key = m_currentQuest.Key[i];

            if (string.Equals(category, "Collect"))
            {
                Data.CollectQuest collectQuest = Managers.Data.CollectQuestDict[key];
                if (item == null)
                {
                    UI_Slot_Inven slot = GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>().FindQuestItem(collectQuest.ItemName);

                    if (slot == null)
                        continue;

                    collectQuest.Count = slot.m_itemCount;
                }
                else
                {
                    if (string.Equals(collectQuest.ItemName, item.name))
                        collectQuest.Count = count;
                }

                if (collectQuest.Count >= collectQuest.GoalCount)
                {
                    collectQuest.Count = collectQuest.GoalCount;
                    collectQuest.Completed = true;
                }
                else
                    collectQuest.Completed = false;

                QuestUpdated?.Invoke();
            }
        }

        CheckQuestCompletion();
    }

    private void CheckQuestCompletion()
    {
        bool allConditionsMet = true;

        for (int i = 0; i < m_currentQuest.QuestCategory.Length; i++)
        {
            string category = m_currentQuest.QuestCategory[i];
            int key = m_currentQuest.Key[i];

            if (string.Equals(category, "Kill"))
            {
                if (!Managers.Data.KillQuestDict[key].Completed)
                    allConditionsMet = false;
            }
            else if (string.Equals(category, "Collect"))
            {
                if (!Managers.Data.CollectQuestDict[key].Completed)
                    allConditionsMet = false;
            }
        }

        if (allConditionsMet)
            ChangeQuestProgress(Define.Quest.CanBeCompleted);
    }

    private void SetIcon(Define.Quest questProgress)
    {
        switch (questProgress)
        {
            case Define.Quest.NotStarted:
                LoadQuestIcon("NotStarted");
                break;
            case Define.Quest.WorkInProgress:
                LoadQuestIcon("WorkInProgress");
                break;
            case Define.Quest.CanBeCompleted:
                LoadQuestIcon("CanBeCompleted", true);
                break;
            case Define.Quest.Finished:
                Managers.Resource.Destroy(m_miniMapIcon);
                Managers.Resource.Destroy(m_worldSpaceIcon);
                break;
        }
    }

    private void LoadQuestIcon(string progress, bool canBeComplete = false)
    {
        if (m_worldSpaceIcon == null)
            m_worldSpaceIcon = Managers.Resource.Instantiate("UI/WorldSpace/Quest_Icon");

        if (m_miniMapIcon == null)
            m_miniMapIcon = Managers.Resource.Instantiate("UI/WorldSpace/Quest_Minimap");

        Sprite sprite = Managers.Resource.Load<Sprite>($"Textures/Minimap_Icon/{progress}");
        m_worldSpaceIcon.GetComponentInChildren<Image>().sprite = sprite;
        m_miniMapIcon.GetComponent<SpriteRenderer>().sprite = sprite;

        GameObject npc;

        if (!canBeComplete)
            npc = Managers.NPC.GetNPC(m_currentQuest.QuestStartNPC);
        else
            npc = Managers.NPC.GetNPC(m_currentQuest.QuestEndNPC);

        m_worldSpaceIcon.transform.position = new Vector3(npc.transform.position.x, npc.GetComponentInChildren<CapsuleCollider>().height * 0.5f + 1.5f, npc.transform.position.z);
        m_miniMapIcon.transform.position = new Vector3(npc.transform.position.x, 7f, npc.transform.position.z);
    }

    public void Clear()
    {
        m_worldSpaceIcon = null;
        m_miniMapIcon = null;
    }
}