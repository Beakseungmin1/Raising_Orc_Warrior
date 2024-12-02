using UnityEngine;

public class SkillEffectManager : MonoBehaviour
{
    public Transform playerPosition;
    public Transform projectileSpawnPoint;
    public Transform mapCenter;

    public void TriggerEffect(Skill skill, Vector3 targetPosition)
    {
        SkillDataSO skillData = skill.BaseData;

        if (skillData.effectPrefab == null)
        {
            Debug.LogWarning("����Ʈ �������� �������� �ʾҽ��ϴ�.");
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
                Debug.LogWarning("�� �� ���� ����Ʈ Ÿ��");
                break;
        }
    }

    private void ApplyPlayerEffect(SkillDataSO skillData)
    {
        GameObject effect = Instantiate(skillData.effectPrefab, playerPosition.position, Quaternion.identity);
        Destroy(effect, skillData.buffDuration);

        if (skillData.skillType == SkillType.Buff)
            ApplyBuff(skillData);
    }

    private void SpawnProjectile(SkillDataSO skillData, Vector3 targetPosition)
    {
        GameObject projectile = Instantiate(skillData.effectPrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;
            rb.velocity = direction * skillData.effectRange; // ��: �ӵ��� ���� ����
        }

        Destroy(projectile, 5f); // ����ü ����
    }

    private void ApplyMapEffect(SkillDataSO skillData, Vector3 targetPosition)
    {
        GameObject effect = Instantiate(skillData.effectPrefab, targetPosition, Quaternion.identity);
        Destroy(effect, skillData.buffDuration);

        if (skillData.skillType == SkillType.Active)
            DealAreaDamage(skillData, targetPosition);
    }

    private void ApplyBuff(SkillDataSO skillData)
    {
        // ����: �÷��̾� ���ݷ� ����
        Debug.Log($"���� ȿ�� ����: ���ݷ� +{skillData.attackIncreasePercent}%");
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
            }
        }
    }
}