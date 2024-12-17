using UnityEngine;

public class ThunderSlashSkill : ActiveSkill
{
    public ThunderSlashSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("���� ���� ���� �ߵ�! ���� 10 �̳��� �� 10�⿡�� ������.");
        SkillEffect effect = GetSkillEffect(targetPosition);

        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }
}