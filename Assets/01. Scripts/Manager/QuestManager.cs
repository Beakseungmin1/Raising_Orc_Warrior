using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger 사용
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, Quest> questMap;

    public List<GameObject> questGameObjs = new List<GameObject>();

    private GameObject questStepObj;

    private Dictionary<string, GameObject> questStepObjMap = new Dictionary<string, GameObject>();

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;
        GameEventsManager.Instance.questEvents.onReStartQuest += RestartQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;
        GameEventsManager.Instance.questEvents.onReStartQuest -= RestartQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.IN_PROGRESS)
            {
                questStepObj = quest.InstatiateCurrentQuestStep(this.transform);
                Debug.Log(questStepObj);
                questStepObjMap.Add(quest.info.id, questStepObj);

                foreach (Transform child in this.transform)
                {
                    if (child.name == quest.info.id + "Step(Clone)")
                    {
                        questGameObjs.Add(child.gameObject);
                    }
                }
            }

            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetRequirements = true;

        // check quest prerequisites for completion
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetRequirements = false;
                // add this break statement here so that we don't continue on to the next quest, since we've proven meetsRequirements to be false at this point.
                break;
            }
        }

        return meetRequirements;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        //quest.InstatiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstatiateCurrentQuestStep(this.transform);
            questStepObjMap.Add(quest.info.id, questStepObj);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        GameEventsManager.Instance.questEvents.QuestProgressCountChange(id);
    }

    private void RestartQuest(string id)
    {
        Quest quest = GetQuestById(id);

        //퀘스트 스텝 프리팹 처리
        QuestStep step = GetQuestStepObjById(id).GetComponent<QuestStep>();
        GameEventsManager.Instance.questEvents.FinishQuestStep(id);

        GameEventsManager.Instance.questEvents.RestartQuestStep(id);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        GameEventsManager.Instance.questEvents.QuestProgressCountChange(id);
    }

    private void ClaimRewards(Quest quest)
    {
        CurrencyManager.Instance.AddCurrency(quest.info.currenyType, quest.info.rewardAmount);
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        Quest quest = GetQuestById(id);
        quest.StoreQuestStepState(questStepState, stepIndex);
        ChangeQuestState(id, quest.state);
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.Instance.questEvents.QuestStateChange(quest);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuest = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuest)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("퀘스트맵을 생성하던 중 중복된 id를 찾았습니다: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }
        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("퀘스트맵에서 ID를 찾을 수 없습니다: " + id);
        }
        return quest;
    }

    public GameObject GetQuestStepObjById(string id)
    {
        GameObject questStepObj = questStepObjMap[id];
        if (questStepObj == null)
        {
            Debug.LogError("퀘스트맵에서 ID를 찾을 수 없습니다: " + id);
        }
        return questStepObj;
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest in questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();

            // seiralize using JsonUtility, but use whatever you want here (like JSON.NET)
            string serializedData = JsonUtility.ToJson(questData);

            //PlayerPrefs에 저장하는 것은 이 튜토리얼 비디오의 빠른 예제일 뿐입니다.
            //이 정보를 장기적으로 PlayerPrefs에 저장하는 것은 적합하지 않을 수 있습니다.
            //대신, 실제 저장 및 불러오기 시스템을 사용하여 파일, 클라우드 등에 데이터를 저장하세요.
            PlayerPrefs.SetString(quest.info.id, serializedData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save quest with id: " + quest.info.id + ": " + e);
        }
    }

    private Quest LoadQuest(QuestInfoSO questInfo)
    {
        Quest quest = null;
        try
        {
            // Load quest from saved data
            if (PlayerPrefs.HasKey(questInfo.id) && loadQuestState)
            {
                string serializedData = PlayerPrefs.GetString(questInfo.id);
                QuestData questData = JsonUtility.FromJson<QuestData>(serializedData);
                quest = new Quest(questInfo, questData.state, questData.questStepIndex, questData.questStepStates);
            }
            // Otherwise, initialize a new quest
            else
            {
                quest = new Quest(questInfo);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load quest with id " + questInfo.id + ": " + e);
        }
        return quest;
    }

}