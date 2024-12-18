using UnityEngine;

public class BurningSwordSkill : PassiveSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("타오르는 검 발동! 공격력 증가 효과 적용.");
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetSkillState();
    }

    protected override void EnhanceSkill()
    {
        // 타오르는 검 강화 로직 (현재는 비워둠)
    }
}