using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;

    [Header("Enemy information")]
    [SerializeField] private string enemyCode; // 적식별코드
    [SerializeField] private float hp; // 체력
    [SerializeField] private float maxHp; // 최대체력
    SpriteRenderer spriteRenderer;

    [Header("Skill Properties")]
    [SerializeField] private float cooldown; // 쿨다운 시간 (보스가 스킬을 지니고 있을시 사용)

    [Header("Skill Effects")]
    [SerializeField] private GameObject skillEffectPrefab; // 스킬 효과 프리팹
    [SerializeField] private Collider2D effectRange; // 스킬 효과 범위
    [SerializeField] private float damagePercent; // 액티브 스킬: 범위 내 적에게 주는 공격력 비율 (%)

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        enemyCode = enemySO.enemyCode;
        hp = enemySO.hp;
        maxHp = enemySO.maxHp;
        spriteRenderer.sprite = enemySO.sprite;

        cooldown = enemySO.cooldown;

        skillEffectPrefab = enemySO.skillEffectPrefab;
        effectRange = enemySO.effectRange;
        damagePercent = enemySO.damagePercent;
    }
}