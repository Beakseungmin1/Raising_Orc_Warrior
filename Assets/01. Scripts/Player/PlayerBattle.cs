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
                    // 몬스터가 없으면 Idle로 복귀
                    currentState = State.Idle;
                    animator.Play("IDLE"); // Animator 상태를 Idle로 강제 전환
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

        StartCoroutine(DelayBeforeResurrection());
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
        currentMonster = null;

        if (!isDead)
        {
            StartCoroutine(DelayBeforeReturningToIdle());
        }
    }

    private IEnumerator DelayBeforeReturningToIdle()
    {
        animator.SetBool("2_Attack", false);
        yield return new WaitForSeconds(0.5f);
        currentState = State.Idle;
        animator.Play("IDLE");
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
        animator.SetTrigger("7_Skill");
        StartCoroutine(ResetToIdleAfterSkill());
    }

    private IEnumerator ResetToIdleAfterSkill()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.2f);

        animator.ResetTrigger("7_Skill");
        animator.SetBool("2_Attack", false);

        if (currentMonster == null || !currentMonster.GetActive())
        {
            currentState = State.Idle;
            animator.Play("IDLE");
        }
        else
        {
            currentState = State.Attacking;
        }
    }

    private void OnDestroy()
    {
        if (skillHandler != null)
        {
            skillHandler.OnSkillUsed -= HandleSkillUsed;
        }
    }

    private IEnumerator DelayBeforeResurrection()
    {
        if (DungeonManager.Instance.playerIsInDungeon)
        {
            yield return new WaitForSeconds(2f);

            bool isCleared = false;
            GameEventsManager.Instance.dungeonEvents.PlayerFinishDungeon(isCleared);

            yield return new WaitForSeconds(5f);

            playerStat.RefillHP();
            isDead = false;
            animator.Play("IDLE");
            animator.SetBool("isDeath", false);
            currentState = State.Idle;
        }
        else
        {
            yield return new WaitForSeconds(5f);

            playerStat.RefillHP();
            isDead = false;
            animator.Play("IDLE");
            animator.SetBool("isDeath", false);
            currentState = State.Idle;
            StageManager.Instance.BackToLastStage();
        }
    }
}