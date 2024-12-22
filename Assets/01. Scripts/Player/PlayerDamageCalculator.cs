using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerDamageCalculator : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;

    public BigInteger TotalDamage;
    public float basicDamage;
    public BigInteger WeaponIncreaseDamage;
    public BigInteger SkillIncreaseDamage;

    private float damageMultiplier = 1.0f;
    private List<(float percent, float duration)> attackBuffs = new List<(float, float)>();

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
        equipManager = GetComponent<EquipManager>();
        Debug.Log("PlayerDamageCalculator initialized.");
    }

    public void ApplySkillEffect(SkillEffect effect)
    {
        Debug.Log($"[ApplySkillEffect] Received Skill Effect - DamagePercent: {effect.DamagePercent}, AttackIncreasePercent: {effect.AttackIncreasePercent}");

        if (effect.DamagePercent > 0)
        {
            Debug.Log($"Applying Damage Multiplier: {effect.DamagePercent}% for the next attack.");
            ApplyDamageMultiplier(effect.DamagePercent);
        }

        if (effect.AttackIncreasePercent > 0)
        {
            Debug.Log($"Adding Attack Buff: {effect.AttackIncreasePercent}% for {effect.BuffDuration} seconds.");
            AddAttackBuff(effect.AttackIncreasePercent, effect.BuffDuration);
        }

        UpdateValue(); // 업데이트 추가
    }

    public void UpdateValue()
    {
        basicDamage = stat.attackPower;
        Debug.Log($"Basic Damage: {basicDamage}");

        if (equipManager.EquippedWeapon != null)
        {
            WeaponIncreaseDamage = (BigInteger)(basicDamage * (equipManager.EquippedWeapon.BaseData.equipAtkIncreaseRate / 100));
            Debug.Log($"Weapon Increase Damage: {WeaponIncreaseDamage}");
        }

        SkillIncreaseDamage = 0;
        foreach (var buff in attackBuffs)
        {
            SkillIncreaseDamage += (BigInteger)(basicDamage * (buff.percent / 100));
        }
        Debug.Log($"Skill Increase Damage: {SkillIncreaseDamage}");
    }

    public BigInteger GetTotalDamage()
    {
        UpdateValue(); // 기본 데미지, 스킬 버프 반영

        Debug.Log($"[GetTotalDamage] Basic: {basicDamage}, Weapon Bonus: {WeaponIncreaseDamage}, Skill Bonus: {SkillIncreaseDamage}, Multiplier: {damageMultiplier}");
        TotalDamage = ((BigInteger)basicDamage + WeaponIncreaseDamage + SkillIncreaseDamage) * (BigInteger)damageMultiplier;

        Debug.Log($"[GetTotalDamage] Final Total Damage: {TotalDamage}");

        // Reset the multiplier **AFTER** returning the total damage
        StartCoroutine(ResetMultiplierAfterDelay());

        return TotalDamage;
    }

    private IEnumerator ResetMultiplierAfterDelay()
    {
        yield return null; // 다음 프레임까지 대기
        ResetDamageMultiplier();
    }

    private void ApplyDamageMultiplier(float multiplierPercent)
    {
        damageMultiplier = 1 + (multiplierPercent / 100f); // 기본 배율에 추가
        Debug.Log($"[ApplyDamageMultiplier] Damage Multiplier set to {damageMultiplier}.");
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1.0f;
        Debug.Log("[PlayerDamageCalculator] Damage Multiplier reset to 1.0");
    }

    private void AddAttackBuff(float percent, float duration)
    {
        Debug.Log($"[AddAttackBuff] Adding Attack Buff: {percent}% for {duration} seconds");
        attackBuffs.Add((percent, duration));
        StartCoroutine(RemoveAttackBuffAfterDuration(percent, duration));
        UpdateValue();
    }

    private IEnumerator RemoveAttackBuffAfterDuration(float percent, float duration)
    {
        yield return new WaitForSeconds(duration);
        attackBuffs.Remove((percent, duration));
        Debug.Log($"Attack Buff removed: {percent}%.");
        UpdateValue();
    }
}