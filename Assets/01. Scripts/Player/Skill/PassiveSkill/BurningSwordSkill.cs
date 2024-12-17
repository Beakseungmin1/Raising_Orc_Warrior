using UnityEngine;

public class BurningSwordSkill : PassiveSkill
{
    public BurningSwordSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("타오르는 검 발동! 전체 공격력 증가 효과 적용.");
        SkillEffect effect = GetSkillEffect(Vector3.zero);

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}