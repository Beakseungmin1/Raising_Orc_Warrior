using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger »ç¿ë
using UnityEngine;

public class DailyQuestManager : Singleton<DailyQuestManager>
{

    private const string LastQuestResetKey = "LastQuestResetDate";
    private DateTime nextResetTime;

    [SerializeField] private QuestInfoSO[] questInfos;

    private string[] questIds;
    private QuestState[] currentQuestStates;

    private void Awake()
    {
        questIds = new string[questInfos.Length];
        currentQuestStates = new QuestState[questIds.Length];

        for (int i = 0; i < questInfos.Length; i++)
        {
            questIds[i] = questInfos[i].id;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void StartDailyQuest()
    {
        for (int i = 0; i < questInfos.Length; i++)
        {
            if (currentQuestStates[i].Equals(QuestState.CAN_START))
            {
                GameEventsManager.Instance.questEvents.StartQuest(questIds[i]);
            }
        }
    }

    public void FinishQuest(string id)
    {
        for (int i = 0; i < questInfos.Length; i++)
        {
            if (questIds[i] == id && currentQuestStates[i].Equals(QuestState.CAN_FINISH))
            {
                GameEventsManager.Instance.questEvents.FinishQuest(questIds[i]);
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        for (int i = 0; i < questIds.Length; i++)
        {
            if (quest.info.id.Equals(questIds[i]))
            {
                currentQuestStates[i] = quest.state;
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < questInfos.Length; i++)
        {
            currentQuestStates[i] = QuestState.CAN_START;
        }
        StartDailyQuest();
        CalculateNextResetTime();
        CheckAndResetQuests();
    }

    private void Update()
    {
        if (DateTime.Now >= nextResetTime)
        {
            ResetDailyQuests();
            CalculateNextResetTime();
        }
    }

    private void CalculateNextResetTime()
    {
        DateTime now = DateTime.Now;
        nextResetTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);
        Debug.Log($"Next reset time calculated: {nextResetTime}");
    }

    private void CheckAndResetQuests()
    {
        if (!PlayerPrefs.HasKey(LastQuestResetKey))
        {
            // First-time setup: Reset quests immediately
            ResetDailyQuests();
            return;
        }

        string lastResetDateStr = PlayerPrefs.GetString(LastQuestResetKey);
        DateTime lastResetDate = DateTime.Parse(lastResetDateStr);

        if (lastResetDate.Date < DateTime.Now.Date)
        {
            ResetDailyQuests();
        }
    }

    private void ResetDailyQuests()
    {
        for (int i = 0; i < questInfos.Length; i++)
        {
            currentQuestStates[i] = QuestState.CAN_START;
        }
        StartDailyQuest();

        PlayerPrefs.SetString(LastQuestResetKey, DateTime.Now.Date.ToString());
        PlayerPrefs.Save();
    }
}