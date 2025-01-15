using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    public string questId;
    private int stepIndex;

    public int level = 1;
    public int count = 0;
    public int countToComplete = 10;

    protected virtual void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onFinishQuestStep += FinishQuestStep;
        GameEventsManager.Instance.questEvents.onRestartQuestStep += RestartQuestStep;
    }

    protected virtual void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onFinishQuestStep -= FinishQuestStep;
        GameEventsManager.Instance.questEvents.onRestartQuestStep -= RestartQuestStep;
    }

    public void InitializeQuestStep(string questId, int stepIndex, string questStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
    }

    protected void CanFinishQuestStep()
    {
        GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
    }

    protected void FinishQuestStep(string id)
    {
        if (!isFinished && this.questId == id)
        {
            isFinished = true;
            count -= countToComplete;
            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            GameEventsManager.Instance.questEvents.QuestProgressCountChange(questId);
        }
    }

    protected void RestartQuestStep(string id)
    {
        if (isFinished && this.questId == id)
        {
            isFinished = false;
            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            GameEventsManager.Instance.questEvents.QuestProgressCountChange(questId);
        }
    }

    protected void ChangeState(string newState)
    {
        GameEventsManager.Instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetQuestStepState(string state);
}
