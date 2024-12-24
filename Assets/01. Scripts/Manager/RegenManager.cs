using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RegenManager : Singleton<RegenManager>
{
    private ChapterSO curChapterSO;
    [SerializeField] private EnemySO[] enemySOs;

    public int totalEnemies = 0; // �ش� �������� �� �� ����
    public int killedEnemies = 0; // ���� �� ����

    public Action OnEnemyCountDown;
    public Action OnEnemyCountZero;

    [SerializeField] private float spawnDistance = 1.5f;

    private void Start()
    {
        RegenStagesEnemy();
        GameEventsManager.Instance.enemyEvents.onEnemyKilled += EnemyKilled;
    }

    public void RegenStagesEnemy()
    {
        killedEnemies = 0; // �ʱ�ȭ
        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];
        enemySOs = curChapterSO.stageSOs[StageManager.Instance.curStageIndexInThisChapter].enemySOs;
        totalEnemies = curChapterSO.stageSOs[StageManager.Instance.curStageIndexInThisChapter].enemySOs.Length;

        for (int i = 0; i < enemySOs.Length; i++)
        {
            EnemySO enemySO = enemySOs[i];
            Vector3 spawnPosition = transform.position + new Vector3(i * spawnDistance, 0, 0);
            RegenEnemy(enemySO, spawnPosition);
        }
    }

    public void RegenEnemy(EnemySO enemySO, Vector3 spawnPosition)
    {
        GameObject obj = ObjectPool.Instance.GetObject("Enemy");
        Enemy enemy = SetUnitObject(obj);
        enemy.enemySO = enemySO;
        enemy.SetupEnemy();
        enemy.transform.position = spawnPosition;

        // �� �̵��� �����ϴ� ������Ʈ �߰�
        EnemyMover enemyMover = enemy.GetComponent<EnemyMover>();
        if (enemyMover == null)
        {
            enemyMover = enemy.gameObject.AddComponent<EnemyMover>();
        }
        enemyMover.SetMoveSpeed(2.0f); // �������� �̵� �ӵ� ����
    }

    private Enemy SetUnitObject(GameObject obj)
    {
        return obj.GetComponent<Enemy>();
    }

    public void EnemyKilled()
    {
        killedEnemies++;
        OnEnemyCountDown?.Invoke();

        if (killedEnemies >= totalEnemies)
        {
            StageManager.Instance.StageClear();
        }
    }
}
