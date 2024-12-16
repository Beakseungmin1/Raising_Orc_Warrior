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

        // ������ resultEquipmentData�� �ֽ�ȭ
        resultEquipmentData = GetNextEquipmentData(currentEquipment);

        if (resultEquipmentData == null)
        {
            Debug.LogWarning("[SetEquipmentData] ���� ��� �����Ͱ� �����ϴ�. ��� �������� ǥ�õ˴ϴ�.");
        }

        InitializeUI(); // UI �ʱ�ȭ
        UpdateNavigationButtons(); // ȭ��ǥ ��ư ���� ������Ʈ
    }

    private void ClearUI()
    {
        // ��� UI ��Ҹ� �ʱ�ȭ ���·� ����
        currentEquipmentIcon.sprite = null;
        currentEquipmentNameTxt.text = "����";
        currentEquipmentAmountTxt.text = "0 (-0)";

        resultEquipmentIcon.sprite = null;
        resultEquipmentNameTxt.text = "��� ����";
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

        // �ʿ� ��� ������ �ִ� �ռ� ���� Ƚ�� ���
        int requiredCount = GetRequiredFuseItemCount();
        maxMaterials = currentEquipment.StackCount - 1;
        maxFusionCount = requiredCount > 0 ? maxMaterials / requiredCount : 0;

        // �ִ� �ռ� Ƚ���� materialCount �ʱ�ȭ
        materialCount = maxFusionCount;

        // UI ����
        UpdateMaterialCount();
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        UpdateFusionButtonState();

        // ��ȭ ǥ��
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

        // �κ��丮�� �����ϴ� ������� Ȯ��
        bool hasItem = currentEquipment.StackCount > 0;
        currentEquipmentIcon.color = hasItem ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // ������ ������ ó��

        currentEquipmentAmountTxt.text = $"{currentEquipment.StackCount - 1} (-{usedMaterials})";
    }

    private void UpdateResultEquipmentUI()
    {
        if (resultEquipmentData == null)
        {
            resultEquipmentIcon.sprite = null;
            resultEquipmentIcon.color = Color.white; // �⺻ ����
            resultEquipmentNameTxt.text = "��� ����";
            resultEquipmentAmountTxt.text = "0 (+0)";
        }
        else
        {
            resultEquipmentIcon.sprite = resultEquipmentData.icon;
            resultEquipmentNameTxt.text = resultEquipmentData.itemName;

            // �κ��丮�� �����ϴ��� Ȯ��
            int resultCount = GetResultItemCount();
            bool hasResultItem = resultCount > 0;
            resultEquipmentIcon.color = hasResultItem ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // ������ ������ ó��

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

        // ȭ��ǥ ��ư ���� ������Ʈ
        leftArrowBtn.gameObject.SetActive(currentIndex > 0); // ù ��° �������� �ƴϸ� ���� ��ư ǥ��
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 2); // ���������� -1������ �̵� ����
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
            return nextWeapon != weapon.BaseData ? nextWeapon : null; // �ڱ� �ڽ� ��ȯ ����
        }
        else if (equipment is Accessory accessory)
        {
            var nextAccessory = DataManager.Instance.GetNextAccessory(accessory.BaseData.grade, accessory.BaseData.rank);
            return nextAccessory != accessory.BaseData ? nextAccessory : null; // �ڱ� �ڽ� ��ȯ ����
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
            Debug.LogWarning("[PerformFusion] �ռ� ������ �������� ����.");
            return;
        }

        bool success = currentEquipment is Weapon weapon
            ? weapon.Fuse(materialCount)
            : (currentEquipment as Accessory)?.Fuse(materialCount) ?? false;

        if (success)
        {
            Debug.Log("[PerformFusion] �ռ� ����!");
            InitializeUI();
        }
        else
        {
            Debug.LogWarning("[PerformFusion] �ռ� ����!");
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