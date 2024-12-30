using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour, IEnemy
{
    public EnemySO enemySO;

    [Header("Enemy Information")]
    [SerializeField] private string enemyCode;
    [SerializeField] private BigInteger hp;
    [SerializeField] private BigInteger maxHp;
    [SerializeField] private BigInteger giveExp;
    [SerializeField] private GameObject model;
    private Animator animator;

    [Header("Skill Properties")]
    [SerializeField] private float cooldown;

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab;
    [SerializeField] private Collider2D effectRange;
    [SerializeField] private float damagePercent;

    [Header("Damage UI")]
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private float damageDisplayDuration = 0.5f;
    [SerializeField] private UnityEngine.Vector3 damageTextOffset = new UnityEngine.Vector3(0, 1f, 0);

    [SerializeField] private Image healthBar;

    private Collider2D enemyCollider;
    private float damageDisplayTimer;
    private bool isDisplayingDamage;
    private bool isClearing;
    private Coroutine damageCoroutine;

    public Action OnEnemyAttack;

    private void Start()
    {
        if (damageText != null)
            damageText.gameObject.SetActive(false);
    }

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

    private void Update()
    {
        if (isDisplayingDamage)
        {
            damageDisplayTimer -= Time.deltaTime;
            if (damageDisplayTimer <= 0f)
            {
                isDisplayingDamage = false;
                damageText.gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(BigInteger damage)
    {
        ShowDamage(damage);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("IDLE") || stateInfo.IsName("DAMAGED"))
            animator.SetTrigger("3_Damaged");

        if (hp - damage > 0)
        {
            hp -= damage;
        }
        else
        {
            hp -= damage;
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

    private void ShowDamage(BigInteger damage)
    {
        if (damageText != null)
        {
            damageText.transform.position = transform.position + damageTextOffset;
            damageText.text = damage.ToString();
            damageText.gameObject.SetActive(true);
            damageDisplayTimer = damageDisplayDuration;
            isDisplayingDamage = true;
        }
    }

    public BigInteger GiveExp()
    {
        return giveExp;
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
        if (damageText != null)
        {
            damageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(damageDisplayDuration);
        }

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
        hp = enemySO.hp;
        maxHp = enemySO.maxHp;
        giveExp = enemySO.giveExp;

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
        isDisplayingDamage = false;
        damageDisplayTimer = 0f;

        if (damageText != null)
            damageText.gameObject.SetActive(false);

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

        GameEventsManager.Instance.StartCoroutine(ClearAndReturnToPool());
    }

    private IEnumerator ClearAndReturnToPool()
    {
        if (damageText != null && damageText.gameObject.activeSelf)
        {
            yield return new WaitForSeconds(damageDisplayDuration);
        }

        ObjectPool.Instance.ReturnObject(gameObject);
    }
}