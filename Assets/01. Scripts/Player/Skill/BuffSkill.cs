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
                Debug.Log("���� �������� ��ų �ߵ� ����.");
                return;
            }

            StartCoroutine(ApplyBuff());
            ResetCondition();
        }
    }

    private IEnumerator ApplyBuff()
    {
        isBuffActive = true;
        Debug.Log($"BuffSkill: {skillData.itemName} �ߵ� - ���ݷ� {skillData.attackIncreasePercent}% ����!");

        yield return new WaitForSeconds(skillData.buffDuration);

        Debug.Log($"BuffSkill: {skillData.itemName} ȿ�� ����");
        isBuffActive = false;
    }

    public override SkillEffect GetSkillEffect()
    {
        return new SkillEffect(
            0f, // ������ ���� ����
            skillData.attackIncreasePercent, // ���ݷ� ������ ����
            skillData.buffDuration, // ���� ���� �ð� ����
            0f, // ȿ�� ���� ����
            EffectType.OnPlayer, // ����Ʈ Ÿ�� ����
            Vector3.zero // ��� ��ġ ����
        );
    }
}