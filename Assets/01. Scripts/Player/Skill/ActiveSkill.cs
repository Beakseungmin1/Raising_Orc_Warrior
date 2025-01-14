using UnityEngine;

public class ActiveSkill : BaseSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate())
        {
            return;
        }

        if (!ConsumeMana())
        {
            return;
        }

        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);
        ResetCondition();
    }

    protected override void EnhanceSkill()
    {
        if (skillEffect.DamagePercent > 0)
        {
            skillEffect.DamagePercent *= 1.05f;
            skillEffect.DamagePercent = Mathf.RoundToInt(skillEffect.DamagePercent);
        }
    }
}