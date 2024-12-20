using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkillQuestStep : QuestStep
{
    private int summonCount = 0;
    private int summonCountToComplete = 10;

    private void OnEnable()
    {
        GameEventsManager.Instance.summonEvents.onSkillSummoned += SkillSummoned;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.summonEvents.onSkillSummoned -= SkillSummoned;
    }

    public void SkillSummoned(int count)
    {
        if (summonCount < summonCountToComplete)
        {
            summonCount += count;
            UpdateState();
        }

        if (summonCount >= summonCountToComplete)
        {
            FinishQuestStep();
        }   
    }

    private void UpdateState()
    {
        string state = summonCount.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        this.summonCount = System.Int32.Parse(state);
        UpdateState();
    }
}
