using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    private bool isWeapon;

    private const int RequiredMaterialsPerFusion = 1;

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
        if (equipment == null)
        {
            return;
        }

        currentEquipment = equipment;

        // 결과 데이터 설정: 기본값은 GetNextEquipmentData로 설정
        resultEquipmentData = resultData ?? GetNextEquipmentData(equipment);

        // 결과 데이터가 없을 경우 예외 처리
        if (resultEquipmentData == null)
        {
            Debug.LogWarning("[SetEquipmentData] resultEquipmentData가 설정되지 않았습니다.");
            return;
        }

        isWeapon = equipment is Weapon;

        // UI 초기화
        InitializeUI();
        UpdateNavigationButtons();
    }

    private void InitializeUI()
    {
        // 현재 아이템 스택 카운트 가져오기 (최소 1개는 남김)
        maxMaterials = Mathf.Max(0, PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentEquipment as IEnhanceable) - 1);
        materialCount = Mathf.Clamp(maxMaterials / RequiredMaterialsPerFusion, 1, maxMaterials);

        // 결과 아이템 최신화 (항상 최신 상태 유지)
        resultEquipmentData = GetNextEquipmentData(currentEquipment);

        // UI 초기화
        UpdateMaterialCount();
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        UpdateFusionButtonState();

        // 현재 재화 표시
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency<float>(CurrencyType.Cube).ToString();
    }

    private void UpdateMaterialCount()
    {
        // MaterialCount와 관련된 UI를 업데이트
        materialCountTxt.text = materialCount.ToString();
    }

    private void UpdateCurrentEquipmentUI()
    {
        if (currentEquipment is IEnhanceable enhanceable)
        {
            currentEquipmentIcon.sprite = enhanceable.BaseData.icon;
            currentEquipmentIcon.color = maxMaterials > 0 ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // 검은색 처리
            currentEquipmentNameTxt.text = enhanceable.BaseData.itemName;
            currentEquipmentAmountTxt.text = $"{maxMaterials} (-{materialCount})"; // 총 스택에서 1개를 더해서 표시
        }
        else
        {
            Debug.LogError("UpdateCurrentEquipmentUI: currentEquipment is not IEnhanceable!");
        }
    }

    private void UpdateResultEquipmentUI()
    {
        if (resultEquipmentData == null)
        {
            resultEquipmentIcon.sprite = null;
            resultEquipmentIcon.color = new Color(0.2f, 0.2f, 0.2f, 1f); // 검은색 처리
            resultEquipmentNameTxt.text = "결합 결과 없음";
            resultEquipmentAmountTxt.text = "0 (+0)";
        }
        else
        {
            int currentResultCount = GetResultItemCount();
            resultEquipmentIcon.sprite = resultEquipmentData.icon;
            resultEquipmentIcon.color = currentResultCount > 0 ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // 검은색 처리
            resultEquipmentNameTxt.text = resultEquipmentData.itemName;
            resultEquipmentAmountTxt.text = $"{currentResultCount} (+{materialCount})";
        }
    }

    private void UpdateFusionButtonState()
    {
        bool canFuse = CanPerformFusion();
        fusionBtn.gameObject.SetActive(canFuse); // 조건에 따라 버튼 숨김 처리
    }

    private void AdjustMaterialCount(int amount)
    {
        // 재료 카운트 계산 (0까지 허용)
        materialCount = Mathf.Clamp(materialCount + amount, 0, maxMaterials);

        // 텍스트 업데이트
        materialCountTxt.text = materialCount.ToString();

        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        // Fusion 버튼의 상태를 항상 유지
    }

    private void SelectPreviousItem()
    {
        var dataList = isWeapon
            ? DataManager.Instance.GetAllWeapons().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList()
            : DataManager.Instance.GetAllAccessories().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList();

        int currentIndex = dataList.FindIndex(item => item == (currentEquipment as IEnhanceable)?.BaseData);
        if (currentIndex == -1)
        {
            Debug.LogWarning("[SelectPreviousItem] 현재 장비가 데이터 리스트에 없습니다.");
            return;
        }

        if (currentIndex > 0) // 왼쪽으로 이동 가능한 경우
        {
            var previousItem = dataList[currentIndex - 1];
            SetEquipmentData(CreateFusableItem(previousItem), GetNextEquipmentData(CreateFusableItem(previousItem)));
        }

        UpdateNavigationButtons();
    }

    private void SelectNextItem()
    {
        var dataList = isWeapon
            ? DataManager.Instance.GetAllWeapons().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList()
            : DataManager.Instance.GetAllAccessories().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList();

        int currentIndex = dataList.FindIndex(item => item == (currentEquipment as IEnhanceable)?.BaseData);
        if (currentIndex == -1)
        {
            Debug.LogWarning("[SelectNextItem] 현재 장비가 데이터 리스트에 없습니다.");
            return;
        }

        if (currentIndex < dataList.Count - 2) // 끝에서 하나 앞까지만 이동 가능
        {
            var nextItem = dataList[currentIndex + 1];
            SetEquipmentData(CreateFusableItem(nextItem), GetNextEquipmentData(CreateFusableItem(nextItem)));
        }

        UpdateNavigationButtons(); // 버튼 상태 업데이트
    }

    private void UpdateNavigationButtons()
    {
        // 데이터 리스트 가져오기
        var dataList = isWeapon
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

        // 현재 아이템의 인덱스 계산
        int currentIndex = dataList.FindIndex(item => item == (currentEquipment as IEnhanceable)?.BaseData);

        // 양쪽 끝에서 버튼을 숨기거나 표시
        leftArrowBtn.gameObject.SetActive(currentIndex > 0);               // 첫 번째 아이템에서는 왼쪽 버튼 숨김
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 2); // 마지막에서 하나 앞까지만 오른쪽 버튼 표시
    }

    private IFusable CreateFusableItem(BaseItemDataSO baseItemData)
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

    private BaseItemDataSO GetNextEquipmentData(IFusable equipment)
    {
        if (equipment == null) return null;

        if (equipment is Weapon weapon)
        {
            return DataManager.Instance.GetNextWeapon(weapon.BaseData.grade, weapon.BaseData.rank);
        }
        else if (equipment is Accessory accessory)
        {
            return DataManager.Instance.GetNextAccessory(accessory.BaseData.grade, accessory.BaseData.rank);
        }

        return null;
    }

    private bool CanPerformFusion()
    {
        return maxMaterials >= materialCount * RequiredMaterialsPerFusion;
    }

    private int GetResultItemCount()
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        if (resultEquipmentData == null) return 0;

        return isWeapon
            ? inventory.WeaponInventory.GetAllItems().Find(x => x.BaseData == resultEquipmentData)?.StackCount ?? 0
            : inventory.AccessoryInventory.GetAllItems().Find(x => x.BaseData == resultEquipmentData)?.StackCount ?? 0;
    }

    private void PerformFusion()
    {
        if (materialCount <= 0 || !CanPerformFusion())
        {
            Debug.LogWarning("[PerformFusion] 합성 조건을 충족하지 않음.");
            return;
        }

        bool success = false;

        if (currentEquipment is Weapon weapon)
        {
            success = weapon.Fuse(materialCount);
        }
        else if (currentEquipment is Accessory accessory)
        {
            success = accessory.Fuse(materialCount);
        }

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