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

    public int curEnemyCount; //�� ����
    //�׼����� ���� ���������� ���� �޾ƿͼ� �����Ѵ�.
    //0�� �Ǹ� ���������Ŵ����� ���� �����Ѵ�.

    private void Start()
    {
        if (curChapterSO != null)
        {
            //���������Ŵ����� é��SO�� �����Ѵ�.
            curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterIndex];
            enemySOs = curChapterSO.stageSOs[StageManager.Instance.curStageIndex].enemySOs;
            curEnemyCount = enemySOs.Length;
            RegenStagesEnemy();
        }
    }

    private void RegenStagesEnemy()
    {
        for (int i = 0; i < curEnemyCount; i++)
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

    
}
