using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill")]
public class SkillDataSO : BaseItemDataSO
{
    [Header("Skill Details")]
    public string description; // 스킬 설명
    public SkillType skillType; // 스킬 타입 (액티브, 버프, 패시브)

    [Header("Activation Condition")]
    public ActivationCondition activationCondition; // 발동 조건 (쿨다운, 공격 횟수)

    [Header("Skill Properties")]
    public float cooldown; // 쿨다운 시간
    public int requiredHits; // 필요한 공격 횟수
    public int manaCost; // 마나 소모량

    [Header("Skill Effects")]
    public AnimationClip animationClip; // 스킬 애니메이션 클립
    public float effectRange; // 효과 범위
    public float damagePercent; // 공격력 비율
    public float buffDuration; // 버프 지속 시간
    public float attackIncreasePercent; // 공격력 증가 비율

    [Header("Upgrade Info")]
    public int requireEmelardForUpgrade; // 업그레이드에 필요한 에메랄드 수
    public int requiredSkillCardsForUpgrade; // 업그레이드에 필요한 스킬 카드 수
}