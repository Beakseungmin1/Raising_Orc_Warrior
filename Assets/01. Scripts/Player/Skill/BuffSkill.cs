using System.Collections;
using UnityEngine;

public class BuffSkill : BaseSkill
{
    private bool isBuffActive;

    public override void Activate(Vector3 targetPosition)
    {
        if (IsReadyToActivate() && !isBuffActive)
        {
            if (!ConsumeMana())
            {
                Debug.Log("마나 부족으로 스킬 발동 실패.");
                return;
            }

            StartCoroutine(ApplyBuff());
            ResetCondition();
        }
    }

    private IEnumerator ApplyBuff()
    {
        isBuffActive = true;
        Debug.Log($"BuffSkill: {skillData.itemName} 발동 - 공격력 {skillData.attackIncreasePercent}% 증가!");

        yield return new WaitForSeconds(skillData.buffDuration);

        Debug.Log($"BuffSkill: {skillData.itemName} 효과 종료");
        isBuffActive = false;
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