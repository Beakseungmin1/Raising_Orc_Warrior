using UnityEngine;

public class BurningSwordSkill : PassiveSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("Ÿ������ �� �ߵ�! ���ݷ� ���� ȿ�� ����.");
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

    }

    protected override void EnhanceSkill()
    {
        // Ÿ������ �� ��ȭ ���� (����� �����)
    }
}