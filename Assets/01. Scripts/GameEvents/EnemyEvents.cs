using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents
{
    public event Action onEnemyKilled;

    public void EnemyKilled()
    {
        if(onEnemyKilled != null)
        {
            onEnemyKilled();
        }
    }

    public event Action onEnemyCleared;

    public void ClearEnemy()
    {
        if (onEnemyCleared != null)
        {
            onEnemyCleared();
        }
    }

    public event Action<IEnemy> onEnemyDie;

    public void EnemyDie(IEnemy enemy)
    {
        if (onEnemyDie != null)
        {
            onEnemyDie(enemy);
        }
    }
}
