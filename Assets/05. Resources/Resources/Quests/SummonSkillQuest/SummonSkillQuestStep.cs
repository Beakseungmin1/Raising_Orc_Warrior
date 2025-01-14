using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkillQuestStep : QuestStep
{
    public QuestInfoSO questInfo;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEventsManager.Instance.summonEvents.onSkillSummoned += SkillSummoned;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEventsManager.Instance.summonEvents.onSkillSummoned -= SkillSummoned;
    }

    public void SkillSummoned(int count)
    {
        this.count += count;

        if (this.count < countToComplete)
        {
            UpdateState();
            GameEventsManager.Instance.questEvents.QuestProgressCountChange(questId);
        }

        if (this.count >= countToComplete)
        {
            CanFinishQuestStep();
            GameEventsManager.Instance.questEvents.QuestProgressCountChange(questId);
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
