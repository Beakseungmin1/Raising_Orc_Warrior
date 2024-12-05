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
        // 피격 애니메이션 재생 추가예정

        if (Hp - Damage > 0)
        {
            Hp -= Damage;
            Debug.Log($"피해를 받음: {Damage}. 현재 HP: {Hp}");
        }
        else
        {
            Hp -= Damage;
            Die();
        }
    }

    public void Die()
    {
        // 애니메이션 재생추가 예정
        gameObject.SetActive(false);
    }

    public bool GetActive()
    {
        return gameObject.activeInHierarchy;
    }


}
