using System.Collections;
using System.Collections.Generic;
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
            upgradeLevelTxt.text = "Lv." + (stat.attackLevel + 1).ToString();
            needMoneyTxt.text = stat.needAttackUpgradeMoney.ToString();
            curValueTxt.text = stat.attackPower.ToString();
            upgradeValueTxt.text = (stat.attackPower + 4).ToString();
        }
    }

    public void UpdateHealthStatUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.healthLevel + 1).ToString();
            needMoneyTxt.text = stat.needHealthUpgradeMoney.ToString();
            curValueTxt.text = stat.maxHealth.ToString();
            upgradeValueTxt.text = (stat.maxHealth + 40).ToString();
        }
    }

    public void UpdateHealthRegenerationUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.healthRegenerationLevel + 1).ToString();
            needMoneyTxt.text = stat.needHealthRegenerationUpgradeMoney.ToString();
            curValueTxt.text = stat.healthRegeneration.ToString();
            upgradeValueTxt.text = (stat.healthRegeneration + 4).ToString();
        }
    }
    public void UpdateCriticalProbabilityUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + stat.criticalProbabilityLevel.ToString();
            needMoneyTxt.text = stat.needCriticalProbabilityUpgradeMoney.ToString();
            curValueTxt.text = stat.criticalProbability.ToString() + "%";
            upgradeValueTxt.text = (stat.criticalProbability + 0.1f).ToString() + "%";
        }
    }
    public void UpdateCriticalIncreaseDamageUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = "Lv." + (stat.criticalIncreaseDamageLevel + 1).ToString();
            needMoneyTxt.text = stat.needCriticalIncreaseDamageUpgradeMoney.ToString();
            curValueTxt.text = stat.criticalIncreaseDamage.ToString() + "%";
            upgradeValueTxt.text = (stat.criticalIncreaseDamage + 1).ToString() + "%";
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
