using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Battle : MonoBehaviour
{
    public PlayerBattle player;

    public Enemy enemy;


    public void TriggerAttack()
    {
        if (player != null)
        {
            player.OnPlayerAttack.Invoke();
        }
        else if (enemy != null)
        {
            enemy.OnEnemyAttack.Invoke();
        }
        else
        {
            return;
        }
    }



}
