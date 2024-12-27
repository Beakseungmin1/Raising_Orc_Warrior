using UnityEngine;
using System.Collections;

public class SkillEffectManager : Singleton<SkillEffectManager>
{
    public Transform playerWeaponPosition;
    public Transform mapCenter;
    private PlayerDamageCalculator playerDamageCalculator;

    private void Start()
    {
        if (playerWeaponPosition == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: �ʼ� Transform�� �������� �ʾҽ��ϴ�.");
        }

        playerDamageCalculator = PlayerObjManager.Instance?.Player?.DamageCalculator;
    }

    public void TriggerEffect(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillEffectManager: ��ȿ���� ���� ��ų�Դϴ�.");
            return;
        }

        SkillEffect effect = skill.GetSkillEffect(targetPosition);

        playerDamageCalculator.ApplySkillEffect(effect);

        switch (effect.EffectType)
        {
            case EffectType.OnPlayer:
                ApplyWeaponEffect(effect);
                break;

            case EffectType.OnMapCenter:
                ApplyAreaEffect(effect);
                break;

            case EffectType.Projectile:
                ApplyProjectileEffect(effect);
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

        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, Quaternion.identity);
        Debug.Log($"SkillEffectManager: �÷��̾� ���� ����Ʈ ���� �Ϸ� - ���ӽð�: {effect.EffectDuration}��");

        Destroy(effectObj, effect.EffectDuration);
    }

    private void ApplyAreaEffect(SkillEffect effect)
    {
        GameObject effectObj = Instantiate(effect.SkillPrefab, effect.TargetPosition, Quaternion.identity);
        Debug.Log($"SkillEffectManager: ���� ����Ʈ ���� �Ϸ� - ����: {effect.EffectRange}, ���ӽð�: {effect.EffectDuration}��");

        Destroy(effectObj, effect.EffectDuration);
    }

    private void ApplyProjectileEffect(SkillEffect effect)
    {
        if (effect.SkillPrefab == null)
        {
            Debug.LogError("SkillEffectManager: ������Ÿ�� ��ų �������� �������� �ʾҽ��ϴ�.");
            return;
        }

        GameObject projectile = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = new Vector2(effect.EffectRange, 0);
        }

        ProjectileHandler projectileHandler = projectile.GetComponent<ProjectileHandler>();
        if (projectileHandler != null)
        {
            projectileHandler.Initialize(effect);
        }

        Destroy(projectile, effect.EffectDuration);
    }
}