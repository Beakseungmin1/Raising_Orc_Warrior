using UnityEngine;

public struct SkillEffect
{
    public GameObject SkillPrefab; // 이펙트 프리팹
    public float DamagePercent;    // 데미지 비율
    public float BuffDuration;     // 버프 지속 시간
    public float EffectRange;      // 스킬 범위
    public EffectType EffectType;  // 이펙트 타입
    public Vector3 TargetPosition; // 대상 위치

    public SkillEffect(GameObject skillPrefab, float damagePercent, float buffDuration, float effectRange, EffectType effectType, Vector3 targetPosition)
    {
        SkillPrefab = skillPrefab;
        DamagePercent = damagePercent;
        BuffDuration = buffDuration;
        EffectRange = effectRange;
        EffectType = effectType;
        TargetPosition = targetPosition;
    }
}