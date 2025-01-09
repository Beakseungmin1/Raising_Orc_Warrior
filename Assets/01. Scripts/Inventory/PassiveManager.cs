using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PassiveManager : Singleton<PassiveManager>
{
    private float totalHpRecoveryIncreaseRate = 0f;
    private float totalHpIncreaseRate = 0f;
    private float totalMpRecoveryIncreaseRate = 0f;
    private float totalMpIncreaseRate = 0f;
    private BigInteger totalExtraExpRate = 0;
    private BigInteger totalGoldGainRate = 0;
    private PlayerStat stat;
    private PlayerInventory inventory;
    private PlayerStatCalculator statCalculator;
    private PlayerDamageCalculator damageCalculator;

    private void OnValidate()
    {
        stat = PlayerObjManager.Instance.Player.stat;
        inventory = PlayerObjManager.Instance.Player.inventory;
        statCalculator = PlayerObjManager.Instance.Player.StatCalculator;
        damageCalculator = PlayerObjManager.Instance.Player.DamageCalculator;
    }

    private void Start()
    {
        if (stat == null)
        {
            stat = PlayerObjManager.Instance.Player.stat;
        }

        if (inventory == null)
        {
            inventory = PlayerObjManager.Instance.Player.inventory;
        }

        if (statCalculator == null)
        {
            statCalculator = PlayerObjManager.Instance.Player.StatCalculator;
        }

        if (damageCalculator == null)
        {
            damageCalculator = PlayerObjManager.Instance.Player.DamageCalculator;
        }
    }

    public void UpdateAccessoryEffects()
    {
        totalExtraExpRate = (BigInteger)inventory.GetTotalAccessoryAddEXPRate();
        stat.ChangeExtraExpRate(totalExtraExpRate);
        statCalculator.UpdateValue();
    }

    public void UpdateWeaponEffects()
    {
        totalGoldGainRate = (BigInteger)inventory.GetTotalWeaponGoldGainRate();
        stat.ChangeExtraGoldGainRate(totalGoldGainRate);
        damageCalculator.GetTotalDamage();
    }

    public void TotalEffects()
    {
        UpdateAccessoryEffects();
        UpdateWeaponEffects();
    }
}