using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCalculator : MonoBehaviour
{
    public float TotalDamage;
    public float basicDamage = 10;
    public float WeaponDamage;


    public float GetTotalDamage()
    {
        TotalDamage = basicDamage + WeaponDamage;

        return TotalDamage;

    }



}
