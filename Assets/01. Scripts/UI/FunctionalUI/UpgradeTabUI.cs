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
        stat = PlayerobjManager.Instance.Player.stat;
    }

    public void UpdateAttackStatUI()
    {
        if (stat != null)
        {
            upgradeLevelTxt.text = stat.attackLevel.ToString();
            needMoneyTxt.text = stat.needAttackUpgradeMoney.ToString();
            curValueTxt.text = stat.attackPower.ToString();
            upgradeValueTxt.text = (stat.attackPower + 4).ToString();
        }
    }






}
