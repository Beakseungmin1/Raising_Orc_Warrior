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
        Skill,
        StoppedIdle
    }

    private State currentState;
    private PlayerDamageCalculator PlayerDamageCalculator;
    private PlayerStat playerStat;
    public ParallaxBackground background;
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

        GameEventsManager.Instance.enemyEvents.onEnemyDie += GetMonsterReward;
    }

    private void Update()
    {
        if (background == null)
        {
            background = BackgroundManager.Instance.ParallaxBackground;
        }

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

            case State.StoppedIdle:
                {
                    currentState = State.StoppedIdle;
                    CancelInvoke("PlayerAttack");
                    BattleManager.Instance.StartBattle();
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
        GameEventsManager.Instance.bossEvents.TimerStop();
        
        StageManager.Instance.isPlayerInBossStage = false;

        isDead = true;
        animator.SetTrigger("4_Death");
        animator.SetBool("isDeath", true);
        currentState = State.Dead;

        if (DungeonManager.Instance.isPlayerInDungeon)
        {
            bool isCleared = false;
            GameEventsManager.Instance.dungeonEvents.PlayerDeadOrTimeEndInDungeon(isCleared);
        }
        else
        {
            StartCoroutine(DelayBeforeResurrection(2f));
        }
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

        totalDamage = PlayerDamageCalculator.GetTotalDamage(true, true);

        currentMonster.TakeDamage(totalDamage);

        SoundManager.Instance.PlaySFXOneShot(SFXType.playerAttack1);
    }

    public void GetMonsterReward(IEnemy enemy)
    {
        
        playerStat.AddExpFromMonsters(enemy);
        BigInteger monsterGold = enemy.GiveMoney();
        CurrencyManager.Instance.AddGold(monsterGold);
        CurrencyManager.Instance.AddCurrency(CurrencyType.Diamond, enemy.GiveDiamond());
        currentMonster = null;

        if (!isDead)
        {
            StartCoroutine(DelayBeforeReturningToIdle());
        }
    }

    private IEnumerator DelayBeforeReturningToIdle()
    {
        bool isDead = false;
        animator.ResetTrigger("7_Skill");
        animator.ResetTrigger("3_Damaged");
        animator.SetBool("isDeath", false);
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
            if (!isDead)
            {
                animator.ResetTrigger("7_Skill");
                animator.ResetTrigger("3_Damaged");
                animator.SetBool("2_Attack", false);
                animator.Play("IDLE");
                currentMonster = null;
                currentState = State.Idle;
            }
        }
    }

    private void HandleSkillUsed(BaseSkill skill)
    {
        if (skill is ActiveSkill activeSkill)
        {
            if (playerStat.mana < activeSkill.SkillData.manaCost)
            {
                return;
            }
            currentState = State.Skill;
            animator.SetTrigger("7_Skill");
            StartCoroutine(ResetToIdleAfterSkill());
        }
    }

    private IEnumerator ResetToIdleAfterSkill()
    {
        yield return new WaitForSeconds(0.5f);

        animator.ResetTrigger("7_Skill");
        animator.SetBool("2_Attack", false);

        // 몬스터가 없거나 죽었다면 Idle 상태로 돌아감
        if (currentMonster == null || !currentMonster.GetActive())
        {
            // 애니메이션이 끝날 때까지 기다리고 상태 전환
            animator.Play("IDLE");
            currentState = State.Idle;
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

    public IEnumerator DelayBeforeResurrection(float delayTime)
    {
        BattleManager.Instance.StartBattle();
        yield return new WaitForSeconds(delayTime);

        if (!DungeonManager.Instance.isPlayerInDungeon)
        {
            StageManager.Instance.BackToLastStage();
            yield return new WaitForSeconds(1f);
        }

        currentState = State.Idle;
        animator.Play("IDLE");

        isDead = false;
    }

    public void SetPlayerStateIdle()
    {
        isDead = false;
        animator.ResetTrigger("7_Skill");
        animator.ResetTrigger("3_Damaged");
        animator.SetBool("isDeath", false);
        animator.SetBool("2_Attack", false);
        currentState = State.Idle;
        animator.Play("IDLE");
        currentMonster = null;

    }

    public void SetPlayerStateStoppedIdle()
    {
        animator.ResetTrigger("7_Skill");
        animator.ResetTrigger("3_Damaged");
        animator.SetBool("isDeath", false);
        animator.SetBool("2_Attack", false);
        currentState = State.StoppedIdle;
        animator.Play("IDLE");
        currentMonster = null;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}