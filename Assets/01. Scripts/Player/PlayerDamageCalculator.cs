using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System;

public class PlayerDamageCalculator : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;

    public BigInteger TotalDamage;
    public float basicDamage;
    public BigInteger WeaponIncreaseDamage;
    public BigInteger SkillIncreaseDamage;
    private BigInteger rawTotalDamage;

    private float damageMultiplier = 1.0f;
    private List<(float percent, float duration)> attackBuffs = new List<(float, float)>();
    public event Action OnCriticalHit;

    private const float damageRandomVariation = 0.1f;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
        equipManager = GetComponent<EquipManager>();

        rawTotalDamage = (BigInteger)stat.attackPower;
    }

    public void ApplySkillEffect(SkillEffect effect)
    {
        if (effect.AttackIncreasePercent > 0)
        {
            AddAttackBuff(effect.AttackIncreasePercent, effect.BuffDuration);
        }

        UpdateValue();
    }

    public void UpdateValue()
    {
        basicDamage = stat.attackPower;

        if (equipManager.EquippedWeapon != null)
        {
            WeaponIncreaseDamage = (BigInteger)(basicDamage * (equipManager.EquippedWeapon.EquipAtkIncreaseRate / 100));
        }

        SkillIncreaseDamage = 0;
        foreach (var buff in attackBuffs)
        {
            SkillIncreaseDamage += (BigInteger)(basicDamage * (buff.percent / 100));
        }
    }

    public BigInteger GetTotalDamage()
    {
        UpdateValue();

        BigInteger baseDamage = (BigInteger)basicDamage;
        BigInteger weaponDamage = WeaponIncreaseDamage;
        BigInteger skillBuffDamage = SkillIncreaseDamage;

        rawTotalDamage = baseDamage + weaponDamage + skillBuffDamage;

        float randomMultiplier = UnityEngine.Random.Range(1 - damageRandomVariation, 1 + damageRandomVariation);
        float totalDamageWithRandom = (float)rawTotalDamage * randomMultiplier;

        bool criticalHit = false;

        if (UnityEngine.Random.Range(0, 100) < stat.criticalProbability)
        {
            criticalHit = true;

            totalDamageWithRandom += totalDamageWithRandom * (stat.criticalIncreaseDamage / 100f);

            if (UnityEngine.Random.Range(0, 100) < stat.bluecriticalProbability)
            {
                totalDamageWithRandom += totalDamageWithRandom * (stat.bluecriticalIncreaseDamage / 100f);
            }
        }

        if (criticalHit)
        {
            OnCriticalHit?.Invoke();
        }

        return (BigInteger)totalDamageWithRandom;
    }

    public BigInteger CalculateSkillDamage(float skillDamagePercent)
    {
        BigInteger baseDamage = (BigInteger)basicDamage;
        BigInteger weaponDamage = WeaponIncreaseDamage;
        BigInteger skillBuffDamage = SkillIncreaseDamage;

        BigInteger skillDamage = baseDamage + weaponDamage + skillBuffDamage;
        skillDamage = skillDamage * (BigInteger)(skillDamagePercent / 100f);

        float randomMultiplier = UnityEngine.Random.Range(1 - damageRandomVariation, 1 + damageRandomVariation);
        float totalSkillDamageWithRandom = (float)skillDamage * randomMultiplier;

        bool criticalHit = false;

        if (UnityEngine.Random.Range(0, 100) < stat.criticalProbability)
        {
            criticalHit = true;

            totalSkillDamageWithRandom += totalSkillDamageWithRandom * (stat.criticalIncreaseDamage / 100f);

            if (UnityEngine.Random.Range(0, 100) < stat.bluecriticalProbability)
            {
                totalSkillDamageWithRandom += totalSkillDamageWithRandom * (stat.bluecriticalIncreaseDamage / 100f);
            }
        }

        if (criticalHit)
        {
            OnCriticalHit?.Invoke();
        }

        return (BigInteger)totalSkillDamageWithRandom;
    }

    public void ResetDamageMultiplier()
    {
        damageMultiplier = 1.0f;
    }

    private void AddAttackBuff(float percent, float duration)
    {
        attackBuffs.Add((percent, duration));
        StartCoroutine(RemoveAttackBuffAfterDuration(percent, duration));
        UpdateValue();
    }

    private IEnumerator RemoveAttackBuffAfterDuration(float percent, float duration)
    {
        yield return new WaitForSeconds(duration);
        attackBuffs.Remove((percent, duration));
        UpdateValue();
    }

    public BigInteger GetRawTotalDamage()
    {
        return rawTotalDamage;
    }
}