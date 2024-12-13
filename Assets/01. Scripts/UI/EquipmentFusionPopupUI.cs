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

        // ��� ������ ����: �⺻���� GetNextEquipmentData�� ����
        resultEquipmentData = resultData ?? GetNextEquipmentData(equipment);

        // ��� �����Ͱ� ���� ��� ���� ó��
        if (resultEquipmentData == null)
        {
            Debug.LogWarning("[SetEquipmentData] resultEquipmentData�� �������� �ʾҽ��ϴ�.");
            return;
        }

        isWeapon = equipment is Weapon;

        // UI �ʱ�ȭ
        InitializeUI();
        UpdateNavigationButtons();
    }

    private void InitializeUI()
    {
        // ���� ������ ���� ī��Ʈ �������� (�ּ� 1���� ����)
        maxMaterials = Mathf.Max(0, PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentEquipment as IEnhanceable) - 1);
        materialCount = Mathf.Clamp(maxMaterials / RequiredMaterialsPerFusion, 1, maxMaterials);

        // ��� ������ �ֽ�ȭ (�׻� �ֽ� ���� ����)
        resultEquipmentData = GetNextEquipmentData(currentEquipment);

        // UI �ʱ�ȭ
        UpdateMaterialCount();
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        UpdateFusionButtonState();

        // ���� ��ȭ ǥ��
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency<float>(CurrencyType.Cube).ToString();
    }

    private void UpdateMaterialCount()
    {
        // MaterialCount�� ���õ� UI�� ������Ʈ
        materialCountTxt.text = materialCount.ToString();
    }

    private void UpdateCurrentEquipmentUI()
    {
        if (currentEquipment is IEnhanceable enhanceable)
        {
            currentEquipmentIcon.sprite = enhanceable.BaseData.icon;
            currentEquipmentIcon.color = maxMaterials > 0 ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // ������ ó��
            currentEquipmentNameTxt.text = enhanceable.BaseData.itemName;
            currentEquipmentAmountTxt.text = $"{maxMaterials} (-{materialCount})"; // �� ���ÿ��� 1���� ���ؼ� ǥ��
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
            resultEquipmentIcon.color = new Color(0.2f, 0.2f, 0.2f, 1f); // ������ ó��
            resultEquipmentNameTxt.text = "���� ��� ����";
            resultEquipmentAmountTxt.text = "0 (+0)";
        }
        else
        {
            int currentResultCount = GetResultItemCount();
            resultEquipmentIcon.sprite = resultEquipmentData.icon;
            resultEquipmentIcon.color = currentResultCount > 0 ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // ������ ó��
            resultEquipmentNameTxt.text = resultEquipmentData.itemName;
            resultEquipmentAmountTxt.text = $"{currentResultCount} (+{materialCount})";
        }
    }

    private void UpdateFusionButtonState()
    {
        bool canFuse = CanPerformFusion();
        fusionBtn.gameObject.SetActive(canFuse); // ���ǿ� ���� ��ư ���� ó��
    }

    private void AdjustMaterialCount(int amount)
    {
        // ��� ī��Ʈ ��� (0���� ���)
        materialCount = Mathf.Clamp(materialCount + amount, 0, maxMaterials);

        // �ؽ�Ʈ ������Ʈ
        materialCountTxt.text = materialCount.ToString();

        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
        // Fusion ��ư�� ���¸� �׻� ����
    }

    private void SelectPreviousItem()
    {
        var dataList = isWeapon
            ? DataManager.Instance.GetAllWeapons().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList()
            : DataManager.Instance.GetAllAccessories().OrderBy(item => item.grade).ThenByDescending(item => item.rank).Cast<BaseItemDataSO>().ToList();

        int currentIndex = dataList.FindIndex(item => item == (currentEquipment as IEnhanceable)?.BaseData);
        if (currentIndex == -1)
        {
            Debug.LogWarning("[SelectPreviousItem] ���� ��� ������ ����Ʈ�� �����ϴ�.");
            return;
        }

        if (currentIndex > 0) // �������� �̵� ������ ���
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
            Debug.LogWarning("[SelectNextItem] ���� ��� ������ ����Ʈ�� �����ϴ�.");
            return;
        }

        if (currentIndex < dataList.Count - 2) // ������ �ϳ� �ձ����� �̵� ����
        {
            var nextItem = dataList[currentIndex + 1];
            SetEquipmentData(CreateFusableItem(nextItem), GetNextEquipmentData(CreateFusableItem(nextItem)));
        }

        UpdateNavigationButtons(); // ��ư ���� ������Ʈ
    }

    private void UpdateNavigationButtons()
    {
        // ������ ����Ʈ ��������
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

        // ���� �������� �ε��� ���
        int currentIndex = dataList.FindIndex(item => item == (currentEquipment as IEnhanceable)?.BaseData);

        // ���� ������ ��ư�� ����ų� ǥ��
        leftArrowBtn.gameObject.SetActive(currentIndex > 0);               // ù ��° �����ۿ����� ���� ��ư ����
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 2); // ���������� �ϳ� �ձ����� ������ ��ư ǥ��
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
            Debug.LogWarning("[PerformFusion] �ռ� ������ �������� ����.");
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