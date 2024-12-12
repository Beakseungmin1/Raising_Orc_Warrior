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

    public int totalEnemies = 0; //해당 스테이지 적 총 개수
    public int killedEnemies = 0; //죽인 적 개수
    //액션으로 몬스터 죽을때마다 값을 받아와서 차감한다.
    //0이 되면 스테이지매니저에 정보 전달한다.
    
    public Action OnEnemyCountDown;
    public Action OnEnemyCountZero;

    private void Start()
    {
        //스테이지매니저의 챕터SO를 참조한다.
        RegenStagesEnemy();
    }

    public void RegenStagesEnemy()
    {
        killedEnemies = 0; //초기화
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

    //정보 다 처리하고 셋엑티브오프하는 매서드 필요.
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
