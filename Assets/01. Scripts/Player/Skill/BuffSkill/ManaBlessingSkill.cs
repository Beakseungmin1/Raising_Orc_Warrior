using UnityEngine;

public class ManaBlessingSkill : BuffSkill
{
    public ManaBlessingSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("������ �ູ �ߵ�! ���� ȸ�� ȿ�� ����.");
        SkillEffect effect = GetSkillEffect(Vector3.zero);

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}