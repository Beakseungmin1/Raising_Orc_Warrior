using UnityEngine;

public class SkillEffectManager : Singleton<SkillEffectManager>
{
    public Transform playerWeaponPosition; // 플레이어 무기 위치
    public Transform mapCenter; // 맵 중심 위치

    private void Awake()
    {
        if (playerWeaponPosition == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: 필수 Transform이 설정되지 않았습니다.");
        }
    }

    public void TriggerEffect(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillEffectManager: 유효하지 않은 스킬입니다.");
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
                Debug.LogWarning($"SkillEffectManager: 알 수 없는 이펙트 타입 - {effect.EffectType}");
                break;
        }
    }

    private void ApplyWeaponEffect(SkillEffect effect)
    {
        if (playerWeaponPosition == null)
        {
            Debug.LogError("SkillEffectManager: 플레이어 무기 위치가 설정되지 않았습니다.");
            return;
        }

        // 무기 위치에 이펙트 생성
        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, Quaternion.identity);
        Debug.Log($"SkillEffectManager: 플레이어 무기 이펙트 생성 완료 - 버프 지속시간: {effect.BuffDuration}초");

        Destroy(effectObj, effect.BuffDuration);
    }

    private void ApplyAreaEffect(SkillEffect effect)
    {
        // 번개처럼 떨어지는 이펙트 생성
        GameObject effectObj = Instantiate(effect.SkillPrefab, effect.TargetPosition, Quaternion.identity);
        Debug.Log($"SkillEffectManager: 번개 이펙트 생성 완료 - 범위: {effect.EffectRange}, 지속시간: {effect.BuffDuration}초");

        Destroy(effectObj, effect.BuffDuration);
    }
}