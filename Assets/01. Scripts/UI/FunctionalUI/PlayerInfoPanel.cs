using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    private PlayerStat stat;
    private EquipManager equipManager;

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

        stat.UpdateUserInformationUI += UpdatePlayerInfoUI;
        equipManager.OnEquippedChanged += UpdatePlayerInfoUI;
    }

    public void UpdatePlayerInfoUI()
    {
        if (stat != null)
        {
            playerLevelTxt.text = "Lv." + stat.level.ToString();
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
            extraExpRateTxt.text = stat.extraExpRate.ToString() + "%";
        }

        if (equipManager != null)
        {
            if (equipManager.EquippedWeapon != null)
            {
                equipWeaponNameTxt.text = equipManager.EquippedWeapon.BaseData.itemName;
                equipWeaponImage.sprite = equipManager.EquippedWeapon.BaseData.icon;
            }
            else
            {
                equipWeaponNameTxt.text = null;
                equipWeaponImage.sprite = null;
            }

            if (equipManager.EquippedAccessory != null)
            {
                equipAccNameTxt.text = equipManager.EquippedAccessory.BaseData.itemName;
                equipAccImage.sprite = equipManager.EquippedAccessory.BaseData.icon;
            }
            else
            {
                equipAccNameTxt.text = null;
                equipAccImage.sprite = null;
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