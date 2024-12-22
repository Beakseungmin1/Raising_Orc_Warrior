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
    }

    public void ApplySkillEffect(SkillEffect effect)
    {
        if (effect.DamagePercent > 0)
        {
            ApplyDamageMultiplier(effect.DamagePercent);
        }

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
            WeaponIncreaseDamage = (BigInteger)(basicDamage * (equipManager.EquippedWeapon.BaseData.equipAtkIncreaseRate / 100));
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

        TotalDamage = ((BigInteger)basicDamage + WeaponIncreaseDamage + SkillIncreaseDamage) * (BigInteger)damageMultiplier;

        StartCoroutine(ResetMultiplierAfterDelay());

        return TotalDamage;
    }

    private IEnumerator ResetMultiplierAfterDelay()
    {
        yield return null;
        ResetDamageMultiplier();
    }

    private void ApplyDamageMultiplier(float multiplierPercent)
    {
        damageMultiplier = 1 + (multiplierPercent / 100f);
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