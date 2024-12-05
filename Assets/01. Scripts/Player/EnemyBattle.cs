using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemySO data; 

    public float Hp;
    public float MaxHp;
    public float giveExp;

    public PlayerBattle Player;

    private void Start()
    {
        Hp = data.hp;
        MaxHp = data.maxHp;
        giveExp = 20;
    }


    public void TakeDamage(float Damage)
    {
        // �ǰ� �ִϸ��̼� ��� �߰�����

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
