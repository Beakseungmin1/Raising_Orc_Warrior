using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;

    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;
    public QuestType questType;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.IN_PROGRESS;
        this.currentQuestStepIndex = 0;
        this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        this.questType = questInfo.questType;

        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO info, QuestState questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        this.info = info;
        this.state = questState;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStates = questStepStates;

        if (this.questStepStates.Length != this.info.questStepPrefabs.Length)
        {
            Debug.LogWarning("퀘스트 단계 프리팹(Quest Step Prefabs)과 퀘스트 단계 상태(Quest Step States)의 길이가 다릅니다. "
            + "이는 퀘스트 정보(QuestInfo)와 저장된 데이터가 동기화되지 않았음을 나타냅니다. "
            + "데이터를 재설정하세요 - 그렇지 않으면 문제가 발생할 수 있습니다. 퀘스트 ID: " + this.info.id);
        }
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);

    }

    public GameObject InstatiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);

            questStepPrefab.name = info.id + "Step";
        }
        return questStepPrefab;
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if(CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("퀘스트스텝 프리팹을 가져오려했으나, 스텝Index가 범위를 벗어났습니다."
                + "there's no current step : QuestId=" + info.id + ", stepIndex=" + currentQuestStepIndex);
        }
        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
        }
        else
        {
            Debug.LogWarning("퀘스트 스텝 데이터에 접근하려고 했으나 스텝인덱스가 범위를 벗어났습니다: "
               + "Quest Id = " + info.id + ", Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }
}
