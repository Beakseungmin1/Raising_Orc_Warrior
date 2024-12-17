using UnityEngine;

public class ManaBlessingSkill : BuffSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("������ �ູ �ߵ�! ���� ȸ�� ȿ�� ����.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}