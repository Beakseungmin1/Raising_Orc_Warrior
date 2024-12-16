using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventorySlotManager : UIBase
{
    [SerializeField] private GameObject equipmentSlotPrefab;
    [SerializeField] private Transform contentParent;

    [Header("Tabs")]
    [SerializeField] private Button weaponTabButton;
    [SerializeField] private Button accessoryTabButton;

    [SerializeField] private Button batchFusionBtn;

    private List<EquipmentInventorySlot> inventorySlots = new List<EquipmentInventorySlot>();
    private PlayerInventory playerInventory;
    private bool isWeaponTabActive = true;

    private void Start()
    {
        playerInventory = PlayerObjManager.Instance?.Player?.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("[EquipmentInventorySlotManager] PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        weaponTabButton.onClick.AddListener(() => ShowTab(true));
        accessoryTabButton.onClick.AddListener(() => ShowTab(false));
        batchFusionBtn.onClick.AddListener(PerformBatchFusion);

        InitializeSlots();
        playerInventory.OnInventoryChanged += UpdateInventorySlots;
        ShowTab(true);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateInventorySlots;
        }
    }

    private void InitializeSlots()
    {
        var weaponDataList = DataManager.Instance.GetAllWeapons();
        var accessoryDataList = DataManager.Instance.GetAllAccessories();

        weaponDataList.Sort((a, b) =>
        {
            int gradeComparison = a.grade.CompareTo(b.grade);
            if (gradeComparison == 0)
            {
                return b.rank.CompareTo(a.rank);
            }
            return gradeComparison;
        });

        accessoryDataList.Sort((a, b) =>
        {
            int gradeComparison = a.grade.CompareTo(b.grade);
            if (gradeComparison == 0)
            {
                return b.rank.CompareTo(a.rank);
            }
            return gradeComparison;
        });

        int totalSlots = weaponDataList.Count + accessoryDataList.Count;

        if (inventorySlots.Count < totalSlots)
        {
            for (int i = inventorySlots.Count; i < totalSlots; i++)
            {
                GameObject slotObj = Instantiate(equipmentSlotPrefab, contentParent);
                var slot = slotObj.GetComponent<EquipmentInventorySlot>();
                if (slot != null)
                {
                    inventorySlots.Add(slot);
                }
            }
        }

        int slotIndex = 0;

        foreach (var weaponData in weaponDataList)
        {
            inventorySlots[slotIndex].AssignItem(new Weapon(weaponData), true);
            slotIndex++;
        }

        foreach (var accessoryData in accessoryDataList)
        {
            inventorySlots[slotIndex].AssignItem(new Accessory(accessoryData), false);
            slotIndex++;
        }

        for (int i = slotIndex; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].ClearSlot();
        }
    }

    private void ShowTab(bool showWeaponTab)
    {
        isWeaponTabActive = showWeaponTab;
        UpdateInventorySlots(isWeaponTabActive);
    }

    public void UpdateInventorySlots(bool isWeaponTab)
    {
        isWeaponTabActive = isWeaponTab;
        foreach (var slot in inventorySlots)
        {
            if (isWeaponTabActive && slot.IsWeaponSlot)
            {
                int stackCount = playerInventory.GetItemStackCount(slot.Item as Weapon);
                slot.UpdateSlotState(stackCount);
                slot.gameObject.SetActive(true);
            }
            else if (!isWeaponTabActive && !slot.IsWeaponSlot)
            {
                int stackCount = playerInventory.GetItemStackCount(slot.Item as Accessory);
                slot.UpdateSlotState(stackCount);
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    private void PerformBatchFusion()
    {
        var itemList = isWeaponTabActive
            ? playerInventory.WeaponInventory.GetAllItems().Cast<IFusable>().ToList()
            : playerInventory.AccessoryInventory.GetAllItems().Cast<IFusable>().ToList();

        bool anyFusionPerformed = false;

        foreach (var item in itemList)
        {
            if (item == null || item.StackCount <= 1)
            {
                Debug.LogWarning($"[PerformBatchFusion] 재료 부족 또는 조건 불충족: {item?.BaseData.itemName ?? "Unknown"}");
                continue;
            }

            int requiredCount = GetRequiredFuseItemCount(item);
            int usableStackCount = item.StackCount - 1; // **남길 재료 1개 제외**
            int maxFusionCount = usableStackCount / requiredCount; // 합성 가능한 최대 횟수

            if (maxFusionCount > 0)
            {
                Debug.Log($"[PerformBatchFusion] Attempting Fusion: {item.BaseData.itemName}, MaxFusionCount: {maxFusionCount}");

                bool success = item.Fuse(maxFusionCount); // **재료 개수를 정확히 전달**
                if (success)
                {
                    Debug.Log($"[PerformBatchFusion] 합성 성공: {item.BaseData.itemName}, 합성 횟수: {maxFusionCount}");
                    anyFusionPerformed = true;
                }
                else
                {
                    Debug.LogWarning($"[PerformBatchFusion] 합성 실패: {item.BaseData.itemName}");
                }
            }
            else
            {
                Debug.LogWarning($"[PerformBatchFusion] 재료 부족 또는 조건 불충족: {item.BaseData.itemName}");
            }
        }

        if (anyFusionPerformed)
        {
            Debug.Log("일괄 합성 완료!");
            UpdateInventorySlots(isWeaponTabActive); // UI 갱신
        }
        else
        {
            Debug.Log("합성 가능한 아이템이 없습니다.");
        }
    }

    private int GetRequiredFuseItemCount(IFusable item)
    {
        return item.BaseData is WeaponDataSO weaponData
            ? weaponData.requireFuseItemCount
            : (item.BaseData as AccessoryDataSO)?.requireFuseItemCount ?? 0;
    }

    private BaseItemDataSO GetNextEquipmentData(IFusable equipment)
    {
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
}