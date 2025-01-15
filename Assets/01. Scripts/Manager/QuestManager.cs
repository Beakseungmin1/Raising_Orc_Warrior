using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger ���
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    private Dictionary<string, Quest> questMap;

    private Dictionary<string, GameObject> questStepObjMap;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;

        GameEventsManager.Instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
    }

    private void Start()
    {
        questStepObjMap = CreateQuestStepObjMap();
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
            GameObject newStepObj = quest.InstatiateCurrentQuestStep(this.transform);
            // Transform�� �ڽĵ��� ��ȸ�ϸ鼭 quest.info.id�� �ش��ϴ� GameObject�� ã�� �߰�
            foreach (Transform child in this.transform)
            {
                if (child.name == quest.info.id + "Step(Clone)")
                {
                    // �̹� questStepObjMap�� ��ϵ� ���, �ߺ� üũ
                    if (!questStepObjMap.ContainsKey(quest.info.id))
                    {
                        questStepObjMap.Add(quest.info.id, child.gameObject);
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate entry detected for quest ID {quest.info.id} in transform children.");
                    }
                }
            }
            questStepObjMap.Add(quest.info.id, newStepObj);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    public void FinishQuest(string id, QuestType questType)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);

        GameEventsManager.Instance.questEvents.FinishQuestStep(id);
        GameEventsManager.Instance.questEvents.QuestProgressCountChange(id);

        if (questType == QuestType.Repeat)
        {
            //����Ʈ ������ ī��Ʈ�� �䱸ġ���� ������ ������ ���θ� �޾ƿ���
            QuestStep step = questStepObjMap[id].GetComponent<QuestStep>();
            
            if (step.count >= step.countToComplete)
            {
                GameEventsManager.Instance.questEvents.RestartQuestStep(id);
                ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
            }
            else
            {
                ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
            }
            GameEventsManager.Instance.questEvents.QuestProgressCountChange(id);

        }
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
                Debug.LogWarning("����Ʈ���� �����ϴ� �� �ߺ��� id�� ã�ҽ��ϴ�: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, LoadQuest(questInfo));
        }
        return idToQuestMap;
    }

    private Dictionary<string, GameObject> CreateQuestStepObjMap()
    {
        Dictionary<string, GameObject> idToQuestStepObjMap = new Dictionary<string, GameObject>();

        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                // ���� QuestStep�� GameObject�� ����
                GameObject questStepObj = quest.InstatiateCurrentQuestStep(this.transform);

                // Transform�� �ڽĵ��� ��ȸ�ϸ鼭 quest.info.id�� �ش��ϴ� GameObject�� ã�� �߰�
                foreach (Transform child in this.transform)
                {
                    if (child.name == quest.info.id + "Step(Clone)")
                    {
                        // �̹� questStepObjMap�� ��ϵ� ���, �ߺ� üũ
                        if (!idToQuestStepObjMap.ContainsKey(quest.info.id))
                        {
                            idToQuestStepObjMap.Add(quest.info.id, child.gameObject);
                        }
                        else
                        {
                            Debug.LogWarning($"Duplicate entry detected for quest ID {quest.info.id} in transform children.");
                        }
                    }
                }
            }
            // Quest ���� ���� �̺�Ʈ ȣ��
            GameEventsManager.Instance.questEvents.QuestStateChange(quest);
        }
        return idToQuestStepObjMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("����Ʈ�ʿ��� ID�� ã�� �� �����ϴ�: " + id);
        }
        return quest;
    }


    public GameObject GetQuestStepObjById(string id)
    {
        if (questStepObjMap[id] == null)
        {
            Debug.LogError("����Ʈ�ʿ��� ID�� ã�� �� �����ϴ�: " + id);
        }
        return questStepObjMap[id];
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

            //PlayerPrefs�� �����ϴ� ���� �� Ʃ�丮�� ������ ���� ������ ���Դϴ�.
            //�� ������ ��������� PlayerPrefs�� �����ϴ� ���� �������� ���� �� �ֽ��ϴ�.
            //���, ���� ���� �� �ҷ����� �ý����� ����Ͽ� ����, Ŭ���� � �����͸� �����ϼ���.
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