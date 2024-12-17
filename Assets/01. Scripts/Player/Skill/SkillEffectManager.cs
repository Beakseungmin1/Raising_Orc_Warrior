using UnityEngine;

public class SkillEffectManager : Singleton<SkillEffectManager>
{
    public Transform playerWeaponPosition; // �÷��̾� ���� ��ġ
    public Transform mapCenter; // �� �߽� ��ġ

    private void Awake()
    {
        if (playerWeaponPosition == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: �ʼ� Transform�� �������� �ʾҽ��ϴ�.");
        }
    }

    public void TriggerEffect(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillEffectManager: ��ȿ���� ���� ��ų�Դϴ�.");
            return;
        }

        SkillEffect effect = skill.GetSkillEffect(targetPosition);

        switch (effect.EffectType)
        {
            case EffectType.OnPlayer:
                ApplyWeaponEffect(effect);
                break;

            case EffectType.OnMapCenter:
                ApplyAreaEffect(effect);
                break;

            default:
                Debug.LogWarning($"SkillEffectManager: �� �� ���� ����Ʈ Ÿ�� - {effect.EffectType}");
                break;
        }
    }

    private void ApplyWeaponEffect(SkillEffect effect)
    {
        if (playerWeaponPosition == null)
        {
            Debug.LogError("SkillEffectManager: �÷��̾� ���� ��ġ�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ��ġ�� ����Ʈ ����
        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, Quaternion.identity);
        Debug.Log($"SkillEffectManager: �÷��̾� ���� ����Ʈ ���� �Ϸ� - ���� ���ӽð�: {effect.BuffDuration}��");

        Destroy(effectObj, effect.BuffDuration);
    }

    private void ApplyAreaEffect(SkillEffect effect)
    {
        // ����ó�� �������� ����Ʈ ����
        GameObject effectObj = Instantiate(effect.SkillPrefab, effect.TargetPosition, Quaternion.identity);
        Debug.Log($"SkillEffectManager: ���� ����Ʈ ���� �Ϸ� - ����: {effect.EffectRange}, ���ӽð�: {effect.BuffDuration}��");

        Destroy(effectObj, effect.BuffDuration);
    }
}