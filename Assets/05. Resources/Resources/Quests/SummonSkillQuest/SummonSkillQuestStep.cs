using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkillQuestStep : QuestStep
{
    public QuestInfoSO questInfo;

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
        if (this.count < countToComplete)
        {
            this.count += count;
            UpdateState();
        }

        if (this.count >= countToComplete)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = count.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        this.count = System.Int32.Parse(state);
        UpdateState();
    }
}
