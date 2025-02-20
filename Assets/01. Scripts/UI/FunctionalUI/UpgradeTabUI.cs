using System;
using TMPro;
using UnityEngine;

public class UpgradeTabUI : MonoBehaviour
{
    private PlayerStat stat;

    [SerializeField] private TextMeshProUGUI upgradeLevelTxt;
    [SerializeField] private TextMeshProUGUI needMoneyTxt;
    [SerializeField] private TextMeshProUGUI curValueTxt;
    [SerializeField] private TextMeshProUGUI upgradeValueTxt;
    private void Awake()
    {
        stat = PlayerObjManager.Instance.Player.stat;
    }

    public void UpdateAttackStatUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.attackLevel).ToString();
            needMoneyTxt.text = stat.needAttackUpgradeMoney.ToString();
            curValueTxt.text = stat.attackPower.ToString();
            if (stat.statUpgradeMultiplier == 0)
            {
                upgradeValueTxt.text = (stat.attackPower + 4).ToString();
            }
            else if (stat.statUpgradeMultiplier == 1)
            {
                upgradeValueTxt.text = (stat.attackPower + 40).ToString();
            }
            else
            {
                upgradeValueTxt.text = (stat.attackPower + 400).ToString();
            }
        }
    }

    public void UpdateHealthStatUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.healthLevel).ToString();
            needMoneyTxt.text = stat.needHealthUpgradeMoney.ToString();
            curValueTxt.text = stat.maxHealth.ToString();
            if (stat.statUpgradeMultiplier == 0)
            {
                upgradeValueTxt.text = (stat.maxHealth + 4).ToString();
            }
            else if (stat.statUpgradeMultiplier == 1)
            {
                upgradeValueTxt.text = (stat.maxHealth + 40).ToString();
            }
            else
            {
                upgradeValueTxt.text = (stat.maxHealth + 400).ToString();
            }
        }
    }

    public void UpdateHealthRegenerationUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.healthRegenerationLevel).ToString();
            needMoneyTxt.text = stat.needHealthRegenerationUpgradeMoney.ToString();
            curValueTxt.text = stat.healthRegeneration.ToString();
            if (stat.statUpgradeMultiplier == 0)
            {
                upgradeValueTxt.text = (stat.healthRegeneration + 4).ToString();
            }
            else if (stat.statUpgradeMultiplier == 1)
            {
                upgradeValueTxt.text = (stat.healthRegeneration + 40).ToString();
            }
            else
            {
                upgradeValueTxt.text = (stat.healthRegeneration + 400).ToString();
            }
        }
    }
    public void UpdateCriticalProbabilityUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.criticalProbabilityLevel).ToString();
            needMoneyTxt.text = stat.needCriticalProbabilityUpgradeMoney.ToString();
            curValueTxt.text = Math.Round(stat.criticalProbability, 1).ToString() + "%";
            if (stat.statUpgradeMultiplier == 0)
            {
                upgradeValueTxt.text = Math.Round(stat.criticalProbability + 0.1f, 1).ToString() + "%";
            }
            else if (stat.statUpgradeMultiplier == 1)
            {
                upgradeValueTxt.text = Math.Round(stat.criticalProbability + 1f, 1).ToString() + "%";
            }
            else
            {
                upgradeValueTxt.text = Math.Round(stat.criticalProbability + 10f, 1).ToString() + "%";
            }
        }
    }
    public void UpdateCriticalIncreaseDamageUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.criticalIncreaseDamageLevel).ToString();
            needMoneyTxt.text = stat.needCriticalIncreaseDamageUpgradeMoney.ToString();
            curValueTxt.text = stat.criticalIncreaseDamage.ToString() + "%";
            if (stat.statUpgradeMultiplier == 0)
            {
                upgradeValueTxt.text = (stat.criticalIncreaseDamage + 1).ToString() + "%";
            }
            else if (stat.statUpgradeMultiplier == 1)
            {
                upgradeValueTxt.text = (stat.criticalIncreaseDamage + 10).ToString() + "%";
            }
            else
            {
                upgradeValueTxt.text = (stat.criticalIncreaseDamage + 100).ToString() + "%";
            }
        }
    }
    public void UpdateblueCriticalIncreaseDamageStatUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + stat.bluecriticalIncreaseDamageLevel.ToString();
            needMoneyTxt.text = stat.needBlueCriticalIncreaseDamageUpgradeMoney.ToString();
            curValueTxt.text = stat.bluecriticalIncreaseDamage.ToString() + "%";
            upgradeValueTxt.text = (stat.bluecriticalIncreaseDamage + 1).ToString() + "%";
        }
    }
    public void UpdateblueCriticalProbabilityStatUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + stat.bluecriticalProbabilityLevel.ToString();
            needMoneyTxt.text = stat.needBlueCriticalProbabilityUpgradeMoney.ToString();
            curValueTxt.text = stat.bluecriticalProbability.ToString() + "%";
            upgradeValueTxt.text = (stat.bluecriticalProbability + 0.1f).ToString() + "%";
        }
    }
}
