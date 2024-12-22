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

    protected override void EnhanceSkill() { }
}