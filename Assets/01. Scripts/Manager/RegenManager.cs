using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Collections;

public class RegenManager : Singleton<RegenManager>
{
    private ChapterSO curChapterSO;
    [SerializeField] private List<EnemySO> enemySOs;
    [SerializeField] private EnemySO bossEnemy;

    public int totalEnemies = 0; // �ش� �������� �� �� ����
    public int killedEnemies = 0; // ���� �� ����
    private int totalEnemiesForDebug = 0; //���� �� ���� �������� �� �� ������ �������� ���׸� üũ�ϱ� ���� ����
    private float scrollSpeed;

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

    public void CacheEnemies()
    {
        cachedEnemies = new List<(GameObject, EnemyMover)>();

        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];

        enemySOs = new List<EnemySO>(curChapterSO.stageSOs[StageManager.Instance.curStageIndexInThisChapter].enemySOs); //�ʱ�ȭ
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

        totalEnemies = enemySOs.Count;
        totalEnemiesForDebug += totalEnemies;

        scrollSpeed = BackgroundManager.Instance.ParallaxBackground.scrollSpeed;
        //scrollSpeed = ParallaxBackground.Instance.GetScrollSpeed();
    }

    public void CacheDungeonBoss(Dungeon dungeon)
    {
        enemySOs = new List<EnemySO>();
        bossEnemy = dungeon.info.dungeonBoss;

        GameObject obj = ObjectPool.Instance.GetObject("DungeonBoss");
        EnemyMover enemyMover = obj.GetComponent<EnemyMover>();

        if (enemyMover == null)
        {
            enemyMover = obj.AddComponent<EnemyMover>();
        }

        cachedEnemies = new List<(GameObject, EnemyMover)>();
        obj.SetActive(false); // ĳ�� �߿��� ��Ȱ��ȭ
        cachedEnemies.Add((obj, enemyMover));
    }

    public void CacheEnemyBoss()
    {
        enemySOs = new List<EnemySO>();
        bossEnemy = curChapterSO.bossStageSOs[StageManager.Instance.curStageIndexInThisChapter].bossEnemySO;

        GameObject obj = ObjectPool.Instance.GetObject("EnemyBoss");
        EnemyMover enemyMover = obj.GetComponent<EnemyMover>();

        if (enemyMover == null)
        {
            enemyMover = obj.AddComponent<EnemyMover>();
        }

        cachedEnemies = new List<(GameObject, EnemyMover)>();
        obj.SetActive(false); // ĳ�� �߿��� ��Ȱ��ȭ
        cachedEnemies.Add((obj, enemyMover));

    }

    public void RegenStagesEnemy()
    {
        if (totalEnemiesForDebug != 0)
        {
            GameEventsManager.Instance.enemyEvents.ClearEnemy();
        }

        killedEnemies = 0;

        for (int i = 0; i < cachedEnemies.Count; i++)
        {
            EnemySO enemySO = enemySOs[i];
            Vector3 spawnPosition = transform.position + new Vector3(i * spawnDistance, 0, 0);
            RegenEnemy(enemySO, spawnPosition, cachedEnemies[i]);
        }
    }

    public void RegenStagesBossEnemy()
    {
        killedEnemies = 0;

        EnemySO bossEnemySO = bossEnemy;
        Vector3 spawnPosition = transform.position + new Vector3(0 * spawnDistance, 0, 0);
        RegenBossEnemy(bossEnemySO, spawnPosition, cachedEnemies[0]);
    }

    public void RegenStagesEnemyDungeonBoss(DungeonInfoSO dungeonInfo)
    {
        killedEnemies = 0;

        EnemySO bossEnemySO = bossEnemy;
        Vector3 spawnPosition = transform.position + new Vector3(0 * spawnDistance, 1, 0);
        RegenEnemyDungeonBoss(bossEnemySO, spawnPosition, cachedEnemies[0], dungeonInfo);
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

        enemyMover.SetMoveSpeed(scrollSpeed);
    }

    public void RegenBossEnemy(EnemySO enemySO, Vector3 spawnPosition, (GameObject enemyObject, EnemyMover enemyMover) cachedEnemy)
    {
        GameObject enemyObject = cachedEnemy.enemyObject;
        EnemyMover enemyMover = cachedEnemy.enemyMover;

        EnemyBoss enemyBoss = SetUnitObjectAsEnemyBoss(enemyObject);
        enemyBoss.enemySO = enemySO;
        enemyBoss.SetupEnemy();
        enemyBoss.transform.position = spawnPosition;
        enemyObject.SetActive(true);

        enemyMover.SetMoveSpeed(scrollSpeed);
    }

    public void RegenEnemyDungeonBoss(EnemySO enemySO, Vector3 spawnPosition, (GameObject enemyObject, EnemyMover enemyMover) cachedEnemy, DungeonInfoSO dungeonInfo)
    {
        GameObject enemyObject = cachedEnemy.enemyObject;
        EnemyMover enemyMover = cachedEnemy.enemyMover;

        EnemyDungeonBoss enemyBoss = SetUnitObjectAsEnemyDungeonBoss(enemyObject);
        enemyBoss.enemySO = enemySO;
        enemyBoss.dungeonInfo = dungeonInfo;
        enemyBoss.SetupEnemy();
        enemyBoss.transform.position = spawnPosition;
        enemyObject.SetActive(true);

        enemyMover.SetMoveSpeed(scrollSpeed);
    }

    private Enemy SetUnitObject(GameObject obj)
    {
        return obj.GetComponent<Enemy>();
    }

    private EnemyBoss SetUnitObjectAsEnemyBoss(GameObject obj)
    {
        return obj.GetComponent<EnemyBoss>();
    }

    private EnemyDungeonBoss SetUnitObjectAsEnemyDungeonBoss(GameObject obj)
    {
        return obj.GetComponent<EnemyDungeonBoss>();
    }

    public void EnemyKilled()
    {
        killedEnemies++;
        totalEnemiesForDebug -= killedEnemies;
        OnEnemyCountDown?.Invoke();

        if (killedEnemies >= totalEnemies)
        {
            StageManager.Instance.StageClear();
        }
    }
}