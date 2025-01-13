using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Collections;

public class Enemy : EnemyBase, IEnemy
{
    public EnemySO enemySO;

    [Header("Enemy Information")]
    [SerializeField] private string enemyCode;
    [SerializeField] private BigInteger hp;
    [SerializeField] private BigInteger maxHp;
    [SerializeField] private BigInteger giveExp;
    [SerializeField] private BigInteger giveMoney;
    [SerializeField] private GameObject model;
    private Animator animator;

    [Header("Skill Properties")]
    [SerializeField] private float cooldown;

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab;
    [SerializeField] private Collider2D effectRange;
    [SerializeField] private float damagePercent;

    [Header("UI Components")]
    [SerializeField] private Image healthBar;

    private Collider2D enemyCollider;
    private bool isClearing;
    private Coroutine damageCoroutine;

    public Action OnEnemyAttack;

    private void OnEnable()
    {
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        GameEventsManager.Instance.enemyEvents.onEnemyCleared += ClearEnemy;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.enemyEvents.onEnemyCleared -= ClearEnemy;
    }

    public override void TakeDamage(BigInteger damage)
    {
        base.TakeDamage(damage);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("IDLE") || stateInfo.IsName("DAMAGED"))
            animator.SetTrigger("3_Damaged");

        if (hp - damage > 0)
        {
            hp -= damage;
        }
        else
        {
            hp = 0;
            UpdateHealthBar();
            Die();
            return;
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

    public BigInteger GiveMoney()
    {
        return giveMoney;
    }

    public void Die()
    {
        if (isClearing) return;

        isClearing = true;
        animator.SetTrigger("4_Death");
        enemyCollider.enabled = false;
        GameEventsManager.Instance.enemyEvents.EnemyKilled();

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        GameEventsManager.Instance.StartCoroutine(DelayedReturnToPool());
    }

    private IEnumerator DelayedReturnToPool()
    {
        yield return new WaitForSeconds(1.0f);
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    public bool GetActive()
    {
        return enemyCollider.enabled;
    }

    public BigInteger GetHealth()
    {
        return maxHp - hp;
    }

    public void SetupEnemy()
    {
        enemyCode = enemySO.enemyCode;
        hp = BigInteger.Parse(enemySO.hpString);
        maxHp = BigInteger.Parse(enemySO.maxHpString);
        giveExp = BigInteger.Parse(enemySO.giveExpString);
        giveMoney = BigInteger.Parse(enemySO.giveMoneyString);

        if (model == null)
        {
            model = enemySO.model;
            model = Instantiate(model, transform);
        }

        cooldown = enemySO.cooldown;
        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;

        enemyCollider.enabled = true;
        isClearing = false;

        UpdateHealthBar();
    }

    public void ClearEnemy()
    {
        if (isClearing) return;

        isClearing = true;

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        enemyCollider.enabled = false;
        BattleManager.Instance.StartBattle();
        GameEventsManager.Instance.StartCoroutine(ClearAndReturnToPool());
    }

    private IEnumerator ClearAndReturnToPool()
    {
        yield return new WaitForSeconds(1.0f);
        ObjectPool.Instance.ReturnObject(gameObject);
    }
}