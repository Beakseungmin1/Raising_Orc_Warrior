using UnityEngine;

public struct SkillEffect
{
    public GameObject SkillPrefab;
    public float DamagePercent;
    public float BuffDuration;
    public float EffectRange;
    public EffectType EffectType;
    public Vector3 TargetPosition;
    public float EffectDuration;

    public SkillEffect(GameObject skillPrefab, float damagePercent, float buffDuration, float effectRange, EffectType effectType, Vector3 targetPosition, float effectDuration)
    {
        SkillPrefab = skillPrefab;
        DamagePercent = damagePercent;
        BuffDuration = buffDuration;
        EffectRange = effectRange;
        EffectType = effectType;
        TargetPosition = targetPosition;
        EffectDuration = effectDuration;
    }
}