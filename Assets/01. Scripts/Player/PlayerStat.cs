using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float level;
    private float exp;
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

    public void SetDefaultStat()
    {
        level = 1;
        exp = 0;
        attackPower = 20;
        maxHealth = 20;
        healthRegeneration = 0;
        criticalProbability = 0;
        criticalIncreaseDamage = 100;
        maxMana = 20;
        manaRegeneration = 10;
        Avoid = 0;
        extraGoldGainRate = 0;
        extraExpRate = 0;
        attackSpeed = 0;
        normalMonsterIncreaseDamage = 0;
        bossMonsterIncreaseDamage = 0;
    }


    public float GetDamage()
    {
        return attackPower;
    }

    public void EquipWeaponValue(Weapon weapon)
    {
        //float equipWeaponPower = attackPower * (weapon.atkIncreaseRate / 100);
        //attackPower += equipWeaponPower;

    }

    public void unEquipWeaponValue(Weapon weapon)
    {
        //float equipWeaponPower = attackPower * (weapon.atkIncreaseRate / 100);
        //attackPower -= equipWeaponPower;

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
