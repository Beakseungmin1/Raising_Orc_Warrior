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
    [SerializeField] private GameObject possessEffectRow0;
    [SerializeField] private TextMeshProUGUI possessEffectTypeTxt0;
    [SerializeField] private TextMeshProUGUI possessEffectValueTxt0;
    [SerializeField] private GameObject possessEffectRow1;
    [SerializeField] private TextMeshProUGUI possessEffectTypeTxt1;
    [SerializeField] private TextMeshProUGUI possessEffectValueTxt1;
    [SerializeField] private GameObject possessEffectRow2;
    [SerializeField] private TextMeshProUGUI possessEffectTypeTxt2;
    [SerializeField] private TextMeshProUGUI possessEffectValueTxt2;

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
            Debug.LogError("[WeaponUpgradePopupUI] Weapon �����Ͱ� ��ȿ���� �ʽ��ϴ�.");
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

        // Equipment Effect Area (���� ȿ��)
        equipEffectTypeTxt.text = "���ݷ� ������";
        equipEffectValueTxt.text = $"{weaponData.equipAtkIncreaseRate}%";

        // Possess Effect Area (���� ȿ��)
        UpdatePossessEffects(weaponData);

        // Upgrade Info
        upgradeCostTxt.text = weaponData.requiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = weaponData.currencyIcon;
    }

    private void UpdatePossessEffects(WeaponDataSO weaponData)
    {
        // ���� ȿ�� �ʱ�ȭ
        possessEffectRow0.SetActive(false);
        possessEffectRow1.SetActive(false);
        possessEffectRow2.SetActive(false);

        int rowIndex = 0;

        // ���ݷ� ������
        if (weaponData.equipAtkIncreaseRate > 0)
        {
            UpdateEffectRow(rowIndex++, "���ݷ� ������", $"{Mathf.RoundToInt(weaponData.equipAtkIncreaseRate / 3)}%");
        }

        // ġ��Ÿ ������ ����
        if (weaponData.passiveCriticalDamageBonus > 0)
        {
            UpdateEffectRow(rowIndex++, "ġ��Ÿ ������ ����", $"{weaponData.passiveCriticalDamageBonus}%");
        }

        // ��� ȹ�淮 ������
        if (weaponData.passiveGoldGainRate > 0)
        {
            UpdateEffectRow(rowIndex++, "��� ȹ�淮 ������", $"{weaponData.passiveGoldGainRate}%");
        }
    }

    private void UpdateEffectRow(int rowIndex, string effectType, string effectValue)
    {
        switch (rowIndex)
        {
            case 0:
                possessEffectRow0.SetActive(true);
                possessEffectTypeTxt0.text = effectType;
                possessEffectValueTxt0.text = effectValue;
                break;
            case 1:
                possessEffectRow1.SetActive(true);
                possessEffectTypeTxt1.text = effectType;
                possessEffectValueTxt1.text = effectValue;
                break;
            case 2:
                possessEffectRow2.SetActive(true);
                possessEffectTypeTxt2.text = effectType;
                possessEffectValueTxt2.text = effectValue;
                break;
            default:
                Debug.LogWarning($"[WeaponUpgradePopupUI] �������� �ʴ� ���� ȿ�� �� �ε���: {rowIndex}");
                break;
        }
    }

    private void UpgradeWeapon()
    {
        if (currentWeapon == null || !currentWeapon.CanEnhance())
        {
            Debug.LogWarning("[WeaponUpgradePopupUI] ��ȭ ������ �������� ���߽��ϴ�.");
            return;
        }

        bool success = currentWeapon.Enhance();
        if (success)
        {
            SetWeaponData(currentWeapon);
            Debug.Log($"[WeaponUpgradePopupUI] ���� {currentWeapon.BaseData.itemName} ��ȭ ����!");
        }
        else
        {
            Debug.LogWarning("[WeaponUpgradePopupUI] ���� ��ȭ ����!");
        }
    }

    private void EquipWeapon()
    {
        EquipManager equipManager = PlayerobjManager.Instance.Player.GetComponent<EquipManager>();
        if (equipManager != null && currentWeapon != null)
        {
            equipManager.EquipWeapon(currentWeapon);
            Debug.Log($"[WeaponUpgradePopupUI] ���� {currentWeapon.BaseData.itemName} ���� �Ϸ�.");
        }
    }

    private void ClosePopup()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}