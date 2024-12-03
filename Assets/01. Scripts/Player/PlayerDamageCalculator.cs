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
        basicDamage = PlayerobjManager.Instance.Player.stat.GetDamage();
        //WeaponIncreaseDamage = basicDamage * (PlayerobjManager.Instance.Player.curWeapon.BaseData.equipAtkIncreaseRate / 100);


    }


    public float GetTotalDamage()
    {
        TotalDamage = basicDamage + WeaponIncreaseDamage;

        return TotalDamage;

    }



}
