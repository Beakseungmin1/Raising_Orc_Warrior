using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IEnemy
{
    public EnemySO enemySO;

    [Header("Enemy information")]
    [SerializeField] private string enemyCode;
    [SerializeField] private BigInteger hp;
    [SerializeField] private BigInteger maxHp;
    [SerializeField] private BigInteger giveExp;
    [SerializeField] private GameObject model;
    [SerializeField] private Animator animator;

    [Header("Skill Properties")]
    [SerializeField] private float cooldown;

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab;
    [SerializeField] private Collider2D effectRange;
    [SerializeField] private float damagePercent;

    [Header("Damage UI")]
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private float damageDisplayDuration = 0.5f;
    [SerializeField] private UnityEngine.Vector3 damageTextOffset = new UnityEngine.Vector3(0, 1f, 0); // 오프셋 추가

    private float damageDisplayTimer = 0f;
    private bool isDisplayingDamage = false;

    [SerializeField] private Image healthBar;

    private PlayerBattle player;
    public Action OnEnemyAttack;

    private void Start()
    {
        if (damageText != null)
        {
            damageText.gameObject.SetActive(false); // 초기화 시 비활성화
        }
    }

    private void Update()
    {
        if (isDisplayingDamage)
        {
            damageDisplayTimer -= Time.deltaTime;
            if (damageDisplayTimer <= 0f)
            {
                isDisplayingDamage = false;
                damageText.gameObject.SetActive(false); // 텍스트 비활성화
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
            Die();
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
            // 텍스트 위치 업데이트
            damageText.transform.position = transform.position + damageTextOffset;

            // 데미지 텍스트 업데이트
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

    public void ClearEnemy()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
    }
}
