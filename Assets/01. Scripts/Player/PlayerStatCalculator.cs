using System.Numerics;
using UnityEngine;

public class PlayerStatCalculator : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;

    public BigInteger basicHealthRegeneration;
    public BigInteger basicMaxHealth;

    private BigInteger currentHealthRegenerationIncreasePercentage = 0;
    private BigInteger currentMaxHealthIncreasePercentage = 0;

    private BigInteger adjustedMaxHealth;
    private BigInteger adjustedHealthRegeneration;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
        equipManager = GetComponent<EquipManager>();
        stat.SetDefaultStat();
        UpdateValue();

        stat.OnStatChange += UpdateValue;
    }

    public void UpdateValue()
    {
        if (stat == null)
        {
            return;
        }

        basicHealthRegeneration = stat.healthRegeneration;
        basicMaxHealth = stat.maxHealth;

        currentHealthRegenerationIncreasePercentage = 0;
        currentMaxHealthIncreasePercentage = 0;

        if (equipManager.EquippedAccessory != null)
        {
            currentHealthRegenerationIncreasePercentage = (BigInteger)equipManager.EquippedAccessory.EquipHpAndHpRecoveryIncreaseRate;
            currentMaxHealthIncreasePercentage = (BigInteger)equipManager.EquippedAccessory.EquipHpAndHpRecoveryIncreaseRate;
        }

        adjustedMaxHealth = basicMaxHealth * (1 + currentMaxHealthIncreasePercentage / 100);
        adjustedHealthRegeneration = basicHealthRegeneration * (1 + currentHealthRegenerationIncreasePercentage / 100);

        if (stat.health > adjustedMaxHealth)
        {
            stat.ResetHealth();
        }
    }

    public BigInteger GetAdjustedMaxHealth()
    {
        return adjustedMaxHealth;
    }

    public BigInteger GetAdjustedHealthRegeneration()
    {
        return adjustedHealthRegeneration;
    }
}