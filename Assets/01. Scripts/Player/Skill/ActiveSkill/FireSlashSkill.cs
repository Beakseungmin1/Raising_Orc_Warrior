using UnityEngine;

public class FlameSlashSkill : ActiveSkill
{
    public FlameSlashSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("불꽃베기 발동! 범위 내의 적에게 1~2회 공격.");
        SkillEffect effect = GetSkillEffect(targetPosition);

        // SkillEffectManager에 이펙트 전달
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }
}