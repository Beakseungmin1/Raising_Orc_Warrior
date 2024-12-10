using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentUpgradePopupUI : UIBase
{
    [Header("Equipment Icon Area")]
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currentAmountTxt;
    [SerializeField] private TextMeshProUGUI neededAmountTxt;

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
    [SerializeField] private Button exitBtn;

    [Header("Navigation Buttons")]
    [SerializeField] private Button fusionReturnBtn;

    private IEnhanceable currentItem; // ���� ��� (Weapon �Ǵ� Accessory)
    private bool isWeapon; // �������� ���θ� �Ǵ�

    private void Start()
    {
        exitBtn.onClick.AddListener(ClosePopup);
        upgradeBtn.onClick.AddListener(UpgradeItem);
        fusionReturnBtn.onClick.AddListener(ReturnToFusionUI);
    }

    public void SetEquipmentData(object equipment, bool isWeaponType)
    {
        if (equipment == null)
        {
            Debug.LogError("[EquipmentUpgradePopupUI] ��� �����Ͱ� ��ȿ���� �ʽ��ϴ�.");
            return;
        }

        currentItem = equipment as IEnhanceable;
        isWeapon = isWeaponType;

        if (isWeapon && currentItem is Weapon weapon)
        {
            UpdateUIForWeapon(weapon);
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            UpdateUIForAccessory(accessory);
        }
    }

    private void UpdateUIForWeapon(Weapon weapon)
    {
        var weaponData = weapon.BaseData;

        equipmentIcon.sprite = weaponData.icon;
        nameTxt.text = weaponData.itemName;
        gradeTxt.text = $"[{weaponData.grade}]";
        gradeTxt.color = weaponData.gradeColor;
        currentAmountTxt.text = weapon.StackCount.ToString();
        neededAmountTxt.text = "1";
        progressSlider.value = Mathf.Clamp01((float)weapon.StackCount / 1);

        equipEffectTypeTxt.text = "���ݷ� ����";
        equipEffectValueTxt.text = $"{weapon.EquipAtkIncreaseRate}%";

        UpdatePossessEffectsForWeapon(weapon.PassiveEquipAtkIncreaseRate, weapon.PassiveCriticalDamageBonus, weapon.PassiveGoldGainRate);

        upgradeCostTxt.text = weapon.RequiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = weaponData.currencyIcon;
    }

    private void UpdateUIForAccessory(Accessory accessory)
    {
        var accessoryData = accessory.BaseData;

        equipmentIcon.sprite = accessoryData.icon;
        nameTxt.text = accessoryData.itemName;
        gradeTxt.text = $"[{accessoryData.grade}]";
        gradeTxt.color = accessoryData.gradeColor;
        currentAmountTxt.text = accessory.StackCount.ToString();
        neededAmountTxt.text = "1";
        progressSlider.value = Mathf.Clamp01((float)accessory.StackCount / 1);

        equipEffectTypeTxt.text = "ü��/ü��ȸ���� ����";
        equipEffectValueTxt.text = $"{accessory.EquipHpAndHpRecoveryIncreaseRate}%";

        UpdatePossessEffectsForAccessory(
            accessory.PassiveHpAndHpRecoveryIncreaseRate,
            accessory.PassiveMpAndMpRecoveryIncreaseRate,
            accessory.PassiveAddEXPRate
        );

        upgradeCostTxt.text = accessory.RequiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = accessoryData.currencyIcon;
    }

    private void UpdatePossessEffectsForWeapon(float atkIncreaseRate, float criticalDamageBonus, float goldGainRate)
    {
        possessEffectRow0.SetActive(false);
        possessEffectRow1.SetActive(false);
        possessEffectRow2.SetActive(false);

        int rowIndex = 0;

        if (atkIncreaseRate > 0)
        {
            UpdateEffectRow(rowIndex++, "���ݷ� ����", $"{atkIncreaseRate}%");
        }
        if (criticalDamageBonus > 0)
        {
            UpdateEffectRow(rowIndex++, "�߰� ġ��Ÿ ������", $"{criticalDamageBonus}%");
        }
        if (goldGainRate > 0)
        {
            UpdateEffectRow(rowIndex++, "�߰� ��� ȹ�淮", $"{goldGainRate}%");
        }
    }

    private void UpdatePossessEffectsForAccessory(float hpRecoveryIncreaseRate, float manaRecoveryIncreaseRate, float expGainRate)
    {
        possessEffectRow0.SetActive(false);
        possessEffectRow1.SetActive(false);
        possessEffectRow2.SetActive(false);

        int rowIndex = 0;

        if (hpRecoveryIncreaseRate > 0)
        {
            UpdateEffectRow(rowIndex++, "ü��/ü��ȸ���� ����", $"{hpRecoveryIncreaseRate}%");
        }
        if (manaRecoveryIncreaseRate > 0)
        {
            UpdateEffectRow(rowIndex++, "��ü ����/���� ȸ����", $"{manaRecoveryIncreaseRate}%");
        }
        if (expGainRate > 0)
        {
            UpdateEffectRow(rowIndex++, "�߰� ����ġ", $"{expGainRate}%");
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
                Debug.LogWarning($"[EquipmentUpgradePopupUI] �������� �ʴ� ���� ȿ�� �� �ε���: {rowIndex}");
                break;
        }
    }

    private void UpgradeItem()
    {
        if (currentItem == null || !currentItem.CanEnhance())
        {
            Debug.LogWarning("[EquipmentUpgradePopupUI] ��ȭ ������ �������� ���߽��ϴ�.");
            return;
        }

        bool success = currentItem.Enhance();
        if (success)
        {
            Debug.Log("[EquipmentUpgradePopupUI] ������ ��ȭ ����!");
            SetEquipmentData(currentItem, isWeapon); // UI ����
        }
        else
        {
            Debug.LogWarning("[EquipmentUpgradePopupUI] ������ ��ȭ ����!");
        }
    }

    private void ClosePopup()
    {
        if (isWeapon)
        {
            PlayerobjManager.Instance.Player.inventory.NotifyWeaponsChanged();
        }
        else
        {
            PlayerobjManager.Instance.Player.inventory.NotifyAccessoriesChanged();
        }

        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }

    private void ReturnToFusionUI()
    {
        Hide();
        var fusionPopup = UIManager.Instance.Show<EquipmentFusionPopupUI>();
        if (fusionPopup != null)
        {
            fusionPopup.SetEquipmentData(currentItem, isWeapon);
        }
    }
}