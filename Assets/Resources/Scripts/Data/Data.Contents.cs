using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    #region Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int maxMp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();

            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);

            return dict;
        }
    }
    #endregion

    #region Enemy
    [Serializable]
    public class Enemy
    {
        public int level;
        public int maxHp;
        public int attack;
        public float attackRate;
        public int moveSpeed;
        public float attackRange;
        public float respawnTime;
    }

    [Serializable]
    public class EnemyData : ILoader<int, Enemy>
    {
        public List<Enemy> enemys = new List<Enemy>();

        public Dictionary<int, Enemy> MakeDict()
        {
            Dictionary<int, Enemy> dict = new Dictionary<int, Enemy>();

            foreach (Enemy enemyStat in enemys)
                dict.Add(enemyStat.level, enemyStat);

            return dict;
        }
    }
    #endregion

    #region Quest
    [Serializable]
    public class Quest
    {
        public int QuestID;
        public string[] QuestCategory;
        public int[] Key;
        public string QuestName;
        public string[] QuestExplain;
        public string QuestStartNPC;
        public string QuestEndNPC;
        public int RewardExp;
        public int RewardGold;
        public bool Accepted;
        public bool Completed;
    }

    [Serializable]
    public class QuestData : ILoader<int, Quest>
    {
        public List<Quest> Quest = new List<Quest>();

        public Dictionary<int, Quest> MakeDict()
        {
            Dictionary<int, Quest> dict = new Dictionary<int, Quest>();

            foreach (Quest quest in Quest)
                dict.Add(quest.QuestID, quest);

            return dict;
        }
    }
    #endregion

    #region QuestDialogue
    [Serializable]
    public class QuestDialogue
    {
        public int DialogueIndex;
        public string Speaker;
        public string Dialogue;
        public int State;
    }

    [Serializable]
    public class QuestDialogueData : ILoader<int, QuestDialogue>
    {
        public List<QuestDialogue> QuestDialogue = new List<QuestDialogue>();

        public Dictionary<int, QuestDialogue> MakeDict()
        {
            Dictionary<int, QuestDialogue> dict = new Dictionary<int, QuestDialogue>();

            foreach (QuestDialogue dialogue in QuestDialogue)
                dict.Add(dialogue.DialogueIndex, dialogue);

            return dict;
        }
    }
    #endregion

    #region KillQuest
    [Serializable]
    public class KillQuest
    {
        public int Key;
        public string TargetName;
        public int Count;
        public int GoalCount;
        public bool Completed;
    }

    [Serializable]
    public class KillQuestData : ILoader<int, KillQuest>
    {
        public List<KillQuest> KillQuest = new List<KillQuest>();

        public Dictionary<int, KillQuest> MakeDict()
        {
            Dictionary<int, KillQuest> dict = new Dictionary<int, KillQuest>();

            foreach (KillQuest killQuest in KillQuest)
                dict.Add(killQuest.Key, killQuest);

            return dict;
        }
    }
    #endregion

    #region CollectQuest
    [Serializable]
    public class CollectQuest
    {
        public int Key;
        public string ItemName;
        public int Count;
        public int GoalCount;
        public bool Completed;
    }

    [Serializable]
    public class CollectQuestData : ILoader<int, CollectQuest>
    {
        public List<CollectQuest> CollectQuest = new List<CollectQuest>();

        public Dictionary<int, CollectQuest> MakeDict()
        {
            Dictionary<int, CollectQuest> dict = new Dictionary<int, CollectQuest>();

            foreach (CollectQuest collectQuest in CollectQuest)
                dict.Add(collectQuest.Key, collectQuest);

            return dict;
        }
    }
    #endregion
}