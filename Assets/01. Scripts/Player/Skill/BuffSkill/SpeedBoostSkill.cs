using UnityEngine;

public class SpeedBoostSkill : BuffSkill
{
    public SpeedBoostSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("고속이동 발동! 10초간 이동속도 증가.");
        SkillEffect effect = GetSkillEffect(Vector3.zero);

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}