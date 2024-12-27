using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int level { get; private set; }
    public BigInteger exp { get; private set; }
    public BigInteger needExp { get; private set; }
    public float attackPower { get; private set; }
    public BigInteger health { get; private set; }
    public BigInteger maxHealth { get; private set; }
    public BigInteger healthRegeneration { get; private set; }
    public float criticalProbability { get; private set; }
    public float criticalIncreaseDamage { get; private set; }
    public float bluecriticalIncreaseDamage { get; private set; }
    public float bluecriticalProbability { get; private set; }
    public float mana { get; private set; }
    public float maxMana { get; private set; }
    public float manaRegeneration { get; private set; }
    public float hitLate { get; private set; }
    public float avoid { get; private set; }
    public float extraGoldGainRate { get; private set; }
    public float extraExpRate { get; private set; }
    public float attackSpeed { get; private set; }
    public float normalMonsterIncreaseDamage { get; private set; }
    public float bossMonsterIncreaseDamage { get; private set; }
    public int attackLevel { get; private set; }
    public int healthLevel { get; private set; }
    public int healthRegenerationLevel { get; private set; }
    public int criticalIncreaseDamageLevel { get; private set; }
    public int criticalProbabilityLevel { get; private set; }
    public int bluecriticalIncreaseDamageLevel { get; private set; }
    public int bluecriticalProbabilityLevel { get; private set; }
    public BigInteger needAttackUpgradeMoney { get; private set; }
    public BigInteger needHealthUpgradeMoney { get; private set; }
    public BigInteger needHealthRegenerationUpgradeMoney { get; private set; }
    public BigInteger needCriticalIncreaseDamageUpgradeMoney { get; private set; }
    public BigInteger needCriticalProbabilityUpgradeMoney { get; private set; }
    public BigInteger needBlueCriticalIncreaseDamageUpgradeMoney { get; private set; }
    public BigInteger needBlueCriticalProbabilityUpgradeMoney { get; private set; }

    public Action UpdateLevelStatUI;

    public Action UpdateUserInformationUI;

    private void Start()
    {
        // 임시
        SetDefaultStat();
    }

    public void AddExpFromMonsters(IEnemy enemy)
    {
        exp += enemy.GiveExp();
    }

    public void AddExp(BigInteger getExp)
    {
        exp += getExp;
    }

    public void decreaseHp(BigInteger damage)
    {
        health -= damage;
    }


    public void LevelUp()
    {
        if (exp >= needExp)
        {
            level++;
            BigInteger curNeedExp = needExp;
            exp -= curNeedExp;
            needExp = needExp * 2;
            UpdateLevelStatUI.Invoke();
            SoundManager.Instance.PlaySFX(SFXType.LevelUp);
        }
        else
        {
            Debug.Log("경험치가 부족합니다");
        }
    }

    public void AttackLevelUp()
    {
        needAttackUpgradeMoney = attackLevel * 1000; //필요가격 수정예정

        if (CurrencyManager.Instance.GetGold() >= needAttackUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needAttackUpgradeMoney);
            attackLevel++;
            attackPower = 20 + (attackLevel * 4);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void HealthLevelUp()
    {
        //현재 돈에서 니드머니 빼기 기능 추가
        needHealthUpgradeMoney = healthLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needHealthUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needHealthUpgradeMoney);
            healthLevel++;
            maxHealth = 200 + (healthLevel * 40);
            health += 40;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void HealthRegenerationLevelUp()
    {
        needHealthRegenerationUpgradeMoney = healthRegenerationLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needHealthRegenerationUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needHealthRegenerationUpgradeMoney);
            healthRegenerationLevel++;
            healthRegeneration = healthRegenerationLevel * 4;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void CriticalIncreaseDamageLevelUp()
    {
        needCriticalIncreaseDamageUpgradeMoney = criticalIncreaseDamageLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needCriticalIncreaseDamageUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needCriticalIncreaseDamageUpgradeMoney);
            criticalIncreaseDamageLevel++;
            criticalIncreaseDamage = criticalIncreaseDamageLevel;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void CriticalProbabilityLevelUp()
    {
        needCriticalProbabilityUpgradeMoney = criticalProbabilityLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needCriticalProbabilityUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needCriticalProbabilityUpgradeMoney);
            criticalProbability = criticalProbabilityLevel * 0.1f;
            criticalProbabilityLevel++;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void BlueCriticalIncreaseDamageLevelUp()
    {
        needBlueCriticalIncreaseDamageUpgradeMoney = bluecriticalIncreaseDamageLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needBlueCriticalIncreaseDamageUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needBlueCriticalIncreaseDamageUpgradeMoney);
            bluecriticalIncreaseDamage = bluecriticalIncreaseDamageLevel;
            bluecriticalIncreaseDamageLevel++;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void BlueCriticalProbabilityLevelUp()
    {
        needBlueCriticalProbabilityUpgradeMoney = bluecriticalProbabilityLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needBlueCriticalProbabilityUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needBlueCriticalProbabilityUpgradeMoney);
            bluecriticalProbability = bluecriticalProbabilityLevel * 0.1f;
            bluecriticalProbabilityLevel++;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void SetDefaultStat()
    {
        level = 1;
        exp = 0;
        needExp = 100;
        attackPower = 20;
        maxHealth = 200;
        health = maxHealth;
        healthRegeneration = 0;
        criticalProbability = 0;
        criticalIncreaseDamage = 100;
        maxMana = 10000;
        mana = maxMana;
        manaRegeneration = 10;
        hitLate = 0;
        avoid = 0;
        extraGoldGainRate = 0;
        extraExpRate = 0;
        attackSpeed = 0;
        normalMonsterIncreaseDamage = 0;
        bossMonsterIncreaseDamage = 0;
        attackLevel = 1;
        healthLevel = 1;
        healthRegenerationLevel = 1;
        criticalIncreaseDamageLevel = 1;
        criticalProbabilityLevel = 1;
        bluecriticalIncreaseDamageLevel = 1;
        bluecriticalProbabilityLevel = 1;
        needAttackUpgradeMoney = 1000;
        needHealthUpgradeMoney = 1000;
        needHealthRegenerationUpgradeMoney = 1000;
        needCriticalIncreaseDamageUpgradeMoney = 1000;
        needCriticalProbabilityUpgradeMoney = 1000;
        needBlueCriticalIncreaseDamageUpgradeMoney = 1000;
        needBlueCriticalProbabilityUpgradeMoney = 1000;
    }

    public float GetMana()
    {
        return mana;
    }

    public void reduceMana(float value)
    {
        mana -= value;
    }

    public void HoldIncreaseWeaponValue(Weapon weapon)
    {
        //float holdWeaponPower = attackPower * (weapon.atkIncreaseRate / 30 / 100);
        //attackPower += holdWeaponPower;

        //float holdIncreaseCriticalPower = criticalIncreaseDamage * (weapon.criticalDamageBonus / 100);
        //criticalIncreaseDamage += holdIncreaseCriticalPower;

        //float holdIncreaseGoldGain = extraGoldGainRate * (weapon.increaseGoldGainRate / 100);
        //extraGoldGainRate += holdIncreaseGoldGain;
    }

    public void UseTimelimitBuffSkill(BaseSkill skill)
    {
        float skillValue = skill.SkillData.attackIncreasePercent;
        float skillTime = skill.SkillData.buffDuration;

        StartCoroutine(BuffCoroutine(skillValue, skillTime));
    }

    private IEnumerator BuffCoroutine(float skillValue, float skillTime)
    {
        // 버프 적용
        attackPower += attackPower * (skillValue / 100);

        yield return new WaitForSeconds(skillTime);

        attackPower -= attackPower * (skillValue / 100);
    }

    public void UseHealSkill(BaseSkill skill)
    {
        BigInteger skillValue = (BigInteger)skill.SkillData.attackIncreasePercent;

        BigInteger Healhealth = maxHealth * (skillValue / 100);

        if (health + Healhealth <= maxHealth)
        {
            health += Healhealth;
        }
        else
        {
            health = maxHealth;
        }

    }

}
