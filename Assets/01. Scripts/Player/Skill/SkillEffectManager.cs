using UnityEngine;
using System.Collections;
using System.Numerics;

public class SkillEffectManager : Singleton<SkillEffectManager>
{
    public Transform playerWeaponPosition; // 플레이어 무기 위치
    public Transform mapCenter; // 맵 중심
    private PlayerDamageCalculator playerDamageCalculator; // 데미지 계산기

    private void Start()
    {
        if (playerWeaponPosition == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: 필수 Transform이 설정되지 않았습니다.");
        }

        playerDamageCalculator = PlayerObjManager.Instance?.Player?.DamageCalculator;
    }

    public void TriggerEffect(BaseSkill skill, UnityEngine.Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillEffectManager: 유효하지 않은 스킬입니다.");
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
                Debug.LogWarning($"SkillEffectManager: 알 수 없는 이펙트 타입 - {effect.EffectType}");
                break;
        }
    }

    // 무기에 이펙트 생성 및 실행
    private void HandleWeaponEffect(SkillEffect effect)
    {
        if (playerWeaponPosition == null)
        {
            Debug.LogError("SkillEffectManager: 플레이어 무기 위치가 설정되지 않았습니다.");
            return;
        }

        // 무기 위치에서 이펙트 생성
        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, UnityEngine.Quaternion.identity);
        Debug.Log($"SkillEffectManager: 무기에 이펙트 생성 완료 - 지속시간: {effect.EffectDuration}초");

        StartCoroutine(PerformWeaponEffect(effect, effectObj));
    }

    private IEnumerator PerformWeaponEffect(SkillEffect effect, GameObject effectObj)
    {
        yield return new WaitForSeconds(0.35f); // 모션 딜레이 (예시)

        // 범위 내 적 탐지
        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        if (targets.Length == 0)
        {
            Debug.Log("SkillEffectManager: 범위 내 적이 없습니다.");
            Destroy(effectObj, effect.EffectDuration);
            yield break;
        }

        // 가장 가까운 적 탐지
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

        // 가장 가까운 적에게 데미지 적용
        if (closestTarget != null)
        {
            IEnemy enemy = closestTarget.GetComponent<IEnemy>();
            if (enemy != null)
            {
                BigInteger skillDamage = playerDamageCalculator.CalculateSkillDamage(effect.DamagePercent);
                enemy.TakeDamage(skillDamage); // 데미지 적용
            }
        }
        else
        {
            Debug.Log("SkillEffectManager: 가장 가까운 적을 찾을 수 없습니다.");
        }

        Destroy(effectObj, effect.EffectDuration);
    }

    // 특정 영역(맵 중심 또는 타겟 위치)에서 효과 실행
    private void HandleAreaEffect(SkillEffect effect)
    {
        if (mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: 맵 중심이 설정되지 않았습니다.");
            return;
        }

        StartCoroutine(PerformAreaEffect(effect));
    }

    private IEnumerator PerformAreaEffect(SkillEffect effect)
    {
        yield return new WaitForSeconds(0.3f); // 모션 딜레이 (예시)

        // 맵 중심 기준 범위 내 모든 적 탐지
        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                // 적의 위치에 맞춰 이펙트 생성
                GameObject effectOnTarget = Instantiate(effect.SkillPrefab, target.transform.position, UnityEngine.Quaternion.identity);
                Debug.Log($"SkillEffectManager: {target.name} 위치에 이펙트 생성");

                // 이펙트를 적의 부모로 설정하여 계속 따라가게 하기
                effectOnTarget.transform.SetParent(target.transform);

                // 적에게 데미지 적용
                IEnemy enemy = target.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    BigInteger skillDamage = playerDamageCalculator.CalculateSkillDamage(effect.DamagePercent);
                    enemy.TakeDamage(skillDamage); // 스킬 데미지 적용
                }

                // 이펙트 지속시간에 맞춰 제거
                Destroy(effectOnTarget, effect.EffectDuration);
            }
        }
    }

    // 투사체 처리
    private void HandleProjectileEffect(SkillEffect effect)
    {
        if (effect.SkillPrefab == null)
        {
            Debug.LogError("SkillEffectManager: 프로젝타일 스킬 프리팹이 설정되지 않았습니다.");
            return;
        }

        GameObject projectile = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, UnityEngine.Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = new UnityEngine.Vector2(effect.EffectRange, 0); // 투사체 방향
        }

        ProjectileHandler projectileHandler = projectile.GetComponent<ProjectileHandler>();
        if (projectileHandler != null)
        {
            projectileHandler.Initialize(effect);
        }

        Destroy(projectile, effect.EffectDuration);
    }
}