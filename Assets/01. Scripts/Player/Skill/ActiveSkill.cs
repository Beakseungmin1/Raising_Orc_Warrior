using UnityEngine;

public class ActiveSkill : BaseSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate())
        {
            Debug.Log($"{skillData.itemName}�� ��ٿ� ���Դϴ�.");
            return;
        }

        if (!ConsumeMana()) // ���� �Ҹ� ó��
        {
            Debug.Log("���� �������� ��ų �ߵ� ����.");
            return;
        }

        Debug.Log($"ActiveSkill: {skillData.itemName} �ߵ�! ��� ��ġ: {targetPosition}");

        // ����Ʈ ����
        if (skillData.effectPrefab != null)
        {
            Instantiate(skillData.effectPrefab, targetPosition, Quaternion.identity);
        }

        ResetCondition();
    }

    public override SkillEffect GetSkillEffect()
    {
        return new SkillEffect(
            skillData.damagePercent, // ������ ���� ����
            0f, // ���ݷ� ������ ����
            0f, // ���� ���ӽð� ����
            skillData.effectRange, // ȿ�� ���� ����
            skillData.effectType, // ����Ʈ Ÿ�� ����
            Vector3.zero // ��� ��ġ ����
        );
    }
}