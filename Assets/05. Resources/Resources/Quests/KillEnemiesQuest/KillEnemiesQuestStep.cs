using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesQuestStep : QuestStep
{
    public QuestInfoSO questInfo;

    private void OnEnable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyKilled -= EnemyKilled;
    }

    public void EnemyKilled()
    {
        this.count++;

        if (this.count < countToComplete)
        {
            UpdateState();

            GameEventsManager.Instance.questEvents.QuestProgressCountChange(questId);
        }

        if (this.count >= countToComplete)
        {
            FinishQuestStep();
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
