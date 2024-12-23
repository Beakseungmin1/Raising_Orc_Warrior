using UnityEngine;

public class SpeedBoostSkill : BuffSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("����̵� �ߵ�! 10�ʰ� �̵��ӵ� ����.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}