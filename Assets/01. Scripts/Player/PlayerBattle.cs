using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
    private Rigidbody rigidbody;

    private BigInteger totalDamage; // ���⿡�� �޾ƿ� ����������
    private float attackSpeed; // ���� �ӵ� �ۼ�Ʈ �������� ����� ������ ���� ���ٿ���
    private float attackDelay = 1f; // ���� ������
    public List<Skill> activeBuffSkills = new List<Skill>(); // ���� Ȱ��ȭ�� ���� ��ų ����Ʈ
    public Animator animator;

    private Enemy currentMonster; // ���� ���� ���� ����

    private bool isSliding = false;

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
        playerStat = GetComponent<PlayerStat>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
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


    public void TakeDamage(BigInteger damage)
    {
        playerStat.decreaseHp(damage);

        if (playerStat.health <= 0)
        {
            currentState = State.Dead;
            //animator.SetTrigger("Die"); // ��� �ִϸ��̼� ���
        }
    }

    public void TakeKnockbackDamage(BigInteger damage, float Knockback)
    {
        playerStat.decreaseHp(damage);

        TakeHit(Knockback);

        if (playerStat.health <= 0)
        {
            currentState = State.Dead;
            //animator.SetTrigger("Die"); // ��� �ִϸ��̼� ���
        }
    }

    public void TakeHit(float power)
    {
        if (!isSliding)
        {
            StartCoroutine(SlideLeft(power));
        }
    }

    private IEnumerator SlideLeft(float power)
    {
        isSliding = true;

        // �������� �̲������� ���� velocity ����
        rigidbody.velocity = new UnityEngine.Vector2(-power, rigidbody.velocity.y);

        // ������ �ð� �Ŀ� velocity�� ������� �ǵ����ϴ�.
        yield return new WaitForSeconds(1);

        // �����̵� �� velocity�� 0���� ���� (�Ǵ� ���ϴ� ������ ����)
        rigidbody.velocity = UnityEngine.Vector2.zero;

        isSliding = false;
    }

    private void PlayerAttack()
    {
        if (currentMonster != null && currentMonster.GetActive())
        {
            totalDamage = PlayerDamageCalculator.GetTotalDamage();

            animator.SetTrigger("2_Attack");
            // ���� �ִϸ��̼� �������
            // animator.SetTrigger("Attack");
            currentMonster.TakeDamage(totalDamage);

        }
    }

    public void GetMonsterReward()
    {

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
            currentMonster = collision.GetComponent<Enemy>();
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
