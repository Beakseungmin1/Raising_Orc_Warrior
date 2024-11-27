using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattle : MonoBehaviour// IDamageable
{
    public float Hp;
    public float MaxHp = 100;

    public PlayerBattle Player;

    private void Start()
    {
        Hp = MaxHp;
    }


    public void TakeDamage(float Damage)
    {

        if (Hp - Damage > 0)
        {
            Hp -= Damage;
            Debug.Log($"���ظ� ����: {Damage}. ���� HP: {Hp}");
        }
        else
        {
            Hp -= Damage;
            Die();
        }
    }

    public void Die()
    {
        // �ִϸ��̼� ����߰� ����
        gameObject.SetActive(false);
    }

    public bool GetActive()
    {
        return gameObject.activeInHierarchy;
    }


}
