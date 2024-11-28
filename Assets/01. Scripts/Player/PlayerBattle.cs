using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour, IDamageable
{
    private PlayerDamageCalculator PlayerDamageCalculator;


    private float totalDamage; // ���⿡�� �޾ƿ� ����������
    private float attackSpeed; // ���� �ӵ� �ۼ�Ʈ �������� ����� ������ ���� ���ٿ���
    private float attackDelay = 1f; // ���� ������

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
            // ���� �ִϸ��̼� �������
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
