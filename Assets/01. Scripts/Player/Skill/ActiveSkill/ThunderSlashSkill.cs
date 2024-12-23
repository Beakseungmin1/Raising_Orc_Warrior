using UnityEngine;

public class ThunderSlashSkill : ActiveSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("���� ���� ���� �ߵ�! ���� 10 �̳��� �� 10�⿡�� ������.");
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }
}