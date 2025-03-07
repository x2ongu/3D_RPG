using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UI_Dialogue : UI_PopUp
{
    enum Texts
    {
        Text_Dialogue,
        Text_Speaker
    }

    enum Buttons
    {
        Button_Skip,
        Button_SkipAll,
        Button_Quit,
        Button_Accept,
        Button_Reject,
        Button_Complete
    }

    public Quest m_quest;

    Data.QuestDialogue m_dialogue;
    Coroutine m_typing;

    int m_index;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Talking(m_index);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SkipDialogue(m_index);
        }
    }

    private void OnEnable()
    {
        SetDialogueIndex();
        Talking(m_index);
    }

    private void OnDisable()
    {
        
    }

    public override void Init()
    {
        m_quest = GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>();
    }

    public void SetDialogueIndex()
    {
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.Button_Accept).gameObject.SetActive(false);
        GetButton((int)Buttons.Button_Reject).gameObject.SetActive(false);
        GetButton((int)Buttons.Button_Complete).gameObject.SetActive(false);

        if (GameManager.Inst.m_quest.CurrentQuest.Accepted == false)
            m_index = GameManager.Inst.m_quest.CurrentQuest.QuestID * 100;
        else if (GameManager.Inst.m_quest.CurrentQuest.Accepted == true)
            m_index = GameManager.Inst.m_quest.CurrentQuest.QuestID * 100 + 50;
    }

    // 수락 & 거절, 완료 버튼이 각각 퀘스트 수락, 완료 시 Active가 켜져야 하며 해당 버튼이 활성화 됐을 때 스킵이 불가능해야 한다
    // 그리고 수락, 거절, 완료 버튼에 알맞은 메소드를 부여해야 한다.
    public void Talking(int index)
    {
        if (m_typing != null)
        {
            StopCoroutine(m_typing);
            GetText((int)Texts.Text_Dialogue).text = m_dialogue.Dialogue;
            m_typing = null;
            return;
        }

        if (!Managers.Data.QuestDialogueDict.ContainsKey(index))
            return;

        m_dialogue = Managers.Data.QuestDialogueDict[index];
        m_index += 1;

        BindText(typeof(Texts));
        GetText((int)Texts.Text_Speaker).text = m_dialogue.Speaker;
        m_typing = StartCoroutine(TypingText(m_dialogue.Dialogue));

        // 만약 Dialogue.json/State = 0 이면 수락, 거절, 완료버튼 모두 끄기
        if (m_dialogue.State == 1)
        {
            GetButton((int)Buttons.Button_Accept).gameObject.SetActive(true);
            GetButton((int)Buttons.Button_Reject).gameObject.SetActive(true);
            GetButton((int)Buttons.Button_Accept).onClick.RemoveAllListeners();
            GetButton((int)Buttons.Button_Reject).onClick.RemoveAllListeners();
            GetButton((int)Buttons.Button_Accept).onClick.AddListener(() => Quest_Accept());
            GetButton((int)Buttons.Button_Reject).onClick.AddListener(() => Quest_Reject());
        }
        else if (m_dialogue.State == 2)
        {
            GetButton((int)Buttons.Button_Complete).gameObject.SetActive(true);
            GetButton((int)Buttons.Button_Complete).onClick.RemoveAllListeners();
            GetButton((int)Buttons.Button_Complete).onClick.AddListener(() => Quest_Complete());
        }
        else
            return;
    }

    public void SkipDialogue(int index)
    {
        if (m_typing != null)
        {
            StopCoroutine(m_typing);
            m_typing = null;
        }

        while (true)
        {
            if (Managers.Data.QuestDialogueDict.ContainsKey(index))
            {
                index++;
                continue;
            }
            else
            {
                m_index = index - 1;
                m_dialogue = Managers.Data.QuestDialogueDict[m_index];
                break;
            }
        }

        BindText(typeof(Texts));
        GetText((int)Texts.Text_Speaker).text = m_dialogue.Speaker;
        GetText((int)Texts.Text_Dialogue).text = m_dialogue.Dialogue;

        if (m_dialogue.State == 1)
        {
            GetButton((int)Buttons.Button_Accept).gameObject.SetActive(true);
            GetButton((int)Buttons.Button_Reject).gameObject.SetActive(true);
            GetButton((int)Buttons.Button_Accept).onClick.RemoveAllListeners();
            GetButton((int)Buttons.Button_Reject).onClick.RemoveAllListeners();
            GetButton((int)Buttons.Button_Accept).onClick.AddListener(() => Quest_Accept());
            GetButton((int)Buttons.Button_Reject).onClick.AddListener(() => Quest_Reject());
        }
        else if (m_dialogue.State == 2)
        {
            GetButton((int)Buttons.Button_Complete).gameObject.SetActive(true);
            GetButton((int)Buttons.Button_Complete).onClick.RemoveAllListeners();
            GetButton((int)Buttons.Button_Complete).onClick.AddListener(() => Quest_Complete());
        }
        else
            return;
    }

    // 아래는 버튼을 눌렀을 때 작동할 메소드들
    public void Quest_Accept()      // 퀘스트 수락 시
    {
        GameManager.Inst.m_quest.CurrentQuest.Accepted = true;
        GameManager.Inst.m_quest.ChangeQuestProgress(Define.Quest.WorkInProgress);
        GameManager.Inst.m_quest.CollectQuestProcess();

        m_quest.QuestSlot = Managers.Resource.Instantiate("UI/SubItem/Quest_Slot", GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().CurrentQuest_List.transform);
        m_quest.QuestSlot.GetComponent<UI_Slot_Quest>().SetSlot(GameManager.Inst.m_quest.CurrentQuest);

        GetButton((int)Buttons.Button_Accept).gameObject.SetActive(false);
        GetButton((int)Buttons.Button_Reject).gameObject.SetActive(false);
        GameManager.Inst.m_popup.ClosePopUp(this, false);
    }

    public void Quest_Reject()      // 퀘스트 거절 시
    {
        GameManager.Inst.m_quest.ChangeQuestProgress(Define.Quest.NotStarted);

        GetButton((int)Buttons.Button_Accept).gameObject.SetActive(false);
        GetButton((int)Buttons.Button_Reject).gameObject.SetActive(false);
        GameManager.Inst.m_popup.ClosePopUp(this, false);
    }

    public void Quest_Complete()    // 퀘스트 완료 시
    {
        GameManager.Inst.m_quest.CurrentQuest.Completed = true;
        GameManager.Inst.m_quest.ChangeQuestProgress(Define.Quest.NotStarted);
        Managers.Sound.Play("UI/Complete");

        if (m_quest.QuestSlot == null)
        {
            m_quest.QuestSlot = GameManager.Inst.m_quest.m_acceptedSlot;
            GameManager.Inst.m_quest.m_acceptedSlot = null;
        }
        m_quest.QuestSlot.transform.SetParent(GameManager.Inst.m_popup.m_questPopup.GetComponent<Quest>().CompleteQuest_List.transform);
        m_quest.QuestSlot.GetComponent<UI_Slot_Quest>().SetSlot(GameManager.Inst.m_quest.CurrentQuest);

        GameManager.Inst.m_player.m_stat.Exp += GameManager.Inst.m_quest.CurrentQuest.RewardExp;
        GameManager.Inst.m_player.m_stat.Gold += GameManager.Inst.m_quest.CurrentQuest.RewardGold;

        GameManager.Inst.m_quest.GetCurrentQuest();
        GetButton((int)Buttons.Button_Complete).gameObject.SetActive(false);
        GameManager.Inst.m_popup.ClosePopUp(this, false);
    }

    IEnumerator TypingText(string text)
    {
        GetText((int)Texts.Text_Dialogue).text = string.Empty;

        StringBuilder stringBuilder = new StringBuilder();
        
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(text[i]);
            GetText((int)Texts.Text_Dialogue).text = stringBuilder.ToString();
            yield return new WaitForSeconds(0.1f);
        }

        m_typing = null;
    }
}