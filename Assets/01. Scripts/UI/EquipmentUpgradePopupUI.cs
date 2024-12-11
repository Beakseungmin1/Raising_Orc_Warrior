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

    [Header("Effect Area")]
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
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;

    private IEnhanceable currentItem;
    private bool isWeapon;

    private void Start()
    {
        upgradeBtn.onClick.AddListener(UpgradeItem);
        exitBtn.onClick.AddListener(ClosePopup);
        fusionReturnBtn.onClick.AddListener(ReturnToFusionUI);
        leftArrowBtn.onClick.AddListener(SelectPreviousItem);
        rightArrowBtn.onClick.AddListener(SelectNextItem);
    }

    public void SetEquipmentData(IEnhanceable equipment, bool isWeaponType)
    {
        if (equipment == null)
        {
            return;
        }

        currentItem = equipment;
        isWeapon = isWeaponType;

        InitializeUI();
    }

    private void InitializeUI()
    {
        equipmentIcon.sprite = currentItem.BaseData.icon;
        nameTxt.text = currentItem.BaseData.itemName;
        gradeTxt.text = $"[{currentItem.BaseData.grade}]";

        currentAmountTxt.text = (currentItem as IStackable)?.StackCount.ToString() ?? "1";
        neededAmountTxt.text = "1";

        progressSlider.value = Mathf.Clamp01((float)((currentItem as IStackable)?.StackCount ?? 0) / int.Parse(neededAmountTxt.text));

        if (isWeapon && currentItem is Weapon weapon)
        {
            equipEffectTypeTxt.text = "���ݷ� ����";
            equipEffectValueTxt.text = $"{weapon.EquipAtkIncreaseRate}%";
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            equipEffectTypeTxt.text = "ü��/ü��ȸ���� ����";
            equipEffectValueTxt.text = $"{accessory.EquipHpAndHpRecoveryIncreaseRate}%";
        }

        UpdatePossessEffects();

        upgradeCostTxt.text = currentItem.RequiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = currentItem.BaseData.currencyIcon;
    }

    private void UpdatePossessEffects()
    {
        // ��� PossessEffectRow�� �ʱ�ȭ (��Ȱ��ȭ)
        possessEffectRow0.SetActive(false);
        possessEffectRow1.SetActive(false);
        possessEffectRow2.SetActive(false);

        int rowIndex = 0;

        // ���� ȿ�� �߰�
        if (isWeapon && currentItem is Weapon weapon)
        {
            if (weapon.PassiveEquipAtkIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "���ݷ� ����", $"{weapon.PassiveEquipAtkIncreaseRate}%");
            }
            if (weapon.PassiveCriticalDamageBonus > 0)
            {
                AddPossessEffectRow(rowIndex++, "�߰� ġ��Ÿ ������", $"{weapon.PassiveCriticalDamageBonus}%");
            }
            if (weapon.PassiveGoldGainRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "�߰� ��� ȹ�淮", $"{weapon.PassiveGoldGainRate}%");
            }
        }
        // �Ǽ����� ȿ�� �߰�
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            if (accessory.PassiveHpAndHpRecoveryIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "ü��/ü��ȸ���� ����", $"{accessory.PassiveHpAndHpRecoveryIncreaseRate}%");
            }
            if (accessory.PassiveMpAndMpRecoveryIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "��ü ����/���� ȸ����", $"{accessory.PassiveMpAndMpRecoveryIncreaseRate}%");
            }
            if (accessory.PassiveAddEXPRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "�߰� ����ġ", $"{accessory.PassiveAddEXPRate}%");
            }
        }

        // ���� Row���� ��Ȱ��ȭ�� ���·� ������
    }

    private void AddPossessEffectRow(int rowIndex, string effectType, string effectValue)
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
                break;
        }
    }

    private void UpgradeItem()
    {
        if (currentItem == null || !currentItem.CanEnhance())
        {
            return;
        }

        bool success = currentItem.Enhance();

        if (success)
        {
            InitializeUI();
        }        
    }

    private void ClosePopup()
    {
        if (isWeapon)
        {
            PlayerObjManager.Instance.Player.inventory.NotifyWeaponsChanged();
        }
        else
        {
            PlayerObjManager.Instance.Player.inventory.NotifyAccessoriesChanged();
        }

        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }

    private void ReturnToFusionUI()
    {
        Hide();
        var fusionPopup = UIManager.Instance.Show<EquipmentFusionPopupUI>();
        if (fusionPopup != null && currentItem is IFusable fusableItem)
        {
            fusionPopup.SetEquipmentData(fusableItem, currentItem.BaseData);
        }
    }

    private void SelectPreviousItem()
    {
        var playerInventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weaponList = playerInventory.WeaponInventory.GetAllItems();
            int currentIndex = weaponList.IndexOf(currentItem as Weapon);

            if (currentIndex > 0) // ���� �������� �����ϴ� ���
            {
                var previousWeapon = weaponList[currentIndex - 1];
                SetEquipmentData(previousWeapon, true);
            }            
        }
        else
        {
            var accessoryList = playerInventory.AccessoryInventory.GetAllItems();
            int currentIndex = accessoryList.IndexOf(currentItem as Accessory);

            if (currentIndex > 0) // ���� �������� �����ϴ� ���
            {
                var previousAccessory = accessoryList[currentIndex - 1];
                SetEquipmentData(previousAccessory, false);
            }            
        }
    }

    private void SelectNextItem()
    {
        var playerInventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weaponList = playerInventory.WeaponInventory.GetAllItems();
            int currentIndex = weaponList.IndexOf(currentItem as Weapon);

            if (currentIndex < weaponList.Count - 1) // ���� �������� �����ϴ� ���
            {
                var nextWeapon = weaponList[currentIndex + 1];
                SetEquipmentData(nextWeapon, true);
            }            
        }
        else
        {
            var accessoryList = playerInventory.AccessoryInventory.GetAllItems();
            int currentIndex = accessoryList.IndexOf(currentItem as Accessory);

            if (currentIndex < accessoryList.Count - 1) // ���� �������� �����ϴ� ���
            {
                var nextAccessory = accessoryList[currentIndex + 1];
                SetEquipmentData(nextAccessory, false);
            }            
        }
    }

    private void OnEquipWeapon()
    {
        EquipManager equipManager = PlayerObjManager.Instance.Player.GetComponent<EquipManager>();
        if (equipManager != null && currentItem is Weapon weapon)
        {
            equipManager.EquipWeapon(weapon);
            Debug.Log($"[WeaponUpgradePopupUI] ���� {weapon.BaseData.itemName} ���� �Ϸ�.");
        }
    }

}