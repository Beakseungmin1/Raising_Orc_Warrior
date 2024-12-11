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

    public int stagesEnemyCount; //�ش� �������� �� �� ����
    public int curEnemyCount; //�� ����
    //�׼����� ���� ���������� ���� �޾ƿͼ� �����Ѵ�.
    //0�� �Ǹ� ���������Ŵ����� ���� �����Ѵ�.
    
    public Action OnEnemyCountDown;
    public Action OnEnemyCountZero;

    private void Start()
    {
        //���������Ŵ����� é��SO�� �����Ѵ�.
        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];
        enemySOs = curChapterSO.stageSOs[StageManager.Instance.curStageIndex].enemySOs;
        stagesEnemyCount = enemySOs.Length;
        curEnemyCount = enemySOs.Length;
        RegenStagesEnemy();
    }

    private void RegenStagesEnemy()
    {
        for (int i = 0; i < stagesEnemyCount; i++)
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
        enemy.transform.position = enemyRegenPos.position;
        enemy.transform.SetParent(field, true);
    }

    //���� �� ó���ϰ� �¿�Ƽ������ϴ� �ż��� �ʿ�.
    private Enemy SetUnitObject(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        return enemy;
    }

    public void EnemyDeath()
    {
        curEnemyCount--;
        OnEnemyCountDown.Invoke();

        if(curEnemyCount <= 0)
        {
            curEnemyCount = 0;
            OnEnemyCountZero.Invoke();
        }
    }
    
}
