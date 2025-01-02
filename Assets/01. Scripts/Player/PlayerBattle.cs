using System;
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
        Dead,
        Skill
    }

    private State currentState;
    private PlayerDamageCalculator PlayerDamageCalculator;
    private PlayerStat playerStat;
    [SerializeField] private ParallaxBackground background;
    public Animator animator;

    private BigInteger totalDamage;
    private float attackSpeed;
    private float attackDelay = 1f;
    private bool isDead;
    public List<BaseSkill> activeBuffSkills = new List<BaseSkill>();
    private PlayerSkillHandler skillHandler;

    public Action OnPlayerAttack;
    public Action OnPlayerSkill;

    private IEnemy currentMonster;

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
        playerStat = GetComponent<PlayerStat>();
        animator = GetComponentInChildren<Animator>();
        currentState = State.Idle;

        skillHandler = GetComponent<PlayerSkillHandler>();
        if (skillHandler != null)
        {
            skillHandler.OnSkillUsed += HandleSkillUsed;
        }

        OnPlayerAttack += GiveDamageToEnemy;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                CancelInvoke("PlayerAttack");
                animator.SetBool("2_Attack", false);
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

            case State.Skill:
                if (currentMonster == null || !currentMonster.GetActive())
                {
                    // ���Ͱ� ������ Idle�� ����
                    currentState = State.Idle;
                    animator.Play("IDLE"); // Animator ���¸� Idle�� ���� ��ȯ
                }
                break;
        }
    }

    public void TakeDamage(BigInteger damage)
    {
        playerStat.decreaseHp(damage);

        if (playerStat.health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeKnockbackDamage(BigInteger damage, float Knockback)
    {
        if (currentState != State.Dead)
        {
            BattleManager.Instance.EndBattle();
            background.StartScrollingRight(Knockback);
        }

        animator.SetBool("2_Attack", false);
        animator.SetTrigger("3_Damaged");
        playerStat.decreaseHp(damage);

        if (playerStat.health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("4_Death");
        animator.SetBool("isDeath", true);
        currentState = State.Dead;
    }

    private void PlayerAttack()
    {
        if (currentMonster != null && currentMonster.GetActive())
        {
            animator.SetBool("2_Attack", true);
        }
    }

    public void GiveDamageToEnemy()
    {
        if (currentMonster == null)
        {
            return;
        }

        totalDamage = PlayerDamageCalculator.GetTotalDamage();

        currentMonster.TakeDamage(totalDamage);

        SoundManager.Instance.PlaySFX(SFXType.playerAttack2);
    }

    public void GetMonsterReward()
    {
        playerStat.AddExpFromMonsters(currentMonster);

        playerStat.UpdateLevelStatUI.Invoke();

        currentMonster = null;

        if (!isDead)
        {
            currentState = State.Idle;
        }
    }

    public bool GetActive()
    {
        return isDead;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            currentMonster = collision.GetComponent<IEnemy>();
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
                if (!isDead)
                {
                    currentState = State.Idle;
                }
            }
        }
    }

    private void HandleSkillUsed(BaseSkill skill)
    {
        currentState = State.Skill;

        // ��ų �ִϸ��̼� ����
        animator.SetTrigger("7_Skill");

        // ��ų ���� �� ���� ���� �� ���� �簳
        StartCoroutine(ResetToIdleAfterSkill());
    }

    private IEnumerator ResetToIdleAfterSkill()
    {
        // ��ų �ִϸ��̼� ���� �ð� ���
        yield return new WaitForSeconds(0.5f); // ��ų �ִϸ��̼� ����
        yield return new WaitForSeconds(0.2f); // �߰� ��� �ð�

        // ��ų Ʈ���� �ʱ�ȭ �� Idle ���·� ��ȯ
        animator.ResetTrigger("7_Skill");
        animator.SetBool("2_Attack", false); // ���� �ߴ�

        // ���Ͱ� ������ Idle�� ��ȯ
        if (currentMonster == null || !currentMonster.GetActive())
        {
            currentState = State.Idle; // Idle ���·� ����
            animator.Play("IDLE"); // Animator ���¸� Idle�� ���� ��ȯ
        }
        else
        {
            currentState = State.Attacking; // ���Ͱ� ������ ���� ���·� ��ȯ
        }
    }

    private void OnDestroy()
    {
        if (skillHandler != null)
        {
            skillHandler.OnSkillUsed -= HandleSkillUsed;
        }
    }
}