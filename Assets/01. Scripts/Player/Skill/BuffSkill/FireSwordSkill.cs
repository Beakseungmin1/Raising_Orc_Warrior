using UnityEngine;

public class FireSwordSkill : BuffSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("���� �� �ߵ�! 10�ʰ� ��ü ���ݷ� ����.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}