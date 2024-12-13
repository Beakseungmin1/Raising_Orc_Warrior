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
        leftArrowBtn.onClick.AddListener(() => SelectAdjacentItem(-1));
        rightArrowBtn.onClick.AddListener(() => SelectAdjacentItem(1));
    }

    public void SetEquipmentData(IFusable equipment, BaseItemDataSO resultData = null)
    {
        if (equipment == null)
        {
            Debug.LogError("SetEquipmentData: currentEquipment is null!");
            return;
        }

        // ���� ��� ����
        currentEquipment = equipment;

        // �ֽ� ���� ī��Ʈ ��������
        maxMaterials = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentEquipment as IEnhanceable);

        // ��� ������ ����
        resultEquipmentData = resultData ?? GetNextEquipmentData(equipment);

        if (resultEquipmentData == null)
        {
            Debug.LogWarning("SetEquipmentData: resultEquipmentData is null.");
        }

        isWeapon = equipment is Weapon;

        // UI �ʱ�ȭ
        InitializeUI();
    }

    private void InitializeUI()
    {
        if (currentEquipment == null)
        {
            Debug.LogError("InitializeUI: currentEquipment is null!");
            return;
        }

        // �ֽ� ���� ī��Ʈ ��������
        maxMaterials = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentEquipment as IEnhanceable);
        materialCount = Mathf.Clamp(maxMaterials / RequiredMaterialsPerFusion, 1, maxMaterials);

        Debug.Log($"[InitializeUI] maxMaterials: {maxMaterials}, materialCount: {materialCount}");

        // UI ������Ʈ
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();

        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
    }

    private void UpdateCurrentEquipmentUI()
    {
        if (currentEquipment is IEnhanceable enhanceable)
        {
            currentEquipmentIcon.sprite = enhanceable.BaseData.icon;
            currentEquipmentNameTxt.text = enhanceable.BaseData.itemName;

            // UI ǥ�� �������� ���� 1���� ǥ��
            int uiStackCount = Mathf.Max(0, maxMaterials);
            materialCountTxt.text = materialCount.ToString();
            currentEquipmentAmountTxt.text = $"{uiStackCount} (-{materialCount * RequiredMaterialsPerFusion})";
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
            resultEquipmentNameTxt.text = "���� ��� ����";
            resultEquipmentAmountTxt.text = "0 (+0)";
        }
        else
        {
            int currentResultCount = GetResultItemCount();
            resultEquipmentIcon.sprite = resultEquipmentData.icon;
            resultEquipmentNameTxt.text = resultEquipmentData.itemName;
            resultEquipmentAmountTxt.text = $"{currentResultCount} (+{materialCount})";
        }
    }

    private int GetResultItemCount()
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        if (resultEquipmentData == null) return 0;

        return isWeapon
            ? inventory.WeaponInventory.GetAllItems().Find(x => x.BaseData == resultEquipmentData)?.StackCount ?? 0
            : inventory.AccessoryInventory.GetAllItems().Find(x => x.BaseData == resultEquipmentData)?.StackCount ?? 0;
    }

    private void AdjustMaterialCount(int amount)
    {
        maxMaterials = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentEquipment as IEnhanceable);

        materialCount = Mathf.Clamp(materialCount + amount, 1, maxMaterials / RequiredMaterialsPerFusion);
        UpdateCurrentEquipmentUI();
        UpdateResultEquipmentUI();
    }

    private void SelectAdjacentItem(int direction)
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        var items = isWeapon
            ? inventory.WeaponInventory.GetAllItems().Cast<IEnhanceable>().ToList()
            : inventory.AccessoryInventory.GetAllItems().Cast<IEnhanceable>().ToList();

        int currentIndex = items.IndexOf(currentEquipment as IEnhanceable);
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, items.Count - 1);

        if (newIndex != currentIndex)
        {
            var newItem = items[newIndex];
            SetEquipmentData(newItem as IFusable, GetNextEquipmentData(newItem as IFusable));
        }
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

    private void PerformFusion()
    {
        if (!CanPerformFusion()) return;

        bool success = ProcessFusion();
        if (success)
        {
            ClosePopup(); // �ռ� ���� �� �˾� �ݱ�
        }
    }

    private bool CanPerformFusion()
    {
        int actualStackCount = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentEquipment as IEnhanceable);

        // �ּ� 1�� ����� ����
        if (actualStackCount - materialCount < 1)
        {
            Debug.LogWarning("[CanPerformFusion] �ּ� 1���� ���ܾ� �մϴ�.");
            return false;
        }

        // �ռ��� �ʿ��� ��ᰡ ������� Ȯ��
        if (materialCount <= 0 || materialCount > actualStackCount)
        {
            Debug.LogWarning("[CanPerformFusion] �ռ��� �ʿ��� ��ᰡ �����մϴ�.");
            return false;
        }

        return true;
    }

    private bool ProcessFusion()
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        if (currentEquipment is Weapon weapon)
        {
            int actualStackCount = inventory.GetItemStackCount(weapon);

            if (weapon.Fuse(materialCount))
            {
                UpdateWeaponResult(inventory);

                if (actualStackCount - materialCount <= 0)
                {
                    inventory.RemoveItemFromInventory(weapon.BaseData);
                }

                return true;
            }
        }
        else if (currentEquipment is Accessory accessory)
        {
            int actualStackCount = inventory.GetItemStackCount(accessory);

            if (accessory.Fuse(materialCount))
            {
                UpdateAccessoryResult(inventory);

                if (actualStackCount - materialCount <= 0)
                {
                    inventory.RemoveItemFromInventory(accessory.BaseData);
                }

                return true;
            }
        }

        Debug.LogWarning("[ProcessFusion] �ռ� ����.");
        return false;
    }

    private void UpdateWeaponResult(PlayerInventory inventory)
    {
        var resultWeapon = inventory.WeaponInventory.GetAllItems()
            .FirstOrDefault(x => x.BaseData == resultEquipmentData);

        if (resultWeapon != null)
        {
            resultWeapon.AddStack(materialCount); // ���� ����
        }
        else
        {
            inventory.AddItemToInventory(resultEquipmentData);
        }
    }

    private void UpdateAccessoryResult(PlayerInventory inventory)
    {
        var resultAccessory = inventory.AccessoryInventory.GetAllItems()
            .FirstOrDefault(x => x.BaseData == resultEquipmentData);

        if (resultAccessory != null)
        {
            resultAccessory.AddStack(materialCount); // ���� ����
        }
        else
        {
            inventory.AddItemToInventory(resultEquipmentData);
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