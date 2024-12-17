using UnityEngine;

public class FlameSlashSkill : ActiveSkill
{
    public FlameSlashSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("�Ҳɺ��� �ߵ�! ���� ���� ������ 1~2ȸ ����.");
        SkillEffect effect = GetSkillEffect(targetPosition);

        // SkillEffectManager�� ����Ʈ ����
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }
}