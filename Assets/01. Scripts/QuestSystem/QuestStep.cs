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

    private void Awake()
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
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

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true; 
            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            //Destroy(this.gameObject);
        }
    }
    
    protected void ChangeState(string newState)
    {
        GameEventsManager.Instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState));
    }

    protected abstract void SetQuestStepState(string state);
}
