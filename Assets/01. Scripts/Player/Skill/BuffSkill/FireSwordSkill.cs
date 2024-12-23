using UnityEngine;

public class FireSwordSkill : BuffSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("불의 검 발동! 10초간 전체 공격력 증가.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}