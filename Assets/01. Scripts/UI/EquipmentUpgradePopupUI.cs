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
            return;
        }

        // 인벤토리에서 최신 데이터 가져오기
        var inventory = PlayerObjManager.Instance.Player.inventory;
        if (isWeaponType)
        {
            var weapon = inventory.WeaponInventory.GetAllItems().FirstOrDefault(item => item.BaseData == equipment.BaseData);
            currentItem = weapon ?? equipment; // 최신 데이터가 있으면 갱신
        }
        else
        {
            var accessory = inventory.AccessoryInventory.GetAllItems().FirstOrDefault(item => item.BaseData == equipment.BaseData);
            currentItem = accessory ?? equipment; // 최신 데이터가 있으면 갱신
        }

        isWeapon = isWeaponType;

        InitializeUI(); // UI 업데이트
        UpdateEquipButtonState(); // 장착 상태 동기화
        UpdateNavigationButtons(); // 화살표 상태 업데이트
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

        // 현재 아이템 스택 개수 확인
        int actualStackCount = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentItem);
        bool isItemAvailable = actualStackCount > 0; // 인벤토리에 아이템이 있는지 확인

        int requiredAmount = GetRequiredFuseItemCount();

        equipmentIcon.sprite = currentItem.BaseData.icon;
        equipmentIcon.color = isItemAvailable ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // 검은색 처리
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

        return 5; // 기본값
    }

    private void UpdateUpgradeButtonState(bool isItemAvailable)
    {
        bool canEnhance = currentItem != null && currentItem.CanEnhance();
        bool canEquip = isItemAvailable; // 인벤토리에 아이템이 있을 때만 장착 가능

        upgradeBtn.gameObject.SetActive(isItemAvailable && canEnhance); // 아이템이 없으면 버튼 자체 숨김
        equipBtn.gameObject.SetActive(isItemAvailable && canEquip);    // 아이템이 없으면 버튼 자체 숨김
    }

    private void UpdateEquipButtonState()
    {
        if (currentItem == null)
        {
            equipBtn.gameObject.SetActive(false);
            return;
        }

        var equipManager = PlayerObjManager.Instance.Player.GetComponent<EquipManager>();

        // 현재 아이템이 장착 상태인지 확인
        if (isWeapon && currentItem is Weapon weapon)
        {
            if (equipManager.EquippedWeapon == weapon)
            {
                equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착중";
                equipBtn.interactable = false;
                return; // 상태 유지
            }
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            if (equipManager.EquippedAccessory == accessory)
            {
                equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "장착중";
                equipBtn.interactable = false;
                return; // 상태 유지
            }
        }

        // 현재 아이템이 장착 상태가 아닐 경우 기본 값
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
            Debug.LogError("강화할 아이템이 없거나 조건을 만족하지 않습니다.");
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
                currentItem = weapon; // 강화된 아이템을 다시 currentItem에 반영
                success = true;
            }
        }
        else
        {
            var accessory = inventory.AccessoryInventory.GetAllItems()
                .FirstOrDefault(item => item.BaseData == currentItem.BaseData);

            if (accessory != null && accessory.Enhance())
            {
                currentItem = accessory; // 강화된 아이템을 다시 currentItem에 반영
                success = true;
            }
        }

        if (success)
        {
            SetEquipmentData(currentItem, isWeapon); // UI 갱신
        }
    }

    private void EquipItem()
    {
        if (currentItem == null)
        {
            Debug.LogError("EquipItem: currentItem is null!");
            return;
        }

        // 이전 장착 상태 복구
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

        // 현재 버튼 상태 업데이트
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

        // 현재 아이템의 인덱스 찾기
        int currentIndex = dataList.IndexOf(currentItem.BaseData);

        // 이전 아이템 설정
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

        // 현재 아이템의 인덱스 찾기
        int currentIndex = dataList.IndexOf(currentItem.BaseData);

        // 다음 아이템 설정
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
        // DataManager에서 모든 아이템 데이터 가져오기
        var dataList = GetSortedItemList();

        // 현재 아이템의 인덱스 찾기
        int currentIndex = dataList.IndexOf(currentItem.BaseData);

        // 버튼 상태 업데이트
        leftArrowBtn.gameObject.SetActive(currentIndex > 0);
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 1);
    }

    private IEnhanceable FindItemInInventoryOrDefault(BaseItemDataSO baseData)
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        // 인벤토리에서 실제 객체 찾기
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