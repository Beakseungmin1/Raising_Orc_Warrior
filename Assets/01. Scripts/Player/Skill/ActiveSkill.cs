using UnityEngine;

public class ActiveSkill : BaseSkill
{
    public ActiveSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);
        ResetCondition();
    }
}