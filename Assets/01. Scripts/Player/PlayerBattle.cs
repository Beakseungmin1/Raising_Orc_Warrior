using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour, IDamageable
{
    public PlayerDamageCalculator PlayerDamageCalculator;

    private float totalDamage; // ���⿡�� �޾ƿ� ����������
    private float attackSpeed; // ���� �ӵ� �ۼ�Ʈ �������� ����� ������ ���� ���ٿ���
    private float attackDelay = 1f; // ���� ������
    private bool run = true;

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
    }

    private void Update()
    {
        if (run)
        {
            transform.position += Vector3.right * Time.deltaTime;
        }
        totalDamage = PlayerDamageCalculator.GetTotalDamage();
    }


    public void TakeDamage()
    {

    }

    IEnumerator PlayerAttack(MonsterBattle monster)
    {
        while (monster != null && monster.GetActive())
        {
            // ���� �ִϸ��̼� �������
            // animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackDelay);

            monster.TakeDamage(totalDamage);

            if (!monster.GetActive())
            {
                run = true;
                break;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            run = false;
            MonsterBattle monster = collision.gameObject.GetComponent<MonsterBattle>();
            StartCoroutine(PlayerAttack(monster));
        }
    }

}
