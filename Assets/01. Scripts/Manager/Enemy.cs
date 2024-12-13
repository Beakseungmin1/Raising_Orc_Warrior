﻿using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;

    [Header("Enemy information")]
    [SerializeField] private string enemyCode; // 적식별코드
    [SerializeField] private float hp; // 체력
    [SerializeField] private float maxHp; // 최대체력
    [SerializeField] private int giveExp; // 주는 경험치
    [SerializeField] private GameObject model; //적 모델
    [SerializeField] private Animator animator;


    [Header("Skill Properties")]
    [SerializeField] private float cooldown; // 쿨다운 시간 (보스가 스킬을 지니고 있을시 사용)

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab; // 스킬 효과 프리팹
    [SerializeField] private Collider2D effectRange; // 스킬 효과 범위
    [SerializeField] private float damagePercent; // 액티브 스킬: 범위 내 적에게 주는 공격력 비율 (%)

    public Action OnEnemyDeath;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        OnEnemyDeath += RegenManager.Instance.EnemyKilled;
    }

    private void Start()
    {
        SetupEnemy();
    }

    public void TakeDamage(float Damage)
    {
        // 피격 애니메이션 재생 추가예정


        if (hp - Damage > 0)
        {
            hp -= Damage;
            Debug.Log($"피해를 받음: {Damage}. 현재 HP: {hp}");
        }
        else
        {
            hp -= Damage;
            Die();
        }
    }

    public int GiveExp()
    {
        return giveExp;
    }

    public void Die()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
        RegenManager.Instance.EnemyKilled();
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
        spriteRenderer.sprite = enemySO.sprite;

        cooldown = enemySO.cooldown;

        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;
    }
}