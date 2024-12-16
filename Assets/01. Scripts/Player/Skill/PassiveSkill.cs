using UnityEngine;

public class PassiveSkill : BaseSkill
{
    public override void UpdateSkill()
    {
        base.UpdateSkill();

        if (skillData.activationCondition == ActivationCondition.Periodic)
        {
            ApplyPeriodicEffect();
        }
    }

    private void ApplyPeriodicEffect()
    {
        Debug.Log($"PassiveSkill: {skillData.itemName} �ֱ��� �ߵ�");
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (skillData.activationCondition == ActivationCondition.HitBased && IsReadyToActivate())
        {
            Debug.Log($"PassiveSkill: {skillData.itemName} ���� ��� �ߵ�");
            ResetCondition();
        }
    }

    public override SkillEffect GetSkillEffect()
    {
        return new SkillEffect(
            0f, // ������ ���� ����
            skillData.attackIncreasePercent, // ���ݷ� ������ ����
            skillData.buffDuration, // ���� ���� �ð� ����
            0f, // ȿ�� ���� ����
            EffectType.OnPlayer, // ����Ʈ Ÿ�� ����
            Vector3.zero // ��� ��ġ ����
        );
    }
}