using UnityEngine;

public class BurningSwordSkill : PassiveSkill
{
    public BurningSwordSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("Ÿ������ �� �ߵ�! ��ü ���ݷ� ���� ȿ�� ����.");
        SkillEffect effect = GetSkillEffect(Vector3.zero);

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}