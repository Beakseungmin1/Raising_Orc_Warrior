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

    private const float damageRandomVariation = 0.1f;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
        equipManager = GetComponent<EquipManager>();
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

        BigInteger totalDamage = baseDamage + weaponDamage;

        float randomMultiplier = Random.Range(1 - damageRandomVariation, 1 + damageRandomVariation);
        float totalDamageWithRandom = (float)totalDamage * randomMultiplier;

        if (Random.Range(0, 100) < stat.criticalProbability)
        {
            totalDamageWithRandom += totalDamageWithRandom * (stat.criticalIncreaseDamage / 100f);

            if (Random.Range(0, 100) < stat.bluecriticalProbability)
            {
                totalDamageWithRandom += totalDamageWithRandom * (stat.bluecriticalIncreaseDamage / 100f);
            }
        }

        return (BigInteger)totalDamageWithRandom;
    }

    public BigInteger CalculateSkillDamage(float skillDamagePercent)
    {
        BigInteger baseDamage = (BigInteger)basicDamage;
        BigInteger weaponDamage = WeaponIncreaseDamage;

        BigInteger skillDamage = baseDamage + weaponDamage;
        skillDamage = skillDamage * (BigInteger)(skillDamagePercent / 100f);

        float randomMultiplier = Random.Range(1 - damageRandomVariation, 1 + damageRandomVariation);
        float totalSkillDamageWithRandom = (float)skillDamage * randomMultiplier;

        if (Random.Range(0, 100) < stat.criticalProbability)
        {
            totalSkillDamageWithRandom += totalSkillDamageWithRandom * (stat.criticalIncreaseDamage / 100f);

            if (Random.Range(0, 100) < stat.bluecriticalProbability)
            {
                totalSkillDamageWithRandom += totalSkillDamageWithRandom * (stat.bluecriticalIncreaseDamage / 100f);
            }
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
}