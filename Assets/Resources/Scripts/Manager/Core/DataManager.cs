using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class SaveData
{
    #region PlayerData
    public float posX;
    public float posZ;
    public float rotY;
    public int level;
    public int curExp;
    public int curHp;
    public int curMp;
    public int gold;
    #endregion

    #region StatsData
    public List<string> statsSlots;
    #endregion

    #region QuestData
    public int curQuestID;
    public bool curQuestAccepted;
    public List<int> curKillCount;
    #endregion

    #region QuickSlotData
    // 스킬 qwer창 따로, 궁극기 F창 따로
    public List<string> skillQuickSlots;
    public List<string> itemQuickSlots;
    public bool isThereFinalSkill;
    // 인벤 슬롯이랑 연동되어 있었던 소비아이템 창 따로
    #endregion

    #region InventoryData
    public List<string> invenSlots;
    public List<string> invenItemType;
    public List<int> invenItemCount;
    #endregion
}

public class DataManager
{
    SaveData saveData = new SaveData();

    string path;
    string fileName;

    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
    public Dictionary<int, Data.Enemy> EnemyDict { get; private set; } = new Dictionary<int, Data.Enemy>();

    #region Quest
    public Dictionary<int, Data.Quest> QuestDict { get; private set; } = new Dictionary<int, Data.Quest>();
    public Dictionary<int, Data.QuestDialogue> QuestDialogueDict { get; private set; } = new Dictionary<int, Data.QuestDialogue>();
    public Dictionary<int, Data.KillQuest> KillQuestDict { get; private set; } = new Dictionary<int, Data.KillQuest>();
    public Dictionary<int, Data.CollectQuest> CollectQuestDict { get; private set; } = new Dictionary<int, Data.CollectQuest>();
    #endregion

    public void Init()
    {
        path = Application.persistentDataPath + "/";
        fileName = "SaveFile";

        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
        EnemyDict = LoadJson<Data.EnemyData, int, Data.Enemy>("EnemyData").MakeDict();

        #region Quest
        QuestDict = LoadJson<Data.QuestData, int, Data.Quest>("QuestData/Quest").MakeDict();
        QuestDialogueDict = LoadJson<Data.QuestDialogueData, int, Data.QuestDialogue>("QuestData/QuestDialogue").MakeDict();
        KillQuestDict = LoadJson<Data.KillQuestData, int, Data.KillQuest>("QuestData/KillQuest").MakeDict();
        CollectQuestDict = LoadJson<Data.CollectQuestData, int, Data.CollectQuest>("QuestData/CollectQuest").MakeDict();
        #endregion
    }

    public void SaveData()
    {
        SavePlayerData();
        SaveStatsData();
        SaveQuestData();
        SaveInventoryData();
        SaveQuickSlotData();

        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        if (!File.Exists(path + fileName))
        {
            Inventory inven = GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>();
            ItemData baseWeapon = Managers.Resource.Load<ItemData>($"Data/ItemData/Equipment/Weapon_LongSword");

            if (baseWeapon != null)
                inven.AcquireItem(baseWeapon);

            GameManager.Inst.m_player.m_stat.Level = 1;
            GameManager.Inst.m_player.m_stat.SetStat(1);
            GameManager.Inst.m_quest.GetComponent<QuestManager>().GetCurrentQuest();

            return;
        }

        string data = File.ReadAllText(path + fileName);
        saveData = JsonUtility.FromJson<SaveData>(data);

        LoadPlayerData(saveData);
        LoadStatsData(saveData);
        LoadQuestData(saveData);
        LoadInventoryData(saveData);
        LoadQuickSlotData(saveData);
    }

    public IEnumerator SaveDataCoroutine()
    {
        SavePlayerData();
        SaveStatsData();
        SaveQuestData();
        SaveInventoryData();
        SaveQuickSlotData();

        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + fileName, data);

        yield return null;
    }

    void SavePlayerData()
    {
        Player player = GameManager.Inst.m_player;

        saveData.posX = player.transform.position.x;
        saveData.posZ = player.transform.position.z;
        saveData.rotY = player.transform.eulerAngles.y;

        saveData.level = player.m_stat.Level;
        saveData.curExp = player.m_stat.Exp;
        saveData.curHp = player.m_stat.Hp;
        saveData.curMp = player.m_stat.Mp;
        saveData.gold = player.m_stat.Gold;
    }
    void LoadPlayerData(SaveData saveData)
    {
        Player player = GameManager.Inst.m_player;

        player.m_navAgent.enabled = false;
        player.transform.position = new Vector3(saveData.posX, 0, saveData.posZ);
        player.transform.rotation = Quaternion.Euler(0, saveData.rotY, 0);
        player.m_navAgent.enabled = true;

        player.m_stat.Level = saveData.level;
        player.m_stat.SetStat(player.m_stat.Level);
        player.m_stat.Exp = saveData.curExp;
        player.m_stat.Hp = saveData.curHp;
        player.m_stat.Mp = saveData.curMp;

        player.m_stat.Gold = saveData.gold;
    }

    void SaveStatsData()
    {
        Stats stats = GameManager.Inst.m_popup.m_statsPopUp.GetComponent<Stats>();
        saveData.statsSlots = new List<string>();

        for (int i = 0; i < stats.m_slots.Length; i++)
        {
            if (stats.m_slots[i].m_itemData == null)
            {
                saveData.statsSlots.Add("");
                continue;
            }

            string itemName = stats.m_slots[i].m_itemData.ToString();

            int lastBracketIndex = itemName.LastIndexOf(" (");
            if (lastBracketIndex != -1)
                itemName = itemName.Substring(0, lastBracketIndex);

            saveData.statsSlots.Add(itemName);
        }
    }
    void LoadStatsData(SaveData saveData)
    {
        Stats stats = GameManager.Inst.m_popup.m_statsPopUp.GetComponent<Stats>();
        int slotCount = Mathf.Min(stats.m_slots.Length, saveData.statsSlots.Count);

        for (int i = 0; i < slotCount; i++)
        {
            if (string.IsNullOrEmpty(saveData.statsSlots[i]))
                continue;

            EquipmentData data = Managers.Resource.Load<EquipmentData>($"Data/ItemData/Equipment/{saveData.statsSlots[i]}");

            if (data != null)
            {
                if (GameManager.Inst.m_player.m_weapon == null && data.m_equipmentType == EquipmentData.EquipmentType.Weapon)
                {
                    GameObject obj = Managers.Resource.Instantiate($"Item/Equipment/{data.m_itemPrefab.name}");
                    obj.transform.localScale *= 2;
                }

                stats.SetItem(data, null);
                stats.m_slots[i].SetItem(data);
            }
        }
    }

    // 퀘스트 Save & Load  > QuestManager.cs 디벨롭 해야함
    void SaveQuestData()
    {
        Data.Quest currentQuest = GameManager.Inst.m_quest.CurrentQuest;
        saveData.curKillCount = new List<int>();

        saveData.curQuestID = currentQuest.QuestID;
        saveData.curQuestAccepted = currentQuest.Accepted;

        if (currentQuest.QuestID == 0)
            return;

        for (int i = 0; i < currentQuest.QuestCategory.Length; i++)
        {
            if (!string.Equals(currentQuest.QuestCategory[i], "Kill"))
                continue;

            int key = currentQuest.Key[i];
            Data.KillQuest killQuest = Managers.Data.KillQuestDict[key];

            if (killQuest != null)
                saveData.curKillCount.Add(killQuest.Count);
        }
    }
    void LoadQuestData(SaveData saveData)
    {
        Data.Quest currentQuest = GameManager.Inst.m_quest.CurrentQuest;
        currentQuest.QuestID = saveData.curQuestID;

        GameManager.Inst.m_quest.GetCurrentQuest(currentQuest.QuestID, saveData.curQuestAccepted);
        for (int i = 0; i < GameManager.Inst.m_quest.CurrentQuest.QuestCategory.Length; i++)
        {
            if (!string.Equals(GameManager.Inst.m_quest.CurrentQuest.QuestCategory[i], "Kill"))
                continue;

            int key = GameManager.Inst.m_quest.CurrentQuest.Key[i];
            Data.KillQuest killQuest = Managers.Data.KillQuestDict[key];
            killQuest.Count = saveData.curKillCount[i];

            GameManager.Inst.m_quest.KillQuestProcess(null, killQuest.Count);
        }
    }
    
    // Todo..
    // 퀵슬롯 Save & Load
    // 인벤토리 Save & Load
    void SaveQuickSlotData()
    {
        // 일반 스킬 퀵 슬롯
        UI_QuickSlot_Skill[] skillSlot = GameManager.Inst.m_quickSlot.m_skillSlot;
        saveData.skillQuickSlots = new List<string>();
        for(int i = 0; i < skillSlot.Length; i++)
        {
            skillSlot[i] = GameManager.Inst.m_quickSlot.m_skillSlot[i];
            if (skillSlot[i].m_skillData == null)
                saveData.skillQuickSlots.Add("");
            else
                saveData.skillQuickSlots.Add(skillSlot[i].m_skillData.name);
        }

        // 필살기 스킬 퀵 슬롯
        UI_QuickSlot_FinalSkill finalSkillSlot = GameManager.Inst.m_quickSlot.m_finalSkillSlot;
        if (finalSkillSlot.m_skillData != null)
            saveData.isThereFinalSkill = true;
        else
            saveData.isThereFinalSkill = false;

        // 아이템 퀵 슬롯
        UI_QuickSlot_Item[] itemSlot = GameManager.Inst.m_quickSlot.m_itemSlot;
        saveData.itemQuickSlots = new List<string>();
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = GameManager.Inst.m_quickSlot.m_itemSlot[i];
            if (itemSlot[i].m_itemData == null)
                saveData.itemQuickSlots.Add("");
            else
                saveData.itemQuickSlots.Add(itemSlot[i].m_itemData.name);
        }
    }
    void LoadQuickSlotData(SaveData saveData)
    {
        // 일반 스킬 퀵 슬롯
        UI_QuickSlot_Skill[] skillSlot = GameManager.Inst.m_quickSlot.m_skillSlot;
        for (int i = 0; i < skillSlot.Length; i++)
        {
            skillSlot[i] = GameManager.Inst.m_quickSlot.m_skillSlot[i];

            if (string.IsNullOrEmpty(saveData.skillQuickSlots[i]))
                continue;

            SkillData skill = Managers.Resource.Load<SkillData>($"Data/SkillData/{saveData.skillQuickSlots[i]}");

            if (skill != null)
                skillSlot[i].SetSkill(skill);
        }

        // 필살기 스킬 퀵 슬롯
        UI_QuickSlot_FinalSkill finalSkillSlot = GameManager.Inst.m_quickSlot.m_finalSkillSlot;
        SkillData finalSkill = Managers.Resource.Load<SkillData>($"Data/SkillData/Skill_FinalAttack");
        if (saveData.isThereFinalSkill == true)
            finalSkillSlot.SetSkill(finalSkill);
    }

    void SaveInventoryData()
    {
        // 슬롯을 순서대로 쫙 받은 다음에 해당 슬롯에 itemData가 있는지 검사
        // 있다면 아이템 데이타 + 수량 저장

        Inventory inven = GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>();
        saveData.invenSlots = new List<string>();
        saveData.invenItemType = new List<string>();
        saveData.invenItemCount = new List<int>();

        for (int i = 0; i < inven.m_slots.Length; i++)
        {
            if (inven.m_slots[i].m_itemData == null)
            {
                saveData.invenSlots.Add("");
                saveData.invenItemType.Add("");
                saveData.invenItemCount.Add(0);
                continue;
            }

            string itemName = inven.m_slots[i].m_itemData.ToString();

            int lastBracketIndex = itemName.LastIndexOf(" (");
            if (lastBracketIndex != -1)
                itemName = itemName.Substring(0, lastBracketIndex); 

            saveData.invenSlots.Add(itemName);
            saveData.invenItemType.Add(inven.m_slots[i].m_itemData.m_itemType.ToString());
            saveData.invenItemCount.Add(inven.m_slots[i].m_itemCount);
        }
    }
    void LoadInventoryData(SaveData saveData)
    {
        UI_QuickSlot_Item[] quickSlot = GameManager.Inst.m_quickSlot.m_itemSlot;

        Inventory inven = GameManager.Inst.m_popup.m_invenPopUp.GetComponent<Inventory>();
        int slotCount = Mathf.Min(inven.m_slots.Length, saveData.invenSlots.Count);

        for (int i = 0; i < slotCount; i++)
        {
            if (string.IsNullOrEmpty(saveData.invenSlots[i]))
                continue;

            ItemData data = Managers.Resource.Load<ItemData>($"Data/ItemData/{saveData.invenItemType[i]}/{saveData.invenSlots[i]}");

            if (data != null)
            {
                inven.m_slots[i].AddItem(data, saveData.invenItemCount[i]);
                GameManager.Inst.m_quest.CollectQuestProcess(data, saveData.invenItemCount[i]);
            }
        }

        for (int i = 0; i < slotCount; i++)
        {
            if (inven.m_slots[i].m_itemData == null)
                continue;

            for (int j = 0; j < quickSlot.Length; j++)
            {
                if (string.IsNullOrEmpty(saveData.itemQuickSlots[j]))
                    continue;

                if (string.Equals(saveData.itemQuickSlots[j], inven.m_slots[i].m_itemData.name))
                {
                    quickSlot[j].ChangeItem(inven.m_slots[i]);
                }
            }
        }
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}