using UnityEngine;

public class ManaBlessingSkill : BuffSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("마나의 축복 발동! 마나 회복 효과 적용.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}