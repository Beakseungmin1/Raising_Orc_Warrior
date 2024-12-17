using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public event Action onEnemyKilled;

    public void EnemyKilled()
    {
        if(onEnemyKilled != null)
        {
            onEnemyKilled();
        }
    }
}
