using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour, IDamageable
{
    private enum State
    {
        Idle,
        Attacking,
        Dead
    }

    private State currentState;
    private PlayerDamageCalculator PlayerDamageCalculator;
    private PlayerStat playerStat;

    private float totalDamage; // ���⿡�� �޾ƿ� ����������
    private float attackSpeed; // ���� �ӵ� �ۼ�Ʈ �������� ����� ������ ���� ���ٿ���
    private float attackDelay = 1f; // ���� ������
    public List<Skill> activeBuffSkills = new List<Skill>(); // ���� Ȱ��ȭ�� ���� ��ų ����Ʈ

    private Enemy currentMonster; // ���� ���� ���� ����

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
        playerStat = GetComponent<PlayerStat>();
        currentState = State.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                CancelInvoke("PlayerAttack");
                BattleManager.Instance.EndBattle();
                break;
            case State.Attacking:
                if (currentMonster != null && !IsInvoking("PlayerAttack"))
                {
                    InvokeRepeating("PlayerAttack", 0f, attackDelay);
                    BattleManager.Instance.StartBattle();
                }
                break;
            case State.Dead:
                BattleManager.Instance.StartBattle();
                break;
        }
    }


    public void TakeDamage(float damage)
    {
        playerStat.decreaseHp(damage);
        if (playerStat.health <= 0)
        {
            currentState = State.Dead;
            //animator.SetTrigger("Die"); // ��� �ִϸ��̼� ���
        }
    }

    private void PlayerAttack()
    {
        if (currentMonster != null && currentMonster.GetActive())
        {
            totalDamage = PlayerDamageCalculator.GetTotalDamage();
            // ���� �ִϸ��̼� �������
            // animator.SetTrigger("Attack");
            currentMonster.TakeDamage(totalDamage);

        }
    }

    public void GetMonsterReward()
    {

        Debug.Log("��������");

        playerStat.AddExpFromMonsters(currentMonster);

        // �÷��̾� ���� ui ������Ʈ
        playerStat.UpdateLevelStatUI.Invoke();

        currentMonster = null;

        currentState = State.Idle;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            currentMonster = collision.gameObject.GetComponent<Enemy>();
            currentState = State.Attacking;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (!currentMonster.GetActive())
            {
                GetMonsterReward();
            }
            else
            {
                currentState = State.Idle;
            }
        }
    }

    public void UseBuffSkill(Skill skill)
    {
        // ���� ������ ����
        activeBuffSkills.Add(skill);
        StartCoroutine(BuffCoroutine(skill, skill.BaseData.buffDuration));
    }

    private IEnumerator BuffCoroutine(Skill skill, float skillTime)
    {
        // ������ �ð� ���� ���
        yield return new WaitForSeconds(skillTime);

        // ���� ����
        activeBuffSkills.Remove(skill);
    }
}
