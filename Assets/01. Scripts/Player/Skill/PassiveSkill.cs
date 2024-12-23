using UnityEngine;

public class PassiveSkill : BaseSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || isActivated) return;

        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);
        ResetCondition();
    }

    protected override void EndEffect()
    {
        base.EndEffect();
    }

    protected override void EnhanceSkill() { }
}