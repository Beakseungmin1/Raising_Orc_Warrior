using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentFusionPopupUI : UIBase
{
    [Header("Current Equipment Info")]
    [SerializeField] private Image currentEquipmentIcon;
    [SerializeField] private TextMeshProUGUI currentEquipmentNameTxt;
    [SerializeField] private TextMeshProUGUI currentEquipmentAmountTxt;
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;

    [Header("Fusion Result Equipment Info")]
    [SerializeField] private Image resultEquipmentIcon;
    [SerializeField] private TextMeshProUGUI resultEquipmentNameTxt;
    [SerializeField] private Button fusionBtn;

    [Header("Material Count Area")]
    [SerializeField] private TextMeshProUGUI materialCountTxt;
    [SerializeField] private Button subtractBtn;
    [SerializeField] private Button addBtn;

    [Header("Exit and Currency Info")]
    [SerializeField] private Button exitBtn;
    [SerializeField] private TextMeshProUGUI curCubeAmountTxt;
    [SerializeField] private Image curCubeIcon;

    [Header("Navigation Buttons")]
    [SerializeField] private Button upgradeReturnBtn;

    private object currentEquipment;
    private object resultEquipmentData;
    private int materialCount;
    private int maxMaterials;
    private bool isWeapon;

    private void Start()
    {
        exitBtn.onClick.AddListener(ClosePopup);
        fusionBtn.onClick.AddListener(PerformFusion);
        subtractBtn.onClick.AddListener(DecreaseMaterialCount);
        addBtn.onClick.AddListener(IncreaseMaterialCount);
        upgradeReturnBtn.onClick.AddListener(ReturnToUpgradeUI);
    }

    public void SetEquipmentData(object equipment, bool isWeaponType)
    {
        if (equipment == null)
        {
            Debug.LogError("[EquipmentFusionPopupUI] Equipment 데이터가 유효하지 않습니다.");
            return;
        }

        currentEquipment = equipment;
        isWeapon = isWeaponType;

        if (isWeaponType)
        {
            var weapon = equipment as Weapon;
            maxMaterials = weapon.StackCount;
            WeaponDataSO weaponData = weapon.BaseData;

            currentEquipmentIcon.sprite = weaponData.icon;
            currentEquipmentNameTxt.text = weaponData.itemName;
            currentEquipmentAmountTxt.text = $"x{weapon.StackCount}";

            resultEquipmentData = DataManager.Instance.GetNextWeapon(weaponData.grade, weaponData.rank);
            UpdateResultEquipmentUI(resultEquipmentData as WeaponDataSO);

            curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
            curCubeIcon.sprite = weaponData.currencyIcon;
        }
        else
        {
            var accessory = equipment as Accessory;
            maxMaterials = accessory.StackCount;
            AccessoryDataSO accessoryData = accessory.BaseData;

            currentEquipmentIcon.sprite = accessoryData.icon;
            currentEquipmentNameTxt.text = accessoryData.itemName;
            currentEquipmentAmountTxt.text = $"x{accessory.StackCount}";

            resultEquipmentData = DataManager.Instance.GetNextAccessory(accessoryData.grade, accessoryData.rank);
            UpdateResultEquipmentUI(resultEquipmentData as AccessoryDataSO);

            curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
            curCubeIcon.sprite = accessoryData.currencyIcon;
        }

        // materialCount를 기본적으로 최대치로 설정
        materialCount = maxMaterials;
        materialCountTxt.text = materialCount.ToString();
    }

    private void UpdateResultEquipmentUI(object resultData)
    {
        if (resultData == null)
        {
            resultEquipmentIcon.sprite = null;
            resultEquipmentNameTxt.text = "결합 결과 없음";
            return;
        }

        if (isWeapon && resultData is WeaponDataSO weaponData)
        {
            resultEquipmentIcon.sprite = weaponData.icon;
            resultEquipmentNameTxt.text = weaponData.itemName;
        }
        else if (!isWeapon && resultData is AccessoryDataSO accessoryData)
        {
            resultEquipmentIcon.sprite = accessoryData.icon;
            resultEquipmentNameTxt.text = accessoryData.itemName;
        }
    }

    private void PerformFusion()
    {
        if (currentEquipment == null || materialCount <= 0 || materialCount > maxMaterials || resultEquipmentData == null)
        {
            Debug.LogWarning("[EquipmentFusionPopupUI] 결합 조건이 충족되지 않았습니다.");
            return;
        }

        var playerInventory = PlayerobjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weapon = currentEquipment as Weapon;
            weapon.RemoveStack(materialCount);

            if (weapon.StackCount <= 0)
            {
                playerInventory.WeaponInventory.RemoveItem(weapon); // 런타임 객체 제거
            }

            playerInventory.NotifyWeaponsChanged(); // 슬롯 갱신
        }
        else
        {
            var accessory = currentEquipment as Accessory;
            accessory.RemoveStack(materialCount);

            if (accessory.StackCount <= 0)
            {
                playerInventory.AccessoryInventory.RemoveItem(accessory); // 런타임 객체 제거
            }

            playerInventory.NotifyAccessoriesChanged(); // 슬롯 갱신
        }

        Debug.Log("[EquipmentFusionPopupUI] 장비 합성 성공!");
        ClosePopup();
    }

    private void IncreaseMaterialCount()
    {
        if (materialCount < maxMaterials)
        {
            materialCount++;
            materialCountTxt.text = materialCount.ToString();
        }
    }

    private void DecreaseMaterialCount()
    {
        if (materialCount > 0)
        {
            materialCount--;
            materialCountTxt.text = materialCount.ToString();
        }
    }

    private void ReturnToUpgradeUI()
    {
        Hide();
        var upgradePopup = UIManager.Instance.Show<EquipmentUpgradePopupUI>();
        if (upgradePopup != null)
        {
            upgradePopup.SetEquipmentData(currentEquipment, isWeapon);
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
}