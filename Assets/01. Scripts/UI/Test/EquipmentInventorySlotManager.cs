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
            if (item == null)
                continue;

            int stackCount = item.StackCount;
            int requiredCount = 0;

            // BaseDataSO에서 필요한 Fuse 카운트 가져오기
            if (item.BaseData is WeaponDataSO weaponData)
            {
                requiredCount = weaponData.requireFuseItemCount;
            }
            else if (item.BaseData is AccessoryDataSO accessoryData)
            {
                requiredCount = accessoryData.requireFuseItemCount;
            }

            // 최대 합성 가능 횟수 계산
            int maxFusionCount = stackCount / requiredCount;

            if (maxFusionCount > 0)
            {
                // 합성 실행
                bool success = item.Fuse(maxFusionCount * requiredCount);
                if (success)
                {
                    Debug.Log($"{item.BaseData.itemName}: {maxFusionCount}회 합성 완료");
                    anyFusionPerformed = true;
                }
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
}