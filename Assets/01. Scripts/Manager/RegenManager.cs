using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenManager : Singleton<RegenManager>
{
    [SerializeField] private StageSO stageSO;
    [SerializeField] private EnemySO[] enemySOs;
    [SerializeField] private Transform[] enemyRegenPoss;
    
    //현재 맵에 등장하는 몹의 수(enemySOs.Length)만큼 for문을 돌려서 enemyRegenPoss에 할당해준다.
    

    private void Start()
    {
        enemySOs = stageSO.enemySOs;

        for (int i = 0; i < enemySOs.Length; i++)
        {
            Transform enemyRegenPos = enemyRegenPoss[i];
            EnemySO enemySO = enemySOs[i];
            RegenEnemy(enemySO,enemyRegenPos);
        }
    }

    public void RegenEnemy(EnemySO enemySO, Transform enemyRegenPos)
    {
        GameObject obj = ObjectPool.Instance.GetObject("Enemy");
        Enemy enemy = SetUnitObject(obj);
        enemy.enemySO = enemySO;
        enemy.transform.position = enemyRegenPos.position;
    }

    //정보 다 처리하고 셋엑티브오프하는 매서드 필요.

    private Enemy SetUnitObject(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        return enemy;
    }

}
