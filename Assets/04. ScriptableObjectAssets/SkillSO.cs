using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName; // 스킬 이름
    public string description; // 스킬 설명

    [Header("Skill Type")]
    public SkillType skillType; // 스킬 타입 (액티브, 버프, 패시브)

    [Header("Activation Condition")]
    public ActivationCondition activationCondition; // 발동 조건 (쿨다운, 공격 횟수)

    [Header("Skill Properties")]
    public float cooldown; // 쿨다운 시간 (쿨다운 기반일 때 유효)
    public int requiredHits; // 필요한 공격 횟수 (공격 횟수 기반일 때 유효)
    public int manaCost; // 마나 소모량

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // 스킬 효과 프리팹
    public float effectRange; // 스킬 효과 범위
    public float damagePercent; // 액티브 스킬: 범위 내 적에게 주는 공격력 비율 (%)
    public float buffDuration; // 버프 지속 시간 (버프 스킬에만 유효)
    public float attackIncreasePercent; // 버프 또는 패시브: 공격력 증가 비율 (%)


    [Header("Gacha Data")]
    public Grade skillGrade; // 스킬 등급
    public int currentSkillLevel; // 현재 스킬 레벨
    public int requireEmelardForUpgrade; // 업그레이드에 필요한 에메랄드 수
    public int requiredSkillCardsForUpgrade; // 업그레이드에 필요한 스킬 카드 수
    public int currentStackAmount; // 현재 들고 있는 스킬 카드 수
}