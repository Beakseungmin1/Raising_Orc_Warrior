using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

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
    [SerializeField] private Button equipBtn;
    [SerializeField] private TextMeshProUGUI upgradeCostTxt;
    [SerializeField] private TextMeshProUGUI curCubeAmountTxt;
    [SerializeField] private Image curCubeIcon;
    [SerializeField] private Button fusionBtn;
    [SerializeField] private Button exitBtn;

    [Header("Navigation Buttons")]
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;

    private IEnhanceable currentItem;
    private bool isWeapon;
    private Button currentlyEquippedButton;

    private void Start()
    {
        upgradeBtn.onClick.AddListener(UpgradeItem);
        equipBtn.onClick.AddListener(EquipItem);
        exitBtn.onClick.AddListener(ClosePopup);
        fusionBtn.onClick.AddListener(ReturnToFusionUI);
        leftArrowBtn.onClick.AddListener(SelectPreviousItem);
        rightArrowBtn.onClick.AddListener(SelectNextItem);
    }

    public void SetEquipmentData(IEnhanceable equipment, bool isWeaponType)
    {
        if (equipment == null)
        {
            Debug.LogError("SetEquipmentData: equipment is null!");
            DisableEnhanceAndEquipButtons();
            fusionBtn.gameObject.SetActive(false);
            return;
        }

        var inventory = PlayerObjManager.Instance.Player.inventory;
        if (isWeaponType)
        {
            var weapon = inventory.WeaponInventory.GetAllItems().FirstOrDefault(item => item.BaseData == equipment.BaseData);
            currentItem = weapon ?? equipment;
        }
        else
        {
            var accessory = inventory.AccessoryInventory.GetAllItems().FirstOrDefault(item => item.BaseData == equipment.BaseData);
            currentItem = accessory ?? equipment;
        }

        isWeapon = isWeaponType;

        InitializeUI();
        UpdateEquipButtonState();
        UpdateNavigationButtons();

        var nextEquipment = GetNextEquipmentData(currentItem as IFusable);
        fusionBtn.gameObject.SetActive(nextEquipment != null);
    }

    private BaseItemDataSO GetNextEquipmentData(IFusable equipment)
    {
        if (equipment == null) return null;

        if (equipment is Weapon weapon)
        {
            var nextWeapon = DataManager.Instance.GetNextWeapon(weapon.BaseData.grade, weapon.BaseData.rank);
            return nextWeapon != weapon.BaseData ? nextWeapon : null;
        }
        else if (equipment is Accessory accessory)
        {
            var nextAccessory = DataManager.Instance.GetNextAccessory(accessory.BaseData.grade, accessory.BaseData.rank);
            return nextAccessory != accessory.BaseData ? nextAccessory : null;
        }

        return null;
    }

    private void InitializeUI()
    {
        if (currentItem == null)
        {
            DisableEnhanceAndEquipButtons();
            return;
        }

        if (isWeapon && currentItem is Weapon weapon)
        {
            equipEffectTypeTxt.text = "공격력 증가";
            equipEffectValueTxt.text = $"{weapon.EquipAtkIncreaseRate}%";
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            equipEffectTypeTxt.text = "체력/체력회복량 증가";
            equipEffectValueTxt.text = $"{accessory.EquipHpAndHpRecoveryIncreaseRate}%";
        }

        int actualStackCount = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentItem);
        bool isItemAvailable = actualStackCount > 0;

        int requiredAmount = GetRequiredFuseItemCount();

        equipmentIcon.sprite = currentItem.BaseData.icon;
        equipmentIcon.color = isItemAvailable ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f);
        nameTxt.text = currentItem.BaseData.itemName;
        gradeTxt.text = $"[{currentItem.BaseData.grade}]";

        currentAmountTxt.text = Mathf.Max(0, actualStackCount - 1).ToString();
        neededAmountTxt.text = requiredAmount.ToString();
        progressSlider.value = Mathf.Clamp01((float)(actualStackCount - 1) / requiredAmount);

        upgradeCostTxt.text = currentItem.RequiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = currentItem.BaseData.currencyIcon;

        UpdateEquipButtonState();
        UpdatePossessEffects();
        UpdateUpgradeButtonState(isItemAvailable);
    }

    private bool IsCurrentlyEquipped(IEnhanceable item)
    {
        if (isWeapon && item is Weapon weapon)
        {
            return PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.IsWeaponEquipped(weapon) ?? false;
        }
        else if (!isWeapon && item is Accessory accessory)
        {
            return PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.IsAccessoryEquipped(accessory) ?? false;
        }
        return false;
    }

    private int GetRequiredFuseItemCount()
    {
        if (currentItem is Weapon weapon)
        {
            return weapon.BaseData.requireFuseItemCount;
        }
        else if (currentItem is Accessory accessory)
        {
            return accessory.BaseData.requireFuseItemCount;
        }

        return 5;
    }

    private void UpdateUpgradeButtonState(bool isItemAvailable)
    {
        bool canEnhance = currentItem != null && currentItem.CanEnhance();
        bool canEquip = isItemAvailable;

        upgradeBtn.gameObject.SetActive(isItemAvailable && canEnhance);
        equipBtn.gameObject.SetActive(isItemAvailable && canEquip);
    }

    private void UpdateEquipButtonState()
    {
        if (currentItem == null)
        {
            equipBtn.gameObject.SetActive(false);
            return;
        }

        var equipManager = PlayerObjManager.Instance.Player.GetComponent<EquipManager>();

        if (isWeapon && currentItem is Weapon weapon)
        {
            if (equipManager.EquippedWeapon == weapon)
            {
                equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착중";
                equipBtn.interactable = false;
                return;
            }
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            if (equipManager.EquippedAccessory == accessory)
            {
                equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착중";
                equipBtn.interactable = false;
                return;
            }
        }

        equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착";
        equipBtn.interactable = true;
    }

    private void UpdatePossessEffects()
    {
        possessEffectRow0.SetActive(false);
        possessEffectRow1.SetActive(false);
        possessEffectRow2.SetActive(false);

        int rowIndex = 0;

        if (isWeapon && currentItem is Weapon weapon)
        {
            if (weapon.PassiveEquipAtkIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "공격력 증가", $"{weapon.PassiveEquipAtkIncreaseRate}%");
            }
            if (weapon.PassiveCriticalDamageBonus > 0)
            {
                AddPossessEffectRow(rowIndex++, "추가 치명타 데미지", $"{weapon.PassiveCriticalDamageBonus}%");
            }
            if (weapon.PassiveGoldGainRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "추가 골드 획득량", $"{weapon.PassiveGoldGainRate}%");
            }
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            if (accessory.PassiveHpAndHpRecoveryIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "체력/체력회복 증가", $"{accessory.PassiveHpAndHpRecoveryIncreaseRate}%");
            }
            if (accessory.PassiveMpAndMpRecoveryIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "마나/마나회복 증가", $"{accessory.PassiveMpAndMpRecoveryIncreaseRate}%");
            }
            if (accessory.PassiveAddEXPRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "추가 경험치", $"{accessory.PassiveAddEXPRate}%");
            }
        }
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
        }
    }

    public void DisableEnhanceAndEquipButtons()
    {
        upgradeBtn.gameObject.SetActive(false);
        equipBtn.gameObject.SetActive(false);
    }

    private void UpgradeItem()
    {
        if (currentItem == null || !currentItem.CanEnhance())
        {
            return;
        }

        var inventory = PlayerObjManager.Instance.Player.inventory;

        bool success = false;

        if (isWeapon)
        {
            var weapon = inventory.WeaponInventory.GetAllItems()
                .FirstOrDefault(item => item.BaseData == currentItem.BaseData);

            if (weapon != null && weapon.Enhance())
            {
                currentItem = weapon;
                success = true;
            }
        }
        else
        {
            var accessory = inventory.AccessoryInventory.GetAllItems()
                .FirstOrDefault(item => item.BaseData == currentItem.BaseData);

            if (accessory != null && accessory.Enhance())
            {
                currentItem = accessory;
                success = true;
            }
        }

        if (success)
        {
            SetEquipmentData(currentItem, isWeapon);
        }
    }

    private void EquipItem()
    {
        if (currentItem == null)
        {
            return;
        }

        if (currentlyEquippedButton != null)
        {
            currentlyEquippedButton.interactable = true;
            currentlyEquippedButton.GetComponentInChildren<TextMeshProUGUI>().text = "장착하기";
        }

        if (isWeapon && currentItem is Weapon weapon)
        {
            PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.EquipWeapon(weapon);
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.EquipAccessory(accessory);
        }

        equipBtn.interactable = false;
        equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착중";
        currentlyEquippedButton = equipBtn;

        Debug.Log($"{currentItem.BaseData.itemName} 장착 완료!");
    }

    private void ClosePopup()
    {
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
        var dataList = GetSortedItemList();

        int currentIndex = dataList.IndexOf(currentItem.BaseData);

        if (currentIndex > 0)
        {
            var newBaseData = dataList[currentIndex - 1];
            currentItem = FindItemInInventoryOrDefault(newBaseData);
            SetEquipmentData(currentItem, isWeapon);
        }

        UpdateNavigationButtons();
    }

    private void SelectNextItem()
    {
        var dataList = GetSortedItemList();

        int currentIndex = dataList.IndexOf(currentItem.BaseData);

        if (currentIndex < dataList.Count - 1)
        {
            var newBaseData = dataList[currentIndex + 1];
            currentItem = FindItemInInventoryOrDefault(newBaseData);
            SetEquipmentData(currentItem, isWeapon);
        }

        UpdateNavigationButtons();
    }

    private void UpdateNavigationButtons()
    {
        var dataList = GetSortedItemList();

        int currentIndex = dataList.IndexOf(currentItem.BaseData);

        leftArrowBtn.gameObject.SetActive(currentIndex > 0);
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 1);
    }

    private IEnhanceable FindItemInInventoryOrDefault(BaseItemDataSO baseData)
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weapon = inventory.WeaponInventory.GetAllItems()
                .FirstOrDefault(item => item.BaseData == baseData);
            return weapon ?? new Weapon((WeaponDataSO)baseData);
        }
        else
        {
            var accessory = inventory.AccessoryInventory.GetAllItems()
                .FirstOrDefault(item => item.BaseData == baseData);
            return accessory ?? new Accessory((AccessoryDataSO)baseData);
        }
    }

    private IEnhanceable CreateEnhanceableItem(BaseItemDataSO baseItemData)
    {
        if (baseItemData is WeaponDataSO weaponData)
        {
            return new Weapon(weaponData);
        }
        else if (baseItemData is AccessoryDataSO accessoryData)
        {
            return new Accessory(accessoryData);
        }
        return null;
    }

    private List<BaseItemDataSO> GetSortedItemList()
    {
        return isWeapon
            ? DataManager.Instance.GetAllWeapons().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList()
            : DataManager.Instance.GetAllAccessories().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList();
    }
}