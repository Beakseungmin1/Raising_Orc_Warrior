using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCalculator : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;




    public float TotalDamage;
    public float basicDamage;
    public float WeaponIncreaseDamage;
    public float SkillIncreaseDamage;



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
            WeaponIncreaseDamage = basicDamage * (equipManager.EquippedWeapon.BaseData.equipAtkIncreaseRate / 100);
        }
        

        foreach (var skill in PlayerobjManager.Instance.Player.PlayerBattle.activeBuffSkills)
        {
            SkillIncreaseDamage += basicDamage * (skill.BaseData.attackIncreasePercent / 100);
        }

    }


    public float GetTotalDamage()
    {
        UpdateValue();
        TotalDamage = basicDamage + WeaponIncreaseDamage + SkillIncreaseDamage;

        return TotalDamage;

    }



}
