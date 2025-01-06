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

        playerDamageCalculator.ApplySkillEffect(effect);

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
        yield return new WaitForSeconds(0.35f); // ��� ������ (����)

        // ���� �� �� Ž��
        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        if (targets.Length == 0)
        {
            Debug.Log("SkillEffectManager: ���� �� ���� �����ϴ�.");
            Destroy(effectObj, effect.EffectDuration);
            yield break;
        }

        // ���� ����� �� Ž��
        Collider2D closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                float distance = UnityEngine.Vector2.Distance(playerWeaponPosition.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        // ���� ����� ������ ������ ����
        if (closestTarget != null)
        {
            IEnemy enemy = closestTarget.GetComponent<IEnemy>();
            if (enemy != null)
            {
                BigInteger skillDamage = playerDamageCalculator.CalculateSkillDamage(effect.DamagePercent);
                enemy.TakeDamage(skillDamage); // ������ ����
            }
        }
        else
        {
            Debug.Log("SkillEffectManager: ���� ����� ���� ã�� �� �����ϴ�.");
        }

        Destroy(effectObj, effect.EffectDuration);
    }

    // Ư�� ����(�� �߽� �Ǵ� Ÿ�� ��ġ)���� ȿ�� ����
    private void HandleAreaEffect(SkillEffect effect)
    {
        if (mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: �� �߽��� �������� �ʾҽ��ϴ�.");
            return;
        }

        StartCoroutine(PerformAreaEffect(effect));
    }

    private IEnumerator PerformAreaEffect(SkillEffect effect)
    {
        yield return new WaitForSeconds(0.3f); // ��� ������ (����)

        // �� �߽� ���� ���� �� ��� �� Ž��
        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                // ���� ��ġ�� ���� ����Ʈ ����
                GameObject effectOnTarget = Instantiate(effect.SkillPrefab, target.transform.position, UnityEngine.Quaternion.identity);
                Debug.Log($"SkillEffectManager: {target.name} ��ġ�� ����Ʈ ����");

                // ����Ʈ�� ���� �θ�� �����Ͽ� ��� ���󰡰� �ϱ�
                effectOnTarget.transform.SetParent(target.transform);

                // ������ ������ ����
                IEnemy enemy = target.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    BigInteger skillDamage = playerDamageCalculator.CalculateSkillDamage(effect.DamagePercent);
                    enemy.TakeDamage(skillDamage); // ��ų ������ ����
                }

                // ����Ʈ ���ӽð��� ���� ����
                Destroy(effectOnTarget, effect.EffectDuration);
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