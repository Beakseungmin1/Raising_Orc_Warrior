﻿using System;
using System.Numerics;
using UnityEngine;

public class EnemyDungeonBoss : MonoBehaviour, IEnemy
{
    private enum Pattern
    {
        Pattern1,
        Pattern2,
        Pattern3
    }

    public EnemySO enemySO;
    public DungeonInfoSO dungeonInfo;

    [Header("Enemy information")]
    [SerializeField] private string enemyCode; // 적식별코드
    [SerializeField] private BigInteger hp; // 체력
    [SerializeField] private BigInteger maxHp; // 최대체력
    [SerializeField] private BigInteger giveExp; // 주는 경험치
    [SerializeField] private GameObject model; //적 모델
    [SerializeField] private Animator animator;
    public float timeLimit = 50f;

    [Header("Skill Properties")]
    [SerializeField] private float cooldown; // 쿨다운 시간 (보스가 스킬을 지니고 있을시 사용)

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab; // 스킬 효과 프리팹
    [SerializeField] private Collider2D effectRange; // 스킬 효과 범위
    [SerializeField] private float damagePercent; // 액티브 스킬: 범위 내 적에게 주는 공격력 비율 (%)

    private Pattern enemyPattern;

    private PlayerBattle player;

    float patternTime = 4f; // 패턴 유지시간
    float toTime = 4; // 패턴 시간 계산용 변수

    private void OnEnable()
    {
        SetupEnemy();
        GameEventsManager.Instance.bossEvents.BossHPSet(maxHp);
        GameEventsManager.Instance.enemyEvents.onEnemyCleared += ClearEnemy;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyCleared -= ClearEnemy;
    }

    private void Start()
    {
        InvokeRepeating("SwitchPattern", 0, patternTime);
    }

    private void Update()
    {
        toTime += Time.deltaTime;

        switch (enemyPattern)
        {
            case Pattern.Pattern1:
                SetfalseAnimation();
                animator.SetBool("Pattern1", true);
                break;
            case Pattern.Pattern2:
                SetfalseAnimation();
                animator.SetBool("Pattern2", true);
                break;
            case Pattern.Pattern3:
                SetfalseAnimation();
                animator.SetTrigger("Pattern3");
                break;
        }
    }

    public void SetfalseAnimation()
    {
        animator.SetBool("Pattern1", false);
        animator.SetBool("Pattern2", false);
    }

    public void TakeDamage(BigInteger Damage)
    {
        if (hp - Damage > 0)
        {
            hp -= Damage;
        }
        else
        {
            hp -= Damage;
            Die();
        }
        GameEventsManager.Instance.bossEvents.BossHPChanged(hp -= Damage);
    }

    public void GiveDamageToPlayer()
    {
        player.TakeDamage(10); // 안에 넣은 값은 임시값
    }

    public void GiveKnockDamageToPlayer()
    {
        player.TakeKnockbackDamage(10, 0.5f); //안에 넣은 값은 임시값 이후 (몬스터고유데미지, 몬스터고유넉백시간) 으로 조정예정
    }

    public BigInteger GiveExp()
    {
        return giveExp;
    }

    public void Die()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
        GameEventsManager.Instance.enemyEvents.EnemyKilled();

        DungeonManager.Instance.ClearDungeon(dungeonInfo.type, dungeonInfo.level);
    }

    public bool GetActive()
    {
        return gameObject.activeInHierarchy;
    }

    public void SetupEnemy()
    {
        enemyCode = enemySO.enemyCode;
        hp = enemySO.hp;
        maxHp = enemySO.maxHp;
        giveExp = enemySO.giveExp;
        animator = GetComponentInChildren<Animator>();

        cooldown = enemySO.cooldown;

        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;
    }

    void SwitchPattern()
    {
        if (toTime > patternTime)
        {
            Pattern newPattern;
            do
            {
                int ran = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Pattern)).Length);
                newPattern = (Pattern)ran;

            } while (newPattern == enemyPattern);

            enemyPattern = newPattern;
            toTime = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerBattle>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    public void ClearEnemy()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
    }
}