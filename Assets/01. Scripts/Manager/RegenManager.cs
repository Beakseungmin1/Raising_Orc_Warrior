using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenManager : Singleton<RegenManager>
{
    public Transform enemyRegenPos;
    public EnemySO enemySO;
    public GameObject enemyPrefab;

    private void Start()
    {
        //SpawnEnemy();
    }

    public void SpawnEnemy(EnemySO enemySO, Transform spawnPos)
    {
        GameObject obj = ObjectPool.Instance.GetObject("Enemy");
        Enemy enemy = SetUnitObject(obj);
        enemy.transform.position = spawnPos.position;
    }

    private Enemy SetUnitObject(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        return enemy;
    }

}
