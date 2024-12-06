using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCalculator : MonoBehaviour
{

    public float TotalDamage;
    public float basicDamage;
    public float WeaponIncreaseDamage;
    public float SkillIncreaseDamage;



    private void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        basicDamage = PlayerobjManager.Instance.Player.stat.attackPower;
        //WeaponIncreaseDamage = basicDamage * (PlayerobjManager.Instance.Player.curWeapon.BaseData.equipAtkIncreaseRate / 100);

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
