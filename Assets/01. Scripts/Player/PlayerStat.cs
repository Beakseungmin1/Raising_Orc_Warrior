using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int level { get; private set; }
    public int exp { get; private set; }
    public int needExp { get; private set; }
    public float attackPower { get; private set; }
    public float health { get; private set; }
    public float maxHealth { get; private set; }
    public float healthRegeneration { get; private set; }
    public float criticalProbability { get; private set; }
    public float criticalIncreaseDamage { get; private set; }
    public float bluecriticalIncreaseDamage { get; private set; }
    public float bluecriticalProbability { get; private set; }
    public float mana { get; private set; }
    public float maxMana { get; private set; }
    public float manaRegeneration { get; private set; }
    public float Avoid { get; private set; }
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
    public float needAttackUpgradeMoney { get; private set; }
    public float needHealthUpgradeMoney { get; private set; }
    public float needHealthRegenerationUpgradeMoney { get; private set; }
    public float needCriticalIncreaseDamageUpgradeMoney { get; private set; }
    public float needCriticalProbabilityUpgradeMoney { get; private set; }
    public float needBlueCriticalIncreaseDamageUpgradeMoney { get; private set; }
    public float needBlueCriticalProbabilityUpgradeMoney { get; private set; }

    public Action UpdateStatUI;

    private void Start()
    {
        // 임시
        SetDefaultStat();
    }


    public void AddExpFromMonsters(Enemy enemy)
    {
        exp += enemy.GiveExp();
    }

    public void LevelUp()
    {
        if (exp >= needExp)
        {
            level++;
            int curNeedExp = needExp;
            exp -= curNeedExp;
            needExp = needExp * 2;
            PlayerLevelInfoUI.Instance.UpdateLevelUI();
        }
        else
        {
            Debug.Log("경험치가 부족합니다");
        }
    }

    public void AttackLevelUp()
    {
        attackLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needAttackUpgradeMoney = attackLevel * 1000; //필요가격 수정예정
        attackPower = attackLevel * 4;
        PlayerStatUpgradeUI.Instance.attackUi.UpdateAttackStatUI();
    }

    public void HealthLevelUp()
    {
        healthLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needHealthUpgradeMoney = healthLevel * 1000;
        maxHealth = healthLevel * 40;
        health += 40;
        PlayerStatUpgradeUI.Instance.healthUi.UpdateHealthStatUI();
    }

    public void HealthRegenerationLevelUp()
    {
        healthRegenerationLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needHealthRegenerationUpgradeMoney = healthRegenerationLevel * 1000;
        healthRegeneration = healthRegenerationLevel * 4;
        PlayerStatUpgradeUI.Instance.healthRegenUi.UpdateHealthRegenerationUI();
    }

    public void CriticalIncreaseDamageLevelUp()
    {
        criticalIncreaseDamageLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needCriticalIncreaseDamageUpgradeMoney = criticalIncreaseDamageLevel * 1000;
        criticalIncreaseDamage = criticalIncreaseDamageLevel;
        PlayerStatUpgradeUI.Instance.criticalDamageUi.UpdateCriticalIncreaseDamageUI();
    }

    public void CriticalProbabilityLevelUp()
    {
        criticalProbabilityLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needCriticalProbabilityUpgradeMoney = criticalProbabilityLevel * 1000;
        criticalProbability = criticalProbabilityLevel * 0.1f;
        PlayerStatUpgradeUI.Instance.criticalProbabilityUi.UpdateCriticalProbabilityUI();
    }

    public void BlueCriticalIncreaseDamageLevelUp()
    {
        bluecriticalIncreaseDamageLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needBlueCriticalIncreaseDamageUpgradeMoney = bluecriticalIncreaseDamageLevel * 1000;
        bluecriticalIncreaseDamage = bluecriticalIncreaseDamageLevel;
        PlayerStatUpgradeUI.Instance.blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
    }

    public void BlueCriticalProbabilityLevelUp()
    {
        bluecriticalProbabilityLevel++;
        //현재 돈에서 니드머니 빼기 기능 추가
        needBlueCriticalProbabilityUpgradeMoney = bluecriticalProbabilityLevel * 1000;
        bluecriticalProbability = bluecriticalProbabilityLevel * 0.1f;
        PlayerStatUpgradeUI.Instance.blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }

    public void SetDefaultStat()
    {
        level = 1;
        exp = 0;
        needExp = 100;
        attackPower = 20;
        maxHealth = 20;
        health = maxHealth;
        healthRegeneration = 0;
        criticalProbability = 0;
        criticalIncreaseDamage = 100;
        maxMana = 20;
        mana = maxMana;
        manaRegeneration = 10;
        Avoid = 0;
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

    public void UseTimelimitBuffSkill(Skill skill)
    {
        float skillValue = skill.BaseData.attackIncreasePercent;
        float skillTime = skill.BaseData.buffDuration;

        StartCoroutine(BuffCoroutine(skillValue, skillTime));
    }

    private IEnumerator BuffCoroutine(float skillValue, float skillTime)
    {
        // 버프 적용
        attackPower += attackPower * (skillValue / 100);

        yield return new WaitForSeconds(skillTime);

        attackPower -= attackPower * (skillValue / 100);
    }

    public void UseHealSkill(Skill skill)
    {
        float skillValue = skill.BaseData.attackIncreasePercent;

        float Healhealth = maxHealth * (skillValue / 100);

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
