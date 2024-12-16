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



    private void Start()
    {
        stat = GetComponent<PlayerStat>();
        equipManager = GetComponent<EquipManager>();
    }

    public void UpdateValue()
    {
        basicDamage = stat.attackPower;

        if (equipManager.EquippedWeapon != null)
        {
            WeaponIncreaseDamage = (BigInteger)(basicDamage * (equipManager.EquippedWeapon.BaseData.equipAtkIncreaseRate / 100));
        }
        
        

        foreach (var skill in PlayerObjManager.Instance.Player.PlayerBattle.activeBuffSkills)
        {
            SkillIncreaseDamage += (BigInteger)(basicDamage * (skill.BaseData.attackIncreasePercent / 100));
        }

    }


    public BigInteger GetTotalDamage()
    {
        UpdateValue();
        TotalDamage = ((BigInteger)basicDamage + WeaponIncreaseDamage + SkillIncreaseDamage);

        return TotalDamage;

    }



}
