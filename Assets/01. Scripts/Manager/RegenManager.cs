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

    private List<(GameObject enemyObject, EnemyMover enemyMover)> cachedEnemies = new List<(GameObject, EnemyMover)>();

    private void Start()
    {
        CacheEnemies();
        RegenStagesEnemy();
        GameEventsManager.Instance.enemyEvents.onEnemyKilled += EnemyKilled;
    }

    private void CacheEnemies()
    {
        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];
        enemySOs = curChapterSO.stageSOs[StageManager.Instance.curStageIndexInThisChapter].enemySOs;

        // �� ������Ʈ�� �̸� ĳ��
        foreach (var enemySO in enemySOs)
        {
            GameObject obj = ObjectPool.Instance.GetObject("Enemy");
            EnemyMover enemyMover = obj.GetComponent<EnemyMover>();

            if (enemyMover == null)
            {
                enemyMover = obj.AddComponent<EnemyMover>();
            }

            obj.SetActive(false); // ĳ�� �߿��� ��Ȱ��ȭ
            cachedEnemies.Add((obj, enemyMover));
        }

        totalEnemies = enemySOs.Length;
    }

    public void RegenStagesEnemy()
    {
        killedEnemies = 0;

        for (int i = 0; i < cachedEnemies.Count; i++)
        {
            EnemySO enemySO = enemySOs[i];
            Vector3 spawnPosition = transform.position + new Vector3(i * spawnDistance, 0, 0);
            RegenEnemy(enemySO, spawnPosition, cachedEnemies[i]);
        }
    }

    public void RegenEnemy(EnemySO enemySO, Vector3 spawnPosition, (GameObject enemyObject, EnemyMover enemyMover) cachedEnemy)
    {
        GameObject enemyObject = cachedEnemy.enemyObject;
        EnemyMover enemyMover = cachedEnemy.enemyMover;

        Enemy enemy = SetUnitObject(enemyObject);
        enemy.enemySO = enemySO;
        enemy.SetupEnemy();
        enemy.transform.position = spawnPosition;
        enemyObject.SetActive(true);

        enemyMover.SetMoveSpeed(2.0f);
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