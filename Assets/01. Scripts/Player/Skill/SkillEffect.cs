using UnityEngine;

public struct SkillEffect
{
    public float DamagePercent; // ������ ����
    public float AttackIncreasePercent; // ���ݷ� ���� ����
    public float BuffDuration; // ���� ���� �ð�
    public float EffectRange; // ��ų ȿ�� ����
    public EffectType EffectType; // ����Ʈ Ÿ��
    public Vector3 TargetPosition; // ȿ�� ��� ��ġ

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