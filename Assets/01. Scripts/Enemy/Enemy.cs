using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour, IEnemy
{
    public EnemySO enemySO;

    [Header("Enemy information")]
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

    private float damageDisplayTimer = 0f;
    private bool isDisplayingDamage = false;

    [SerializeField] private Image healthBar;
    private Collider2D enemyCollider;

    private PlayerBattle playerBattle;
    public Action OnEnemyAttack;

    private void Start()
    {
        if (damageText != null)
        {
            damageText.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        playerBattle = PlayerObjManager.Instance.Player.PlayerBattle;

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

    public void TakeDamage(BigInteger Damage)
    {
        ShowDamage(Damage);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

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

    private void ShowDamage(BigInteger Damage)
    {
        if (damageText != null)
        {
            damageText.transform.position = transform.position + damageTextOffset;
            damageText.text = Damage.ToString();
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
        animator.SetTrigger("4_Death");
        enemyCollider.enabled = false; // 콜라이더 비활성화
        GameEventsManager.Instance.enemyEvents.EnemyKilled();
        StartCoroutine(DelayedReturnToPool());
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

        enemyCollider.enabled = true; // 콜라이더 활성화
        UpdateHealthBar();
    }       

    public void ClearEnemy()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
    }
}