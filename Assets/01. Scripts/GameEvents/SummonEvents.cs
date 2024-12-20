using System;
using UnityEngine;

public class SummonEvents
{
    public event Action<int> onWeaponSummoned;

    public void WeaponSummoned(int summonCount)
    {
        if (onWeaponSummoned != null)
        {
            onWeaponSummoned(summonCount);
        }
    }

    public event Action<int> onSkillSummoned;

    public void SkillSummoned(int summonCount)
    {
        if (onSkillSummoned != null)
        {
            onSkillSummoned(summonCount);
        }
    }

    public event Action<int> onAccessorySummoned;

    public void AccessorySummoned(int summonCount)
    {
        if (onAccessorySummoned != null)
        {
            onAccessorySummoned(summonCount);
        }
    }

}
