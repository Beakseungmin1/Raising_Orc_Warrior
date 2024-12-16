using System;
using System.Numerics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;

    [Header("Enemy information")]
    [SerializeField] private string enemyCode; // 적식별코드
    [SerializeField] private BigInteger hp; // 체력
    [SerializeField] private BigInteger maxHp; // 최대체력
    [SerializeField] private BigInteger giveExp; // 주는 경험치
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
        OnEnemyDeath += RegenManager.Instance.EnemyKilled;
    }

    //private void Start()
    //{
    //    SetupEnemy();
    //}

    public void TakeDamage(BigInteger Damage)
    {
        Debug.Log($"몬스터 체력: {hp}");
        Debug.Log($"데미지: {Damage}");
        animator.SetTrigger("2_Damaged");

        if (hp - Damage > 0)
        {
            hp -= Damage;
            //Debug.Log($"피해를 받음: {Damage}. 현재 HP: {hp}");
        }
        else
        {
            hp -= Damage;
            Die();
        }
    }

    public BigInteger GiveExp()
    {
        return giveExp;
    }

    public void Die()
    {
        animator.SetTrigger("4_Death");
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
        model = enemySO.model;
        model = Instantiate(model, transform);
        animator = GetComponentInChildren<Animator>();

        cooldown = enemySO.cooldown;

        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Monster"))
    //    {
    //        currentMonster = collision.gameObject.GetComponent<PlayerBattle>();
    //        currentState = State.Attacking;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Monster"))
    //    {
    //        if (!currentMonster.GetActive())
    //        {
    //            GetMonsterReward();
    //        }
    //        else
    //        {
    //            currentState = State.Idle;
    //        }
    //    }
    //}
}