using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemiesQuestStep : QuestStep
{
    private int enemiesKilled = 0;
    private int killsToComplete = 3;

    private void OnEnable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyKilled += EnemyKilled;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyKilled -= EnemyKilled;
    }

    private void EnemyKilled()
    {
        if (enemiesKilled < killsToComplete)
        {
            enemiesKilled++;
            UpdateState();
            Debug.Log($"���� ���ͼ�: {enemiesKilled}");
        }

        if (enemiesKilled >= killsToComplete)
        {
            Debug.Log("����Ʈ �Ϸ�");
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = enemiesKilled.ToString();
        ChangeState(state);
    }
}
