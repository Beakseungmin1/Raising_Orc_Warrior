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

        // 스킬 효과를 데미지 계산기에 전달
        playerDamageCalculator.ApplySkillEffect(effect);

        // 효과 타입에 따라 처리
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
        yield return new WaitForSeconds(0.2f); // 모션 딜레이 (예시)

        // 범위 내 첫 번째 적 탐지 및 데미지
        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        bool hasHitTarget = false;

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                IEnemy enemy = target.GetComponent<IEnemy>();
                if (enemy != null && !hasHitTarget) // 첫 번째 적만 처리
                {
                    hasHitTarget = true; // 첫 번째 적 공격 완료
                    break; // 첫 번째 적만 공격하므로 루프 종료
                }
            }
        }

        if (!hasHitTarget)
        {
            Debug.Log("SkillEffectManager: 범위 내 적이 없습니다.");
        }

        Destroy(effectObj, effect.EffectDuration);
    }

    // 특정 영역(맵 중심 또는 타겟 위치)에서 효과 실행
    private void HandleAreaEffect(SkillEffect effect)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(effect.TargetPosition, effect.EffectRange);

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                GameObject areaEffect = Instantiate(effect.SkillPrefab, target.transform.position, UnityEngine.Quaternion.identity);
                Debug.Log($"SkillEffectManager: 범위 효과 생성 - {target.name}");

                Destroy(areaEffect, effect.EffectDuration);
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