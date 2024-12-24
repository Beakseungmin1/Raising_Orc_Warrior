using System.Collections.Generic;
using UnityEngine;
using System;

public class RegenManager : Singleton<RegenManager>
{
    private ChapterSO curChapterSO;
    [SerializeField] private EnemySO[] enemySOs;
    [SerializeField] private EnemySO bossEnemySO;

    public int totalEnemies = 0; // 해당 스테이지 적 총 개수
    public int killedEnemies = 0; // 죽인 적 개수

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

        // 적 오브젝트를 미리 캐싱
        foreach (var enemySO in enemySOs)
        {
            GameObject obj = ObjectPool.Instance.GetObject("Enemy");
            EnemyMover enemyMover = obj.GetComponent<EnemyMover>();

            if (enemyMover == null)
            {
                enemyMover = obj.AddComponent<EnemyMover>();
            }

            obj.SetActive(false); // 캐싱 중에는 비활성화
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

    public void RegenBossStagesEnemy()
    {
        killedEnemies = 0; //초기화
        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];
        bossEnemySO = curChapterSO.bossStageSO.bossEnemySO;
        totalEnemies = 1;

        EnemySO enemySO = bossEnemySO;
        Vector3 spawnPosition = transform.position + new Vector3(0 * spawnDistance, 0, 0);
        RegenEnemy(enemySO, spawnPosition, cachedEnemies[0]);
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

    public void ClearEnemies()
    {
        GameEventsManager.Instance.enemyEvents.ClearEnemy();
    }
}