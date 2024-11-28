using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour, IDamageable
{
    private PlayerDamageCalculator PlayerDamageCalculator;


    private float totalDamage; // 계산기에서 받아온 최종데미지
    private float attackSpeed; // 공격 속도 퍼센트 게이지로 만들어 딜레이 에서 빼줄예정
    private float attackDelay = 1f; // 공격 딜레이

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
    }

    private void Update()
    {
        totalDamage = PlayerDamageCalculator.GetTotalDamage();
    }


    public void TakeDamage(float Damage)
    {

    }

    IEnumerator PlayerAttack(EnemyBattle monster)
    {
        while (monster != null && monster.GetActive())
        {
            // 공격 애니메이션 재생예정
            // animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackDelay);

            monster.TakeDamage(totalDamage);

            if (!monster.GetActive())
            {
                break;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            BattleManager.Instance.StartBattle();
            EnemyBattle monster = collision.gameObject.GetComponent<EnemyBattle>();
            StartCoroutine(PlayerAttack(monster));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            BattleManager.Instance.EndBattle();
            EnemyBattle monster = collision.gameObject.GetComponent<EnemyBattle>();
            StopCoroutine(PlayerAttack(monster));
        }
    }

}
