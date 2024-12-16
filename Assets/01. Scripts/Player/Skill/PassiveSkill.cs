using UnityEngine;

public class PassiveSkill : BaseSkill
{
    public override void UpdateSkill()
    {
        base.UpdateSkill();

        if (skillData.activationCondition == ActivationCondition.Periodic)
        {
            ApplyPeriodicEffect();
        }
    }

    private void ApplyPeriodicEffect()
    {
        Debug.Log($"PassiveSkill: {skillData.itemName} 주기적 발동");
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (skillData.activationCondition == ActivationCondition.HitBased && IsReadyToActivate())
        {
            Debug.Log($"PassiveSkill: {skillData.itemName} 공격 기반 발동");
            ResetCondition();
        }
    }

    public override SkillEffect GetSkillEffect()
    {
        return new SkillEffect(
            0f, // 데미지 비율 없음
            skillData.attackIncreasePercent, // 공격력 증가율 전달
            skillData.buffDuration, // 버프 지속 시간 전달
            0f, // 효과 범위 없음
            EffectType.OnPlayer, // 이펙트 타입 전달
            Vector3.zero // 대상 위치 없음
        );
    }
}