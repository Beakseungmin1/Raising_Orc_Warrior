using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RegenManager : Singleton<RegenManager>
{
    private ChapterSO curChapterSO;
    [SerializeField] private EnemySO[] enemySOs;

    public int totalEnemies = 0; // 해당 스테이지 적 총 개수
    public int killedEnemies = 0; // 죽인 적 개수

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
        killedEnemies = 0; // 초기화
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

        // 적 이동을 관리하는 컴포넌트 추가
        EnemyMover enemyMover = enemy.GetComponent<EnemyMover>();
        if (enemyMover == null)
        {
            enemyMover = enemy.gameObject.AddComponent<EnemyMover>();
        }
        enemyMover.SetMoveSpeed(2.0f); // 왼쪽으로 이동 속도 설정
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
