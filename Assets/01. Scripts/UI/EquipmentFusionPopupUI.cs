using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class EquipmentFusionPopupUI : UIBase
{
    [Header("Current Equipment Info")]
    [SerializeField] private Image currentEquipmentIcon;
    [SerializeField] private TextMeshProUGUI currentEquipmentNameTxt;
    [SerializeField] private TextMeshProUGUI currentEquipmentAmountTxt;

    [Header("Fusion Result Equipment Info")]
    [SerializeField] private Image resultEquipmentIcon;
    [SerializeField] private TextMeshProUGUI resultEquipmentNameTxt;
    [SerializeField] private TextMeshProUGUI resultEquipmentAmountTxt;

    [Header("Material Count Area")]
    [SerializeField] private TextMeshProUGUI materialCountTxt;
    [SerializeField] private Button subtractBtn;
    [SerializeField] private Button addBtn;

    [Header("Exit and Currency Info")]
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button fusionBtn;
    [SerializeField] private TextMeshProUGUI curCubeAmountTxt;

    [Header("Navigation Buttons")]
    [SerializeField] private Button upgradeReturnBtn;
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;

    private IFusable currentEquipment;
    private BaseItemDataSO resultEquipmentData;
    private int materialCount;
    private int maxMaterials;
    private int maxFusionCount;

    private bool isWeapon;

    private void Start()
    {
        exitBtn.onClick.AddListener(ClosePopup);
        fusionBtn.onClick.AddListener(PerformFusion);
        subtractBtn.onClick.AddListener(() => AdjustMaterialCount(-1));
        addBtn.onClick.AddListener(() => AdjustMaterialCount(1));
        upgradeReturnBtn.onClick.AddListener(ReturnToUpgradeUI);
        leftArrowBtn.onClick.AddListener(SelectPreviousItem);
        rightArrowBtn.onClick.AddListener(SelectNextItem);
    }

    public void SetEquipmentData(IFusable equipment, BaseItemDataSO resultData = null)
    {
        if (equipment == null) return;

        currentEquipment = equipment;
        isWeapon = equipment is Weapon;

        // 강제로 resultEquipmentData를 최신화
        resultEquipmentData = GetNextEquipmentData(currentEquipment);

        if (resultEquipmentData == null)
        {
            Debug.LogWarning("[SetEquipmentData] 다음 장비 데이터가 없습니다. 결과 없음으로 표시됩니다.");
        }

        InitializeUI(); // UI 초기화
        UpdateNavigationButtons(); // 화살표 버튼 상태 업데이트
    }

    private void ClearUI()
    {
        // 모든 UI 요소를 초기화 상태로 설정
        currentEquipmentIcon.sprite = null;
        currentEquipmentNameTxt.text = "없음";
        currentEquipmentAmountTxt.text = "0 (-0)";

        resultEquipmentIcon.sprite = null;
        resultEquipmentNameTxt.text = "결과 없음";
        resultEquipmentAmountTxt.text = "0 (+0)";

        materialCountTxt.text = "0";
        fusionBtn.gameObject.SetActive(false);
        leftArrowBtn.gameObject.SetActive(false);
        rightArrowBtn.gameObject.SetActive(false);
    }

    private void InitializeUI()
    {
        if (currentEquipment == null)
        {
            ClearUI();
            return;
        }

        // 필요 재료 개수와 최대 합성 가능 횟수 계산
        int requiredCount = GetRequiredFuseItemCount();
        maxMaterials = currentEquipment.StackCount - 1;
        maxFusionCount = requiredCount > 0 ? maxMaterials / requiredCount : 0;

        // 최대 합성 횟수로 materialCount 초기화
        materialCount = maxFusionCount;

        // UI 갱신
        UpdateMaterialCount();
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        UpdateFusionButtonState();

        // 재화 표시
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
    }

    private int GetRequiredFuseItemCount()
    {
        return currentEquipment.BaseData is WeaponDataSO weaponData
            ? weaponData.requireFuseItemCount
            : (currentEquipment.BaseData as AccessoryDataSO)?.requireFuseItemCount ?? 0;
    }

    private void UpdateMaterialCount()
    {
        materialCountTxt.text = materialCount.ToString();
    }

    private void UpdateCurrentEquipmentUI()
    {
        currentEquipmentIcon.sprite = currentEquipment.BaseData.icon;
        currentEquipmentNameTxt.text = currentEquipment.BaseData.itemName;

        int requiredCount = GetRequiredFuseItemCount();
        int usedMaterials = materialCount * requiredCount;

        // 인벤토리에 존재하는 장비인지 확인
        bool hasItem = currentEquipment.StackCount > 0;
        currentEquipmentIcon.color = hasItem ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // 없으면 검은색 처리

        currentEquipmentAmountTxt.text = $"{currentEquipment.StackCount - 1} (-{usedMaterials})";
    }

    private void UpdateResultEquipmentUI()
    {
        if (resultEquipmentData == null)
        {
            resultEquipmentIcon.sprite = null;
            resultEquipmentIcon.color = Color.white; // 기본 색상
            resultEquipmentNameTxt.text = "결과 없음";
            resultEquipmentAmountTxt.text = "0 (+0)";
        }
        else
        {
            resultEquipmentIcon.sprite = resultEquipmentData.icon;
            resultEquipmentNameTxt.text = resultEquipmentData.itemName;

            // 인벤토리에 존재하는지 확인
            int resultCount = GetResultItemCount();
            bool hasResultItem = resultCount > 0;
            resultEquipmentIcon.color = hasResultItem ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // 없으면 검은색 처리

            resultEquipmentAmountTxt.text = $"{resultCount} (+{materialCount})";
        }
    }

    private void UpdateFusionButtonState()
    {
        bool canFuse = materialCount > 0 && maxFusionCount > 0;
        fusionBtn.gameObject.SetActive(canFuse);

        if (!canFuse)
        {
            materialCount = 0;
            UpdateMaterialCount();
        }
    }

    private void AdjustMaterialCount(int amount)
    {
        materialCount = Mathf.Clamp(materialCount + amount, 0, maxFusionCount);
        UpdateMaterialCount();
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        UpdateFusionButtonState();
    }

    private void SelectPreviousItem()
    {
        var dataList = GetSortedItemList();

        int currentIndex = dataList.IndexOf(currentEquipment.BaseData);

        if (currentIndex > 0)
        {
            var newBaseData = dataList[currentIndex - 1];
            currentEquipment = FindFusableItemInInventoryOrDefault(newBaseData);
            SetEquipmentData(currentEquipment);
        }

        UpdateNavigationButtons();
    }

    private void SelectNextItem()
    {
        var dataList = GetSortedItemList();

        int currentIndex = dataList.IndexOf(currentEquipment.BaseData);

        if (currentIndex < dataList.Count - 1)
        {
            var newBaseData = dataList[currentIndex + 1];
            currentEquipment = FindFusableItemInInventoryOrDefault(newBaseData);
            SetEquipmentData(currentEquipment);
        }

        UpdateNavigationButtons();
    }

    private void UpdateNavigationButtons()
    {
        var dataList = GetSortedItemList();
        int currentIndex = dataList.IndexOf(currentEquipment.BaseData);

        // 화살표 버튼 상태 업데이트
        leftArrowBtn.gameObject.SetActive(currentIndex > 0); // 첫 번째 아이템이 아니면 왼쪽 버튼 표시
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 2); // 마지막에서 -1까지만 이동 가능
    }

    private IFusable FindFusableItemInInventoryOrDefault(BaseItemDataSO baseData)
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

    private List<BaseItemDataSO> GetSortedItemList()
    {
        return isWeapon
            ? DataManager.Instance.GetAllWeapons()
                .OrderBy(item => item.grade)
                .ThenByDescending(item => item.rank)
                .Cast<BaseItemDataSO>()
                .ToList()
            : DataManager.Instance.GetAllAccessories()
                .OrderBy(item => item.grade)
                .ThenByDescending(item => item.rank)
                .Cast<BaseItemDataSO>()
                .ToList();
    }

    private BaseItemDataSO GetNextEquipmentData(IFusable equipment)
    {
        if (equipment == null) return null;

        if (equipment is Weapon weapon)
        {
            var nextWeapon = DataManager.Instance.GetNextWeapon(weapon.BaseData.grade, weapon.BaseData.rank);
            return nextWeapon != weapon.BaseData ? nextWeapon : null; // 자기 자신 반환 방지
        }
        else if (equipment is Accessory accessory)
        {
            var nextAccessory = DataManager.Instance.GetNextAccessory(accessory.BaseData.grade, accessory.BaseData.rank);
            return nextAccessory != accessory.BaseData ? nextAccessory : null; // 자기 자신 반환 방지
        }

        return null;
    }

    private int GetResultItemCount()
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;
        return isWeapon
            ? inventory.WeaponInventory.GetAllItems().Find(x => x.BaseData == resultEquipmentData)?.StackCount ?? 0
            : inventory.AccessoryInventory.GetAllItems().Find(x => x.BaseData == resultEquipmentData)?.StackCount ?? 0;
    }

    private void PerformFusion()
    {
        if (materialCount <= 0)
        {
            Debug.LogWarning("[PerformFusion] 합성 조건을 충족하지 않음.");
            return;
        }

        bool success = currentEquipment is Weapon weapon
            ? weapon.Fuse(materialCount)
            : (currentEquipment as Accessory)?.Fuse(materialCount) ?? false;

        if (success)
        {
            Debug.Log("[PerformFusion] 합성 성공!");
            InitializeUI();
        }
        else
        {
            Debug.LogWarning("[PerformFusion] 합성 실패!");
        }
    }

    private void ClosePopup()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }

    private void ReturnToUpgradeUI()
    {
        Hide();
        UIManager.Instance.Show<EquipmentUpgradePopupUI>()?.SetEquipmentData(currentEquipment as IEnhanceable, isWeapon);
    }
}