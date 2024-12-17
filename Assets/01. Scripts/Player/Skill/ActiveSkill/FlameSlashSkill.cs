using UnityEngine;

public class FlameSlashSkill : ActiveSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("�Ҳɺ��� �ߵ�! ���� ���� ������ 1~2ȸ ����.");
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }
}