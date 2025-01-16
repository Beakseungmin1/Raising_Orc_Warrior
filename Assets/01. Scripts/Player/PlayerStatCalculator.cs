using System.Numerics;
using UnityEngine;

public class PlayerStatCalculator : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;

    public BigInteger basicHealthRegeneration;
    public BigInteger basicMaxHealth;

    public float basicManaRegeneration;
    public float basicMaxMana;

    private BigInteger currentHealthRegenerationIncreasePercentage = 0;
    private BigInteger currentMaxHealthIncreasePercentage = 0;

    private float currentManaRegenerationIncreasePercentage = 0;
    private float currentMaxManaIncreasePercentage = 0;

    private BigInteger adjustedMaxHealth;
    private BigInteger adjustedHealthRegeneration;

    private float adjustedMaxMana;
    private float adjustedManaRegeneration;

    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;
        equipManager = PlayerObjManager.Instance.Player.EquipManager;
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

        basicManaRegeneration = stat.manaRegeneration;
        basicMaxMana = stat.maxMana;

        currentHealthRegenerationIncreasePercentage = 0;
        currentMaxHealthIncreasePercentage = 0;

        currentManaRegenerationIncreasePercentage = 0;
        currentMaxManaIncreasePercentage = 0;

        if (equipManager.EquippedAccessory != null)
        {
            currentHealthRegenerationIncreasePercentage = (BigInteger)equipManager.EquippedAccessory.EquipHpAndHpRecoveryIncreaseRate;
            currentMaxHealthIncreasePercentage = (BigInteger)equipManager.EquippedAccessory.EquipHpAndHpRecoveryIncreaseRate;
        }

        float totalHpAndHpRecovery = PlayerObjManager.Instance.Player.inventory.GetTotalAccessoryHpAndHpRecovery();

        float totalMpAndMpRecovery = PlayerObjManager.Instance.Player.inventory.GetTotalAccessoryMpAndMpRecovery();

        BigInteger totalHealthIncreasePercentage = currentMaxHealthIncreasePercentage + (BigInteger)totalHpAndHpRecovery;
        BigInteger totalHealthRecoveryPercentage = currentHealthRegenerationIncreasePercentage + (BigInteger)totalHpAndHpRecovery;

        adjustedMaxHealth = basicMaxHealth * (totalHealthIncreasePercentage + 100) * 100 / 10000;
        adjustedHealthRegeneration = basicHealthRegeneration * (totalHealthRecoveryPercentage + 100) * 100 / 10000;

        adjustedMaxMana = basicMaxMana * (1 + totalMpAndMpRecovery / 100);
        adjustedManaRegeneration = basicManaRegeneration * (1 + totalMpAndMpRecovery / 100);

        if (stat.health > adjustedMaxHealth)
        {
            stat.ResetHealth();
        }

        if (stat.mana > adjustedMaxMana)
        {
            stat.ResetMana();
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

    public float GetAdjustedMaxMana()
    {
        return adjustedMaxMana;
    }

    public float GetAdjustedManaRegeneration()
    {
        return adjustedManaRegeneration;
    }
}