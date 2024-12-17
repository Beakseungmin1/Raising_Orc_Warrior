using UnityEngine;

public class ThunderSlashSkill : ActiveSkill
{
    public ThunderSlashSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("수라 번개 베기 발동! 범위 10 이내의 적 10기에게 데미지.");
        SkillEffect effect = GetSkillEffect(targetPosition);

        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }
}