using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Battle : MonoBehaviour
{
    public PlayerBattle player;

    public EnemyBoss enemy;


    public void TriggerAttack()
    {
        if (player != null && player.OnPlayerAttack != null)
        {
            player.OnPlayerAttack.Invoke();
        }

        if (enemy != null && enemy.OnEnemyAttack != null)
        {
            enemy.OnEnemyAttack.Invoke();
        }
    }

}
