using UnityEngine;

public class SkillEffect
{
    public GameObject SkillPrefab;
    public float DamagePercent;
    public float BuffDuration;
    public float EffectRange;
    public float EffectDuration;
    public EffectType EffectType;
    public float AttackIncreasePercent;
    public float ManaRecoveryAmount;
    public float HpRecoveryAmount;
    public float MoveSpeedIncrease;
    public float AttackSpeedIncrease;

    public SkillEffect(
        GameObject skillPrefab,
        float damagePercent,
        float buffDuration,
        float effectRange,
        EffectType effectType,
        float effectDuration,
        float attackIncreasePercent,
        float manaRecoveryAmount,
        float hpRecoveryAmount,
        float moveSpeedIncrease,
        float attackSpeedIncrease)
    {
        SkillPrefab = skillPrefab;
        DamagePercent = damagePercent;
        BuffDuration = buffDuration;
        EffectRange = effectRange;
        EffectType = effectType;
        EffectDuration = effectDuration;
        AttackIncreasePercent = attackIncreasePercent;
        ManaRecoveryAmount = manaRecoveryAmount;
        HpRecoveryAmount = hpRecoveryAmount;
        MoveSpeedIncrease = moveSpeedIncrease;
        AttackSpeedIncrease = attackSpeedIncrease;
    }
}
