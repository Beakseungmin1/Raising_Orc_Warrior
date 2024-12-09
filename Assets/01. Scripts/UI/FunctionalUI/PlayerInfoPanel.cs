using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    private PlayerStat stat;

    [SerializeField] private TextMeshProUGUI playerLevelTxt;
    [SerializeField] private TextMeshProUGUI playerNameTxt;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI equipWeaponNameTxt;
    [SerializeField] private Image equipWeaponImage;
    [SerializeField] private TextMeshProUGUI equipAccNameTxt;
    [SerializeField] private Image equipAccImage;
    [SerializeField] private TextMeshProUGUI attackPowerTxt;
    [SerializeField] private TextMeshProUGUI maxHealthTxt;
    [SerializeField] private TextMeshProUGUI healthRegenTxt;
    [SerializeField] private TextMeshProUGUI criticalProbabilityTxt;
    [SerializeField] private TextMeshProUGUI criticalIncreaseDamageTxt;
    [SerializeField] private TextMeshProUGUI maxManaTxt;
    [SerializeField] private TextMeshProUGUI manaRegenTxt;
    [SerializeField] private TextMeshProUGUI hitLateTxt;
    [SerializeField] private TextMeshProUGUI avoidTxt;
    [SerializeField] private TextMeshProUGUI extraGoldGainRateTxt;
    [SerializeField] private TextMeshProUGUI extraExpRateTxt;

    private void Awake()
    {
        stat = PlayerobjManager.Instance.Player.stat;

        stat.UpdateUserInformationUI += UpdatePlayerInfoUI;
    }


    public void UpdatePlayerInfoUI()
    {
        if (stat != null)
        {
            playerLevelTxt.text = "Lv." + stat.level.ToString();

            // 이름 추가
            // 이미지 추가
            // 장착중인 무기 이름추가
            // 장착중인 무기 이미지추가
            // 장착중인 악세사리 이름추가
            // 장착중인 악세사리 이미지추가

            attackPowerTxt.text = stat.attackPower.ToString();
            maxHealthTxt.text = stat.maxHealth.ToString();
            healthRegenTxt.text = stat.healthRegeneration.ToString();
            criticalProbabilityTxt.text = stat.criticalProbability.ToString() + "%";
            criticalIncreaseDamageTxt.text = stat.criticalIncreaseDamage.ToString() + "%";
            maxManaTxt.text = stat.maxMana.ToString();
            manaRegenTxt.text = stat.manaRegeneration.ToString();
            hitLateTxt.text = stat.hitLate.ToString();
            avoidTxt.text = stat.avoid.ToString();
            extraGoldGainRateTxt.text = stat.extraGoldGainRate.ToString() + "%";
            extraExpRateTxt.text= stat.extraExpRate.ToString() + "%";

        }
        else
        {
            return;
        }
    }







}
