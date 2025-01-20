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
            Debug.LogWarning("����Ʈ �ܰ� ������(Quest Step Prefabs)�� ����Ʈ �ܰ� ����(Quest Step States)�� ���̰� �ٸ��ϴ�. "
            + "�̴� ����Ʈ ����(QuestInfo)�� ����� �����Ͱ� ����ȭ���� �ʾ����� ��Ÿ���ϴ�. "
            + "�����͸� �缳���ϼ��� - �׷��� ������ ������ �߻��� �� �ֽ��ϴ�. ����Ʈ ID: " + this.info.id);
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
            Debug.LogWarning("����Ʈ���� �������� ��������������, ����Index�� ������ ������ϴ�."
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
            Debug.LogWarning("����Ʈ ���� �����Ϳ� �����Ϸ��� ������ �����ε����� ������ ������ϴ�: "
               + "Quest Id = " + info.id + ", Step Index = " + stepIndex);
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }
}
