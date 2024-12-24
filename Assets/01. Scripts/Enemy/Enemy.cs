using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IEnemy
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

    [SerializeField] private Image healthBar;

    private PlayerBattle player;

    public Action OnEnemyAttack;

    private int Hitcounter = 0;

    public void TakeDamage(BigInteger Damage)
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 기본 레이어

        // IDLE 애니메이션이 재생 중 일때의 로직
        if (stateInfo.IsName("IDLE") || stateInfo.IsName("DAMAGED"))
        {
            animator.SetTrigger("3_Damaged");
        }

        if (hp - Damage > 0)
        {
            hp -= Damage;
        }
        else
        {
            hp -= Damage;
            Die();
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float fillAmount = (float)((double)hp / (double)maxHp);
        healthBar.fillAmount = fillAmount;
    }

    public BigInteger GiveExp()
    {
        return giveExp;
    }

    public void Die()
    {
        animator.SetTrigger("4_Death");
        ObjectPool.Instance.ReturnObject(gameObject);
        GameEventsManager.Instance.enemyEvents.EnemyKilled();
    }

    public bool GetActive()
    {
        return gameObject.activeInHierarchy;
    }

    public BigInteger GetHealth()
    {
        return maxHp - hp;
    }

    public void SetupEnemy()
    {
        enemyCode = enemySO.enemyCode;
        hp = enemySO.hp;
        maxHp = enemySO.maxHp;
        giveExp = enemySO.giveExp;
        if (model == null)
        {
            model = enemySO.model;
            model = Instantiate(model, transform);
        }
        animator = GetComponentInChildren<Animator>();

        cooldown = enemySO.cooldown;

        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;
        UpdateHealthBar();
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
}