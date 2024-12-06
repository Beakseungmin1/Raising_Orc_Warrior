using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUpgradePopupUI : UIBase
{
    [Header("Equipment Icon Area")]
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI currentAmountTxt;
    [SerializeField] private TextMeshProUGUI neededAmountTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;
    [SerializeField] private Slider progressSlider;

    [Header("Equipment Effect Area")]
    [SerializeField] private TextMeshProUGUI equipEffectTypeTxt;
    [SerializeField] private TextMeshProUGUI equipEffectValueTxt;

    [Header("Possess Effect Area")]
    [SerializeField] private GameObject possessEffectRow1;
    [SerializeField] private TextMeshProUGUI possessEffect1TypeTxt;
    [SerializeField] private TextMeshProUGUI possessEffect1ValueTxt;
    [SerializeField] private GameObject possessEffectRow2;
    [SerializeField] private TextMeshProUGUI possessEffect2TypeTxt;
    [SerializeField] private TextMeshProUGUI possessEffect2ValueTxt;
    [SerializeField] private GameObject possessEffectRow3;
    [SerializeField] private TextMeshProUGUI possessEffect3TypeTxt;
    [SerializeField] private TextMeshProUGUI possessEffect3ValueTxt;

    [Header("Upgrade Info")]
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private TextMeshProUGUI upgradeCostTxt;
    [SerializeField] private TextMeshProUGUI curCubeAmountTxt;
    [SerializeField] private Image curCubeIcon;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button exitBtn;

    private Weapon currentWeapon;

    private void Start()
    {
        exitBtn.onClick.AddListener(ClosePopup);
        upgradeBtn.onClick.AddListener(UpgradeWeapon);
        equipBtn.onClick.AddListener(EquipWeapon);
    }

    public void SetWeaponData(Weapon weapon)
    {
        if (weapon == null || weapon.BaseData == null)
        {
            Debug.LogError("[WeaponUpgradePopupUI] Weapon 데이터가 유효하지 않습니다.");
            return;
        }

        currentWeapon = weapon;

        WeaponDataSO weaponData = weapon.BaseData;

        // Equipment Icon Area
        equipmentIcon.sprite = weaponData.icon;
        nameTxt.text = weaponData.itemName;
        gradeTxt.text = $"[{weaponData.grade}]";
        gradeTxt.color = weaponData.gradeColor;
        currentAmountTxt.text = weapon.StackCount.ToString();
        neededAmountTxt.text = weaponData.requiredCurrencyForUpgrade.ToString();
        progressSlider.value = (float)weapon.StackCount / weaponData.requiredCurrencyForUpgrade;

        // Equipment Effect Area (장착 효과)
        equipEffectTypeTxt.text = "공격력 증가율";
        equipEffectValueTxt.text = $"{weaponData.equipAtkIncreaseRate}%";

        // Possess Effect Area (보유 효과)
        if (weaponData.equipAtkIncreaseRate > 0)
        {
            possessEffectRow1.SetActive(true);
            possessEffect1TypeTxt.text = "공격력 증가율";
            possessEffect1ValueTxt.text = $"{weaponData.equipAtkIncreaseRate}%";
        }
        else
        {
            possessEffectRow1.SetActive(false);
        }

        // 치명타 데미지 증가
        if (weaponData.passiveCriticalDamageBonus > 0)
        {
            possessEffectRow2.SetActive(true);
            possessEffect2TypeTxt.text = "치명타 데미지 증가";
            possessEffect2ValueTxt.text = $"{weaponData.passiveCriticalDamageBonus}%";
        }
        else
        {
            possessEffectRow2.SetActive(false);
        }

        // 골드 획득량 증가율
        if (weaponData.passiveGoldGainRate > 0)
        {
            possessEffectRow3.SetActive(true);
            possessEffect3TypeTxt.text = "골드 획득량 증가율";
            possessEffect3ValueTxt.text = $"{weaponData.passiveGoldGainRate}%";
        }
        else
        {
            possessEffectRow3.SetActive(false);
        }

        // Upgrade Info
        upgradeCostTxt.text = weaponData.requiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = weaponData.currencyIcon;
    }

    private void UpgradeWeapon()
    {
        if (currentWeapon == null || !currentWeapon.CanEnhance())
        {
            Debug.LogWarning("[WeaponUpgradePopupUI] 강화 조건을 충족하지 못했습니다.");
            return;
        }

        bool success = currentWeapon.Enhance();
        if (success)
        {
            SetWeaponData(currentWeapon);
            Debug.Log($"[WeaponUpgradePopupUI] 무기 {currentWeapon.BaseData.itemName} 강화 성공!");
        }
        else
        {
            Debug.LogWarning("[WeaponUpgradePopupUI] 무기 강화 실패!");
        }
    }

    private void EquipWeapon()
    {
        EquipManager equipManager = PlayerobjManager.Instance.Player.GetComponent<EquipManager>();
        if (equipManager != null && currentWeapon != null)
        {
            equipManager.EquipWeapon(currentWeapon.BaseData);
            Debug.Log($"[WeaponUpgradePopupUI] 무기 {currentWeapon.BaseData.itemName} 장착 완료.");
        }
    }

    private void ClosePopup()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}