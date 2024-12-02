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
            Debug.LogWarning("이펙트 프리팹이 설정되지 않았습니다.");
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
                Debug.LogWarning("알 수 없는 이펙트 타입");
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
            rb.velocity = direction * skillData.effectRange; // 예: 속도로 범위 설정
        }

        Destroy(projectile, 5f); // 투사체 제거
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
        // 예시: 플레이어 공격력 증가
        Debug.Log($"버프 효과 적용: 공격력 +{skillData.attackIncreasePercent}%");
    }

    private void DealAreaDamage(SkillDataSO skillData, Vector3 targetPosition)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(targetPosition, skillData.effectRange);

        foreach (var enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = skillData.damagePercent; // 데미지 계산
                damageable.TakeDamage(damage);
            }
        }
    }
}