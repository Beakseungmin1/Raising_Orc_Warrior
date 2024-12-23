using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeaponQuestStep : QuestStep
{
    public QuestInfoSO questInfo;

    private void OnEnable()
    {
        GameEventsManager.Instance.summonEvents.onWeaponSummoned += WeaponSummoned;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.summonEvents.onWeaponSummoned -= WeaponSummoned;
    }

    public void WeaponSummoned(int count)
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
