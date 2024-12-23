using UnityEngine;

public struct SkillEffect
{
    public GameObject SkillPrefab;
    public float DamagePercent;
    public float BuffDuration;
    public float EffectRange;
    public float EffectDuration;
    public EffectType EffectType;
    public Vector3 TargetPosition;
    public float AttackIncreasePercent;
    public float ManaRecoveryAmount;
    public float HpRecoveryAmount;
    public float MoveSpeedIncrease;

    public SkillEffect(
        GameObject skillPrefab,
        float damagePercent,
        float buffDuration,
        float effectRange,
        EffectType effectType,
        Vector3 targetPosition,
        float effectDuration,
        float attackIncreasePercent,
        float manaRecoveryAmount,
        float hpRecoveryAmount,
        float moveSpeedIncrease)
    {
        SkillPrefab = skillPrefab;
        DamagePercent = damagePercent;
        BuffDuration = buffDuration;
        EffectRange = effectRange;
        EffectType = effectType;
        TargetPosition = targetPosition;
        EffectDuration = effectDuration;
        AttackIncreasePercent = attackIncreasePercent;
        ManaRecoveryAmount = manaRecoveryAmount;
        HpRecoveryAmount = hpRecoveryAmount;
        MoveSpeedIncrease = moveSpeedIncrease;
    }
}