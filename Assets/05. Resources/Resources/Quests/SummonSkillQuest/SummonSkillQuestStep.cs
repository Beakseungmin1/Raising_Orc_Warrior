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
        }

        if (summonCount >= summonCountToComplete)
        {
            FinishQuestStep();
        }
        
    }
}
