using UnityEngine;

public class ActiveSkill : BaseSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate())
        {
            Debug.Log($"{skillData.itemName}은 쿨다운 중입니다.");
            return;
        }

        if (!ConsumeMana()) // 마나 소모 처리
        {
            Debug.Log("마나 부족으로 스킬 발동 실패.");
            return;
        }

        Debug.Log($"ActiveSkill: {skillData.itemName} 발동! 대상 위치: {targetPosition}");

        // 이펙트 생성
        if (skillData.effectPrefab != null)
        {
            Instantiate(skillData.effectPrefab, targetPosition, Quaternion.identity);
        }

        ResetCondition();
    }

    public override SkillEffect GetSkillEffect()
    {
        return new SkillEffect(
            skillData.damagePercent, // 데미지 비율 전달
            0f, // 공격력 증가는 없음
            0f, // 버프 지속시간 없음
            skillData.effectRange, // 효과 범위 전달
            skillData.effectType, // 이펙트 타입 전달
            Vector3.zero // 대상 위치 없음
        );
    }
}