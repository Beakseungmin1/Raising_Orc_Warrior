using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private Button leftArrowBtn; // 왼쪽 화살표 버튼
    [SerializeField] private Button rightArrowBtn; // 오른쪽 화살표 버튼

    private IFusable currentEquipment;
    private BaseItemDataSO resultEquipmentData;
    private int materialCount;
    private int maxMaterials;

    private bool isWeapon;

    // 합성에 필요한 기본 재료 수량 (상수로 설정)
    private const int RequiredMaterialsPerFusion = 1;

    private void Start()
    {
        exitBtn.onClick.AddListener(ClosePopup);
        fusionBtn.onClick.AddListener(PerformFusion);
        subtractBtn.onClick.AddListener(DecreaseMaterialCount);
        addBtn.onClick.AddListener(IncreaseMaterialCount);

        upgradeReturnBtn.onClick.AddListener(ReturnToUpgradeUI);

        // 화살표 버튼 이벤트 추가
        leftArrowBtn.onClick.AddListener(SelectPreviousItem);
        rightArrowBtn.onClick.AddListener(SelectNextItem);
    }

    public void SetEquipmentData(IFusable equipment, BaseItemDataSO resultData)
    {
        if (equipment == null)
        {
            return;
        }

        currentEquipment = equipment;
        resultEquipmentData = resultData;

        isWeapon = equipment is Weapon;

        InitializeUI();
    }

    private void InitializeUI()
    {
        maxMaterials = (currentEquipment as IStackable)?.StackCount ?? 0;

        currentEquipmentAmountTxt.text = $"{maxMaterials} (-{materialCount * RequiredMaterialsPerFusion})";
        materialCountTxt.text = materialCount.ToString();

        if (currentEquipment is IEnhanceable enhanceable)
        {
            currentEquipmentIcon.sprite = enhanceable.BaseData.icon;
            currentEquipmentNameTxt.text = enhanceable.BaseData.itemName;
        }

        resultEquipmentData = GetNextEquipmentData();
        UpdateResultEquipmentUI();

        int maxFusionCount = Mathf.Min(maxMaterials / RequiredMaterialsPerFusion, 10);
        materialCount = Mathf.Clamp(materialCount, 1, maxFusionCount);
        materialCountTxt.text = materialCount.ToString();

        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();

        currentEquipmentAmountTxt.text = $"{maxMaterials} (-{materialCount * RequiredMaterialsPerFusion})";

        UpdateResultEquipmentUI();
    }

    private void UpdateResultEquipmentUI()
    {
        if (resultEquipmentData == null)
        {
            resultEquipmentIcon.sprite = null;
            resultEquipmentNameTxt.text = "결합 결과 없음";
            resultEquipmentAmountTxt.text = "0 (+0)";
        }
        else
        {
            int currentResultCount = GetResultItemCount(); // 현재 결과 아이템 수량
            resultEquipmentIcon.sprite = resultEquipmentData.icon;
            resultEquipmentNameTxt.text = resultEquipmentData.itemName;
            resultEquipmentAmountTxt.text = $"{currentResultCount} (+{materialCount})"; // 합성 후 결과 수량
        }
    }

    private BaseItemDataSO GetNextEquipmentData()
    {
        // 합성 후 결과 아이템을 가져옵니다.
        if (currentEquipment is Weapon weapon)
        {
            return DataManager.Instance.GetNextWeapon(weapon.BaseData.grade, weapon.BaseData.rank);
        }
        else if (currentEquipment is Accessory accessory)
        {
            return DataManager.Instance.GetNextAccessory(accessory.BaseData.grade, accessory.BaseData.rank);
        }

        return null;
    }

    private int GetResultItemCount()
    {
        var playerInventory = PlayerObjManager.Instance.Player.inventory;
        int currentResultCount = 0;

        // currentEquipment가 IEnhanceable을 구현하는지 확인
        if (currentEquipment is IEnhanceable enhanceable)
        {
            if (isWeapon)
            {
                foreach (var weapon in playerInventory.WeaponInventory.GetAllItems())
                {
                    if (weapon.BaseData == resultEquipmentData)
                    {
                        currentResultCount = weapon.StackCount;
                        break;
                    }
                }
            }
            else
            {
                foreach (var accessory in playerInventory.AccessoryInventory.GetAllItems())
                {
                    if (accessory.BaseData == resultEquipmentData)
                    {
                        currentResultCount = accessory.StackCount;
                        break;
                    }
                }
            }
        }
        
        return currentResultCount;
    }

    private void PerformFusion()
    {
        if (currentEquipment == null || materialCount <= 0 || resultEquipmentData == null)
        {
            return;
        }

        int totalRequiredMaterials = materialCount * RequiredMaterialsPerFusion;

        if (maxMaterials < totalRequiredMaterials)
        {
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
            var playerInventory = PlayerObjManager.Instance.Player.inventory;

            if (isWeapon)
            {
                playerInventory.NotifyWeaponsChanged();
            }
            else
            {
                playerInventory.NotifyAccessoriesChanged();
            }

            ClosePopup();
        }        
    }

    private void IncreaseMaterialCount()
    {
        if (materialCount < maxMaterials / RequiredMaterialsPerFusion)
        {
            materialCount++;
            materialCountTxt.text = materialCount.ToString();
            InitializeUI();
        }
    }

    private void DecreaseMaterialCount()
    {
        if (materialCount > 1)
        {
            materialCount--;
            materialCountTxt.text = materialCount.ToString();
            InitializeUI();
        }
    }

    private void SelectPreviousItem()
    {
        var playerInventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weaponList = playerInventory.WeaponInventory.GetAllItems();
            int currentIndex = weaponList.IndexOf(currentEquipment as Weapon);

            if (currentIndex > 0)
            {
                var previousWeapon = weaponList[currentIndex - 1];
                SetEquipmentData(previousWeapon, DataManager.Instance.GetNextWeapon(previousWeapon.BaseData.grade, previousWeapon.BaseData.rank));
            }            
        }
        else
        {
            var accessoryList = playerInventory.AccessoryInventory.GetAllItems();
            int currentIndex = accessoryList.IndexOf(currentEquipment as Accessory);

            if (currentIndex > 0)
            {
                var previousAccessory = accessoryList[currentIndex - 1];
                SetEquipmentData(previousAccessory, DataManager.Instance.GetNextAccessory(previousAccessory.BaseData.grade, previousAccessory.BaseData.rank));
            }
        }

        InitializeUI();
    }

    private void SelectNextItem()
    {
        var playerInventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weaponList = playerInventory.WeaponInventory.GetAllItems();
            int currentIndex = weaponList.IndexOf(currentEquipment as Weapon);

            if (currentIndex < weaponList.Count - 1)
            {
                var nextWeapon = weaponList[currentIndex + 1];
                SetEquipmentData(nextWeapon, DataManager.Instance.GetNextWeapon(nextWeapon.BaseData.grade, nextWeapon.BaseData.rank));
            }            
        }
        else
        {
            var accessoryList = playerInventory.AccessoryInventory.GetAllItems();
            int currentIndex = accessoryList.IndexOf(currentEquipment as Accessory);

            if (currentIndex < accessoryList.Count - 1)
            {
                var nextAccessory = accessoryList[currentIndex + 1];
                SetEquipmentData(nextAccessory, DataManager.Instance.GetNextAccessory(nextAccessory.BaseData.grade, nextAccessory.BaseData.rank));
            }            
        }

        InitializeUI();
    }

    private void ReturnToUpgradeUI()
    {
        Hide();
        var upgradePopup = UIManager.Instance.Show<EquipmentUpgradePopupUI>();
        if (upgradePopup != null)
        {
            upgradePopup.SetEquipmentData(currentEquipment as IEnhanceable, isWeapon);
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
}