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

    private BigInteger totalDamage; // 계산기에서 받아온 최종데미지
    private float attackSpeed; // 공격 속도 퍼센트 게이지로 만들어 딜레이 에서 빼줄예정
    private float attackDelay = 1f; // 공격 딜레이
    private bool isDead;
    public List<BaseSkill> activeBuffSkills = new List<BaseSkill>(); // 현재 활성화된 버프 스킬 리스트
    private PlayerSkillHandler skillHandler;

    public Action OnPlayerAttack;

    private IEnemy currentMonster; // 현재 공격 중인 몬스터

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
        playerStat = GetComponent<PlayerStat>();
        animator = GetComponentInChildren<Animator>();
        currentState = State.Idle;

        skillHandler = GetComponent<PlayerSkillHandler>();
        if (skillHandler != null)
        {
            skillHandler.OnSkillUsed += HandleSkillUsed; // 스킬 사용 이벤트 구독
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
                // 스킬 상태에서도 currentMonster를 유지
                if (currentMonster == null || !currentMonster.GetActive())
                {
                    currentState = State.Idle; // 적이 없으면 Idle 상태로 전환
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

    // 데미지 만큼 데미지를 주고 Knockback의 시간만큼 밀어냄
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

        // 플레이어 레벨 ui 업데이트
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
        // 1. 스킬 상태로 전환
        currentState = State.Skill;

        // 2. 애니메이션 실행
        animator.Play("IDLE"); // 애니메이션 초기화
        animator.SetTrigger("2_Attack"); // 스킬 애니메이션 실행
        // 4. 일정 시간 후 Idle 상태로 복귀
        StartCoroutine(ResetToIdleAfterSkill());
    }

    private IEnumerator ResetToIdleAfterSkill()
    {
        // 스킬 애니메이션 지속 시간 동안 대기
        yield return new WaitForSeconds(0.2f); // 필요에 따라 조정 (스킬 애니메이션 시간)

        // 적이 있는 경우 Attacking 상태로 복귀
        if (currentMonster != null && currentMonster.GetActive())
        {
            currentState = State.Attacking;
        }
        else
        {
            currentState = State.Idle;
        }

        // 트리거 초기화
        animator.ResetTrigger("2_Attack");
        // 애니메이션 강제 호출 제거 (Animator가 상태 전환을 처리)
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (skillHandler != null)
        {
            skillHandler.OnSkillUsed -= HandleSkillUsed;
        }
    }
}
