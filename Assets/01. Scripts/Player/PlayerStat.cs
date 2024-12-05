using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private int level;
    private float exp;
    private float needExp;
    private float attackPower;
    private float health;
    private float maxHealth;
    private float healthRegeneration;
    private float criticalProbability;
    private float criticalIncreaseDamage;
    private float mana;
    private float maxMana;
    private float manaRegeneration;
    private float Avoid;
    private float extraGoldGainRate;
    private float extraExpRate;
    private float attackSpeed;
    private float normalMonsterIncreaseDamage;
    private float bossMonsterIncreaseDamage;
    private int attackLevel;
    private int healthLevel;
    private int healthRegenerationLevel;
    private int criticalIncreaseDamageLevel;
    private int criticalProbabilityLevel;
    private int bluecriticalIncreaseDamageLevel;
    private int bluecriticalProbabilityLevel;

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

    




}
