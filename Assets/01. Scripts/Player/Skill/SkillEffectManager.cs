using UnityEngine;
using System.Collections;
using System.Numerics;

public class SkillEffectManager : Singleton<SkillEffectManager>
{
    public Transform playerWeaponPosition; // �÷��̾� ���� ��ġ
    public Transform mapCenter; // �� �߽�
    private PlayerDamageCalculator playerDamageCalculator; // ������ ����

    private void Start()
    {
        if (playerWeaponPosition == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: �ʼ� Transform�� �������� �ʾҽ��ϴ�.");
        }

        playerDamageCalculator = PlayerObjManager.Instance?.Player?.DamageCalculator;
    }

    public void TriggerEffect(BaseSkill skill, UnityEngine.Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillEffectManager: ��ȿ���� ���� ��ų�Դϴ�.");
            return;
        }

        SkillEffect effect = skill.GetSkillEffect(targetPosition);

        // ��ų ȿ���� ������ ���⿡ ����
        playerDamageCalculator.ApplySkillEffect(effect);

        // ȿ�� Ÿ�Կ� ���� ó��
        HandleEffect(effect);
    }

    private void HandleEffect(SkillEffect effect)
    {
        switch (effect.EffectType)
        {
            case EffectType.OnPlayer:
                HandleWeaponEffect(effect);
                break;

            case EffectType.OnMapCenter:
                HandleAreaEffect(effect);
                break;

            case EffectType.Projectile:
                HandleProjectileEffect(effect);
                break;

            default:
                Debug.LogWarning($"SkillEffectManager: �� �� ���� ����Ʈ Ÿ�� - {effect.EffectType}");
                break;
        }
    }

    // ���⿡ ����Ʈ ���� �� ����
    private void HandleWeaponEffect(SkillEffect effect)
    {
        if (playerWeaponPosition == null)
        {
            Debug.LogError("SkillEffectManager: �÷��̾� ���� ��ġ�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� ��ġ���� ����Ʈ ����
        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, UnityEngine.Quaternion.identity);
        Debug.Log($"SkillEffectManager: ���⿡ ����Ʈ ���� �Ϸ� - ���ӽð�: {effect.EffectDuration}��");

        StartCoroutine(PerformWeaponEffect(effect, effectObj));
    }

    private IEnumerator PerformWeaponEffect(SkillEffect effect, GameObject effectObj)
    {
        yield return new WaitForSeconds(0.2f); // ��� ������ (����)

        // ���� �� ù ��° �� Ž�� �� ������
        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        bool hasHitTarget = false;

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                IEnemy enemy = target.GetComponent<IEnemy>();
                if (enemy != null && !hasHitTarget) // ù ��° ���� ó��
                {
                    hasHitTarget = true; // ù ��° �� ���� �Ϸ�
                    break; // ù ��° ���� �����ϹǷ� ���� ����
                }
            }
        }

        if (!hasHitTarget)
        {
            Debug.Log("SkillEffectManager: ���� �� ���� �����ϴ�.");
        }

        Destroy(effectObj, effect.EffectDuration);
    }

    // Ư�� ����(�� �߽� �Ǵ� Ÿ�� ��ġ)���� ȿ�� ����
    private void HandleAreaEffect(SkillEffect effect)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(effect.TargetPosition, effect.EffectRange);

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                GameObject areaEffect = Instantiate(effect.SkillPrefab, target.transform.position, UnityEngine.Quaternion.identity);
                Debug.Log($"SkillEffectManager: ���� ȿ�� ���� - {target.name}");

                Destroy(areaEffect, effect.EffectDuration);
            }
        }
    }

    // ����ü ó��
    private void HandleProjectileEffect(SkillEffect effect)
    {
        if (effect.SkillPrefab == null)
        {
            Debug.LogError("SkillEffectManager: ������Ÿ�� ��ų �������� �������� �ʾҽ��ϴ�.");
            return;
        }

        GameObject projectile = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, UnityEngine.Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = new UnityEngine.Vector2(effect.EffectRange, 0); // ����ü ����
        }

        ProjectileHandler projectileHandler = projectile.GetComponent<ProjectileHandler>();
        if (projectileHandler != null)
        {
            projectileHandler.Initialize(effect);
        }

        Destroy(projectile, effect.EffectDuration);
    }
}