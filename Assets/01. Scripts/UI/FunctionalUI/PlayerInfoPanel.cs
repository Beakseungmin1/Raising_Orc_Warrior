using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;
    private PlayerDamageCalculator playerDamageCalculator;
    private PlayerStatCalculator playerStatCalculator;

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
        stat = PlayerObjManager.Instance.Player.stat;
        equipManager = PlayerObjManager.Instance.Player.EquipManager;
        playerDamageCalculator = PlayerObjManager.Instance.Player.DamageCalculator;
        playerStatCalculator = PlayerObjManager.Instance.Player.StatCalculator;

        stat.UpdateUserInformationUI += UpdatePlayerInfoUI;
        equipManager.OnEquippedChanged += UpdatePlayerInfoUI;
    }

    public void UpdatePlayerInfoUI()
    {
        if (stat != null)
        {
            playerLevelTxt.text = "Lv." + stat.level+1.ToString();
            attackPowerTxt.text = playerDamageCalculator.GetRawTotalDamage().ToString();
            maxHealthTxt.text = playerStatCalculator.GetAdjustedMaxHealth().ToString();
            healthRegenTxt.text = playerStatCalculator.GetAdjustedHealthRegeneration().ToString();
            criticalProbabilityTxt.text = stat.criticalProbability.ToString() + "%";
            criticalIncreaseDamageTxt.text = playerDamageCalculator.GetTotalCriticalDamageBonus().ToString() + "%";
            maxManaTxt.text = playerStatCalculator.GetAdjustedMaxMana().ToString();
            manaRegenTxt.text = playerStatCalculator.GetAdjustedManaRegeneration().ToString();
            hitLateTxt.text = stat.hitLate.ToString();
            avoidTxt.text = stat.avoid.ToString();
            extraGoldGainRateTxt.text = stat.extraGoldGainRate.ToString() + "%";
            extraExpRateTxt.text = stat.extraExpRate.ToString() + "%";
        }

        if (equipManager != null)
        {
            if (equipManager.EquippedWeapon != null)
            {
                equipWeaponNameTxt.text = equipManager.EquippedWeapon.BaseData.itemName;
                equipWeaponImage.sprite = equipManager.EquippedWeapon.BaseData.icon;
                equipWeaponImage.color = Color.white;
            }
            else
            {
                equipWeaponNameTxt.text = null;
                equipWeaponImage.sprite = null;
                equipWeaponImage.color = new Color32(98, 98, 98, 255);
            }

            if (equipManager.EquippedAccessory != null)
            {
                equipAccNameTxt.text = equipManager.EquippedAccessory.BaseData.itemName;
                equipAccImage.sprite = equipManager.EquippedAccessory.BaseData.icon;
                equipAccImage.color = Color.white;
            }
            else
            {
                equipAccNameTxt.text = null;
                equipAccImage.sprite = null;
                equipAccImage.color = new Color32(98, 98, 98, 255);
            }
        }
    }

    private void OnDestroy()
    {
        if (stat != null)
            stat.UpdateUserInformationUI -= UpdatePlayerInfoUI;

        if (equipManager != null)
            equipManager.OnEquippedChanged -= UpdatePlayerInfoUI;
    }
}