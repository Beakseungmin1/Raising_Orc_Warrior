using UnityEngine;
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

    public void TriggerSkill()
    {
        if (player != null && player.OnPlayerSkill != null)
        {
            player.OnPlayerSkill.Invoke();
        }

        if (enemy != null && enemy.OnEnemySkill != null)
        {
            enemy.OnEnemySkill.Invoke();
        }
    }
}