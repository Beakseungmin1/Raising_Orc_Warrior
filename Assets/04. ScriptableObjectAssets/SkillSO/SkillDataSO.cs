using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill")]
public class SkillDataSO : BaseItemDataSO
{
    [Header("Skill Details")]
    public string description; // 스킬 설명
    public string effectDescription; // 스킬 효과 설명    
    public SkillType skillType; // 스킬 타입 (액티브, 버프, 패시브)
    public EffectType effectType; // 스킬 이펙트 타입

    [Header("Activation Condition")]
    public ActivationCondition activationCondition; // 발동 조건 (쿨다운, 공격 횟수)
    public float cooldown; // 쿨다운 시간
    public int requiredHits; // 필요한 공격 횟수
    public float periodicInterval;

    [Header("Skill Properties")]
    public float damagePercent; // 공격력 비율
    public int manaCost; // 마나 소모량
    public float effectRange; // 효과 범위
    public float buffDuration; // 버프 지속 시간
    public float attackIncreasePercent; // 공격력 증가 비율

    [Header("Upgrade Info")]
    public int requireSkillCardsForUpgrade; // 업그레이드에 필요한 카드수
    public int maxLevel; // 최대 레벨    

    [Header("Visual and Effects")]
    public GameObject effectPrefab; // 이펙트 프리팹
}