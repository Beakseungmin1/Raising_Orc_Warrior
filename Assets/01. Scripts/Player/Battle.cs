using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public PlayerBattle player;

    public EnemyBoss enemy;

    public Image healthBar;

    public void SetEnemyScript(EnemyBoss enemyboss)
    {
        enemy = enemyboss;
    }

    public void SetHealthBar()
    {
    }

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
