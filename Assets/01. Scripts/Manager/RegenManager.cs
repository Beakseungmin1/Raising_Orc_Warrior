using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RegenManager : Singleton<RegenManager>
{
    private ChapterSO curChapterSO;
    [SerializeField] private EnemySO[] enemySOs;
    [SerializeField] private Transform[] enemyRegenPoss;

    [SerializeField] private Transform field;

    public int totalEnemies = 0; //�ش� �������� �� �� ����
    public int killedEnemies = 0; //���� �� ����
    //�׼����� ���� ���������� ���� �޾ƿͼ� �����Ѵ�.
    //0�� �Ǹ� ���������Ŵ����� ���� �����Ѵ�.
    
    public Action OnEnemyCountDown;
    public Action OnEnemyCountZero;

    private void Start()
    {
        //���������Ŵ����� é��SO�� �����Ѵ�.
        RegenStagesEnemy();
    }

    public void RegenStagesEnemy()
    {
        killedEnemies = 0; //�ʱ�ȭ
        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];
        enemySOs = curChapterSO.stageSOs[StageManager.Instance.curStageIndexInThisChapter].enemySOs;
        totalEnemies = curChapterSO.stageSOs[StageManager.Instance.curStageIndexInThisChapter].enemySOs.Length;

        for (int i = 0; i < enemySOs.Length; i++)
        {
            Transform enemyRegenPos = enemyRegenPoss[i];
            EnemySO enemySO = enemySOs[i];
            RegenEnemy(enemySO, enemyRegenPos);
        }
    }

    public void RegenEnemy(EnemySO enemySO, Transform enemyRegenPos)
    {
        GameObject obj = ObjectPool.Instance.GetObject("Enemy");
        Enemy enemy = SetUnitObject(obj);
        enemy.enemySO = enemySO;
        enemy.SetupEnemy();
        enemy.transform.position = enemyRegenPos.position;
        enemy.transform.SetParent(field, true);
    }

    //���� �� ó���ϰ� �¿�Ƽ������ϴ� �ż��� �ʿ�.
    private Enemy SetUnitObject(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        return enemy;
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
