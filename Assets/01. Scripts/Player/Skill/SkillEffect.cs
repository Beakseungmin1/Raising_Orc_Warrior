using UnityEngine;

public struct SkillEffect
{
    public GameObject SkillPrefab; // ����Ʈ ������
    public float DamagePercent;    // ������ ����
    public float BuffDuration;     // ���� ���� �ð�
    public float EffectRange;      // ��ų ����
    public EffectType EffectType;  // ����Ʈ Ÿ��
    public Vector3 TargetPosition; // ��� ��ġ

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