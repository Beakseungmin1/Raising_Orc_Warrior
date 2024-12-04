using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenManager : Singleton<RegenManager>
{
    private ChapterSO curChapterSO;
    [SerializeField] private EnemySO[] enemySOs;
    [SerializeField] private Transform[] enemyRegenPoss;

    [SerializeField] private Transform field;


    private void Start()
    {
        //스테이지매니저의 챕터SO를 참조한다.
        curChapterSO = StageManager.Instance.chapterSOs[StageManager.Instance.curChapterNum];

        enemySOs = curChapterSO.stageSOs[StageManager.Instance.curStageNum].enemySOs;
        RegenStagesEnemy();
    }

    private void RegenStagesEnemy()
    {
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
        enemy.transform.position = enemyRegenPos.position;
        enemy.transform.SetParent(field, true);
    }

    //정보 다 처리하고 셋엑티브오프하는 매서드 필요.
    private Enemy SetUnitObject(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        return enemy;
    }

}
