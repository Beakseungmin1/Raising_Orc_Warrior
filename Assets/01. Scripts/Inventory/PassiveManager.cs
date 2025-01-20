using System.Numerics;

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

    private void Awake()
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
        if (stat == null)
        {
            stat = PlayerObjManager.Instance.Player.stat;
        }

        totalExtraExpRate = (BigInteger)inventory.GetTotalAccessoryAddEXPRate();
        stat.ChangeExtraExpRate(totalExtraExpRate);
        statCalculator.UpdateValue();
    }

    public void UpdateWeaponEffects()
    {
        if (stat == null)
        {
            stat = PlayerObjManager.Instance.Player.stat;
        }

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