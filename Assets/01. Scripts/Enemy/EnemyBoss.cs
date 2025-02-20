﻿using System;
using System.Numerics;
using UnityEngine;
using System.Collections;

public class EnemyBoss : EnemyBase, IEnemy
{
    public EnemySO enemySO;

    [Header("Enemy information")]
    [SerializeField] private string enemyCode; // 적식별코드
    [SerializeField] private BigInteger hp; // 체력
    [SerializeField] private BigInteger maxHp; // 최대체력
    [SerializeField] private BigInteger giveExp; // 주는 경험치
    [SerializeField] private BigInteger giveMoney; // 주는 돈
    [SerializeField] private float giveDiamond;
    [SerializeField] private GameObject model; //적 모델
    [SerializeField] private Animator animator;
    private Collider2D enemyBossCollider;
    public float timeLimit = 50f;

    [Header("Skill Properties")]
    [SerializeField] private float cooldown; // 쿨다운 시간 (보스가 스킬을 지니고 있을시 사용)

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab; // 스킬 효과 프리팹
    [SerializeField] private Collider2D effectRange; // 스킬 효과 범위
    [SerializeField] private float damagePercent; // 액티브 스킬: 범위 내 적에게 주는 공격력 비율 (%)

    public Action OnEnemyDeath;

    public Action OnEnemyAttack;

    public Action OnEnemySkill;

    private PlayerBattle player;

    private int Hitcounter = 0;

    private void OnEnable()
    {
        SetupEnemy();
        ResetState();
        GameEventsManager.Instance.bossEvents.BossHPSet(maxHp);
        OnEnemyAttack = GiveDamageToPlayer;
        GameEventsManager.Instance.enemyEvents.onEnemyCleared += ClearEnemy;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyCleared -= ClearEnemy;
    }

    private void ResetState()
    {
        Hitcounter = 0;
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    public override void TakeDamage(BigInteger Damage)
    {
        base.TakeDamage(Damage);

        // 기본 레이어
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // IDLE 애니메이션이 재생 중 일때의 로직
        if (stateInfo.IsName("IDLE"))
        {
            animator.SetTrigger("3_Damaged");
        }

        if (hp - Damage > 0)
        {
            hp -= Damage;
        }
        else
        {
            hp = 0;
            Die();
        }
        GameEventsManager.Instance.bossEvents.BossHPChanged(hp);
    }

    private void EnemyAttack()
    {
        if (player != null && !player.GetActive())
        {
            animator.SetTrigger("2_Attack");
        }
    }

    public void GiveDamageToPlayer()
    {
        if (Hitcounter >= 2)
        {
            player.TakeKnockbackDamage(10, 0.5f); //안에 넣은 값은 임시값 이후 (몬스터고유데미지, 몬스터고유넉백시간) 으로 조정예정
            Hitcounter = 0;
            //Debug.Log("강력한 공격발동 현재 히트 : " + Hitcounter);
        }
        else
        {
            player.TakeDamage(10); // 안에 넣은 값은 임시값
            Hitcounter++;
            //Debug.Log("일반공격 현재 히트 : " + Hitcounter);
        }
    }

    public float GiveDiamond()
    {
        return giveDiamond;
    }

    public BigInteger GiveExp()
    {
        return giveExp;
    }

    public void Die()
    {
        GameEventsManager.Instance.bossEvents.TimerStop();
        animator.SetTrigger("4_Death");
        enemyBossCollider.enabled = false;

        GameEventsManager.Instance.StartCoroutine(DelayedReturnToPool());
        
    }

    public bool GetActive()
    {
        return gameObject.activeInHierarchy;
    }
    public BigInteger GiveMoney()
    {
        return giveMoney;
    }

    public void SetupEnemy()
    {
        enemyCode = enemySO.enemyCode;
        hp = BigInteger.Parse(enemySO.hpString);
        maxHp = BigInteger.Parse(enemySO.maxHpString);
        giveExp = BigInteger.Parse(enemySO.giveExpString);
        giveDiamond = enemySO.giveDiamond;
        //if (model == null)
        //{
        //    model = enemySO.model;
        //    model = Instantiate(model, transform);
        //}
        animator = GetComponentInChildren<Animator>();
        enemyBossCollider = GetComponent<Collider2D>();

        enemyBossCollider.enabled = true;
        cooldown = enemySO.cooldown;

        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerBattle>();
            InvokeRepeating("EnemyAttack", 0f, 1.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CancelInvoke("EnemyAttack");
            player = null;
        }
    }

    public void ClearEnemy()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private IEnumerator DelayedReturnToPool()
    {
        yield return new WaitForSeconds(1.0f);
        ObjectPool.Instance.ReturnObject(gameObject);
        StageManager.Instance.BossStageClear();
    }
}