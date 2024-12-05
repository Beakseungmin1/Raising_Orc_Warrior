using UnityEngine;

public class SkillEffectManager : MonoBehaviour
{
    public Transform playerPosition; // �÷��̾� ��ġ
    public Transform projectileSpawnPoint; // ����ü ���� ��ġ
    public Transform mapCenter; // �� �߾� ��ġ

    private void Awake()
    {
        if (playerPosition == null || projectileSpawnPoint == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: �ʼ� Transform�� �������� �ʾҽ��ϴ�.");
        }
    }

    public void TriggerEffect(Skill skill, Vector3 targetPosition)
    {
        if (skill == null || skill.BaseData == null)
        {
            Debug.LogWarning("SkillEffectManager: ��ȿ���� ���� ��ų �������Դϴ�.");
            return;
        }

        SkillDataSO skillData = skill.BaseData;

        if (skillData.effectPrefab == null)
        {
            Debug.LogWarning($"SkillEffectManager: ��ų {skillData.itemName}�� ����Ʈ �������� �������� �ʾҽ��ϴ�.");
            return;
        }

        switch (skillData.effectType)
        {
            case EffectType.OnPlayer:
                ApplyPlayerEffect(skillData);
                break;

            case EffectType.Projectile:
                SpawnProjectile(skillData, targetPosition);
                break;

            case EffectType.OnMapCenter:
                ApplyMapEffect(skillData, targetPosition);
                break;

            default:
                Debug.LogWarning($"SkillEffectManager: �� �� ���� ����Ʈ Ÿ�� - {skillData.effectType}");
                break;
        }
    }

    private void ApplyPlayerEffect(SkillDataSO skillData)
    {
        if (playerPosition == null)
        {
            Debug.LogError("SkillEffectManager: PlayerPosition�� �������� �ʾҽ��ϴ�.");
            return;
        }

        GameObject effect = Instantiate(skillData.effectPrefab, playerPosition.position, Quaternion.identity);
        if (effect == null)
        {
            Debug.LogError($"SkillEffectManager: ��ų {skillData.itemName}�� ����Ʈ ���� ����!");
            return;
        }

        Debug.Log($"SkillEffectManager: ��ų {skillData.itemName}�� ����Ʈ ���� �Ϸ�.");
        Destroy(effect, skillData.buffDuration);

        if (skillData.skillType == SkillType.Buff)
        {
            ApplyBuff(skillData);
        }
    }

    private void SpawnProjectile(SkillDataSO skillData, Vector3 targetPosition)
    {
        if (projectileSpawnPoint == null)
        {
            Debug.LogError("SkillEffectManager: ProjectileSpawnPoint�� �������� �ʾҽ��ϴ�.");
            return;
        }

        GameObject projectile = Instantiate(skillData.effectPrefab, projectileSpawnPoint.position, Quaternion.identity);
        if (projectile == null)
        {
            Debug.LogError($"SkillEffectManager: ��ų {skillData.itemName}�� ����ü ���� ����!");
            return;
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;
            rb.velocity = direction * skillData.effectRange; // ����ü �ӵ� ����
        }

        Debug.Log($"SkillEffectManager: ��ų {skillData.itemName}�� ����ü ���� �Ϸ�.");
        Destroy(projectile, 5f); // ����ü �ڵ� ����
    }

    private void ApplyMapEffect(SkillDataSO skillData, Vector3 targetPosition)
    {
        GameObject effect = Instantiate(skillData.effectPrefab, targetPosition, Quaternion.identity);
        if (effect == null)
        {
            Debug.LogError($"SkillEffectManager: ��ų {skillData.itemName}�� �� �߽� ����Ʈ ���� ����!");
            return;
        }

        Debug.Log($"SkillEffectManager: ��ų {skillData.itemName}�� �� �߽� ����Ʈ ���� �Ϸ�.");
        Destroy(effect, skillData.buffDuration);

        if (skillData.skillType == SkillType.Active)
        {
            DealAreaDamage(skillData, targetPosition);
        }
    }

    private void ApplyBuff(SkillDataSO skillData)
    {
        Debug.Log($"SkillEffectManager: ���� ȿ�� ���� - ���ݷ� {skillData.attackIncreasePercent}% ����.");
    }

    private void DealAreaDamage(SkillDataSO skillData, Vector3 targetPosition)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(targetPosition, skillData.effectRange);

        foreach (var enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = skillData.damagePercent; // ������ ���
                damageable.TakeDamage(damage);
                Debug.Log($"SkillEffectManager: {enemy.name}���� {damage} ������ ����.");
            }
        }
    }
}