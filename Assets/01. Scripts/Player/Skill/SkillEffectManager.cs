using UnityEngine;

public class SkillEffectManager : MonoBehaviour
{
    public Transform playerPosition; // 플레이어 위치
    public Transform projectileSpawnPoint; // 투사체 시작 위치
    public Transform mapCenter; // 맵 중앙 위치

    private void Awake()
    {
        if (playerPosition == null || projectileSpawnPoint == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: 필수 Transform이 설정되지 않았습니다.");
        }
    }

    public void TriggerEffect(Skill skill, Vector3 targetPosition)
    {
        if (skill == null || skill.BaseData == null)
        {
            Debug.LogWarning("SkillEffectManager: 유효하지 않은 스킬 데이터입니다.");
            return;
        }

        SkillDataSO skillData = skill.BaseData;

        if (skillData.effectPrefab == null)
        {
            Debug.LogWarning($"SkillEffectManager: 스킬 {skillData.itemName}에 이펙트 프리팹이 설정되지 않았습니다.");
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
                Debug.LogWarning($"SkillEffectManager: 알 수 없는 이펙트 타입 - {skillData.effectType}");
                break;
        }
    }

    private void ApplyPlayerEffect(SkillDataSO skillData)
    {
        if (playerPosition == null)
        {
            Debug.LogError("SkillEffectManager: PlayerPosition이 설정되지 않았습니다.");
            return;
        }

        GameObject effect = Instantiate(skillData.effectPrefab, playerPosition.position, Quaternion.identity);
        if (effect == null)
        {
            Debug.LogError($"SkillEffectManager: 스킬 {skillData.itemName}의 이펙트 생성 실패!");
            return;
        }

        Debug.Log($"SkillEffectManager: 스킬 {skillData.itemName}의 이펙트 생성 완료.");
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
            Debug.LogError("SkillEffectManager: ProjectileSpawnPoint가 설정되지 않았습니다.");
            return;
        }

        GameObject projectile = Instantiate(skillData.effectPrefab, projectileSpawnPoint.position, Quaternion.identity);
        if (projectile == null)
        {
            Debug.LogError($"SkillEffectManager: 스킬 {skillData.itemName}의 투사체 생성 실패!");
            return;
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;
            rb.velocity = direction * skillData.effectRange; // 투사체 속도 설정
        }

        Debug.Log($"SkillEffectManager: 스킬 {skillData.itemName}의 투사체 생성 완료.");
        Destroy(projectile, 5f); // 투사체 자동 제거
    }

    private void ApplyMapEffect(SkillDataSO skillData, Vector3 targetPosition)
    {
        GameObject effect = Instantiate(skillData.effectPrefab, targetPosition, Quaternion.identity);
        if (effect == null)
        {
            Debug.LogError($"SkillEffectManager: 스킬 {skillData.itemName}의 맵 중심 이펙트 생성 실패!");
            return;
        }

        Debug.Log($"SkillEffectManager: 스킬 {skillData.itemName}의 맵 중심 이펙트 생성 완료.");
        Destroy(effect, skillData.buffDuration);

        if (skillData.skillType == SkillType.Active)
        {
            DealAreaDamage(skillData, targetPosition);
        }
    }

    private void ApplyBuff(SkillDataSO skillData)
    {
        Debug.Log($"SkillEffectManager: 버프 효과 적용 - 공격력 {skillData.attackIncreasePercent}% 증가.");
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
                Debug.Log($"SkillEffectManager: {enemy.name}에게 {damage} 데미지 적용.");
            }
        }
    }
}