using UnityEngine;

public struct SkillEffect
{
    public float DamagePercent; // 데미지 비율
    public float AttackIncreasePercent; // 공격력 증가 비율
    public float BuffDuration; // 버프 지속 시간
    public float EffectRange; // 스킬 효과 범위
    public EffectType EffectType; // 이펙트 타입
    public Vector3 TargetPosition; // 효과 대상 위치

    public SkillEffect(float damagePercent, float attackIncreasePercent, float buffDuration, float effectRange, EffectType effectType, Vector3 targetPosition)
    {
        DamagePercent = damagePercent;
        AttackIncreasePercent = attackIncreasePercent;
        BuffDuration = buffDuration;
        EffectRange = effectRange;
        EffectType = effectType;
        TargetPosition = targetPosition;
    }
}