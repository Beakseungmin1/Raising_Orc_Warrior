using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenManager : Singleton<RegenManager>
{
    [SerializeField] private StageSO stageSO;
    [SerializeField] private EnemySO[] enemySOs;
    [SerializeField] private Transform[] enemyRegenPoss;

    [SerializeField] private Transform field;

    private void Start()
    {
        enemySOs = stageSO.enemySOs;
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

    //���� �� ó���ϰ� �¿�Ƽ������ϴ� �ż��� �ʿ�.
    private Enemy SetUnitObject(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        return enemy;
    }

}
