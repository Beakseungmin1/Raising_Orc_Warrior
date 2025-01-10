using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBossBattle : MonoBehaviour
{
    public EnemyDungeonBoss enemy;
    public GameObject paticle;
    public Transform hand;

    public void Pattern1()
    {
        if (enemy != null)
        {
            Instantiate(paticle,hand);
            enemy.GiveKnockDamageToPlayer();
            SoundManager.Instance.PlaySFX(SFXType.Boss1);
        }
    }

    public void Pattern2()
    {
        if (enemy != null)
        {
            enemy.GiveKnockDamageToPlayer();
        }
    }

    public void Pattern3()
    {
        if (enemy != null)
        {
            enemy.GiveDamageToPlayer();
        }
    }





}
