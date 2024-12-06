using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public int level { get; private set; }
    public float exp { get; private set; }
    public float needExp { get; private set; }
    public float attackPower { get; private set; }
    public float health { get; private set; }
    public float maxHealth { get; private set; }
    public float healthRegeneration { get; private set; }
    public float criticalProbability { get; private set; }
    public float criticalIncreaseDamage { get; private set; }
    public float mana { get; private set; }
    public float maxMana { get; private set; }
    public float manaRegeneration { get; private set; }
    public float Avoid { get; private set; }
    public float extraGoldGainRate { get; private set; }
    public float extraExpRate { get; private set; }
    public float attackSpeed { get; private set; }
    public float normalMonsterIncreaseDamage { get; private set; }
    public float bossMonsterIncreaseDamage { get; private set; }
    public int attackLevel {  get; private set; }
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

    private void Start()
    {
        // 임시
        SetDefaultStat();
        PlayerLevelInfoUI.Instance.UpdateLevelUI();
    }

    public void AddExpFromMonsters(EnemyBattle enemy)
    {
        exp += enemy.giveExp;
    }

    public void LevelUp()
    {
        if (exp >= needExp)
        {
            level++;
            float curNeedExp = needExp;
            exp -= curNeedExp;
            needExp = needExp * 2;
            PlayerLevelInfoUI.Instance.UpdateLevelUI();
        }
        else
        {
            Debug.Log("경험치가 부족합니다");
        }
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
    }

    public float GetMana()
    {
        return mana;
    }

    public void reduceMana(float value)
    {
        mana -= value;
    }



    public float GetDamage()
    {
        return attackPower;
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

    public int GetLevel()
    {
        return level;
    }
    public float GetExp()
    {
        return exp;
    }
    public float GetNeedExp()
    {
        return needExp;
    }


}
