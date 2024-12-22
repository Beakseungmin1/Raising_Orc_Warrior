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
            Debug.LogError("SkillEffectManager: 필수 Transform이 설정되지 않았습니다.");
        }

        playerDamageCalculator = PlayerObjManager.Instance?.Player?.DamageCalculator;
    }

    public void TriggerEffect(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("SkillEffectManager: 유효하지 않은 스킬입니다.");
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

        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, Quaternion.identity);
        Debug.Log($"SkillEffectManager: 플레이어 무기 이펙트 생성 완료 - 지속시간: {effect.EffectDuration}초");

        Destroy(effectObj, effect.EffectDuration);
    }

    private void ApplyAreaEffect(SkillEffect effect)
    {
        GameObject effectObj = Instantiate(effect.SkillPrefab, effect.TargetPosition, Quaternion.identity);
        Debug.Log($"SkillEffectManager: 번개 이펙트 생성 완료 - 범위: {effect.EffectRange}, 지속시간: {effect.EffectDuration}초");

        Destroy(effectObj, effect.EffectDuration);
    }
}