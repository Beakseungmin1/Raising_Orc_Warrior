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
    private EquipManager equipManager;

    private void Start()
    {
        playerInventory = PlayerObjManager.Instance?.Player?.inventory;
        equipManager = PlayerObjManager.Instance?.Player.EquipManager;
        if (playerInventory == null)
        {
            return;
        }

        weaponTabButton.onClick.AddListener(() => ShowTab(true));
        accessoryTabButton.onClick.AddListener(() => ShowTab(false));
        batchFusionBtn.onClick.AddListener(PerformBatchFusion);

        InitializeSlots();
        playerInventory.OnInventoryChanged += UpdateInventorySlots;
        equipManager.OnEquippedChanged += UpdateEquipStates;
        ShowTab(true);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= UpdateInventorySlots;
        }

        if (equipManager != null)
        {
            equipManager.OnEquippedChanged -= UpdateEquipStates;
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
            var existingWeapon = playerInventory.WeaponInventory.GetItem(weaponData.itemName);

            if (existingWeapon == null)
            {
                inventorySlots[slotIndex].AssignItem(new Weapon(weaponData), true);
            }

            else
            {
                inventorySlots[slotIndex].AssignItem(existingWeapon, true);
            }

            slotIndex++;
        }

        foreach (var accessoryData in accessoryDataList)
        {
            var existingAccessory = playerInventory.AccessoryInventory.GetItem(accessoryData.itemName);

            if (existingAccessory == null)
            {
                inventorySlots[slotIndex].AssignItem(new Accessory(accessoryData), false);
            }
            
            else
            {
                inventorySlots[slotIndex].AssignItem(existingAccessory, false);
            }
            
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

        UpdateEquipStates();
    }

    private void UpdateEquipStates()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.Item is Weapon weapon)
            {
                bool isWeaponEquipped = equipManager.EquippedWeapon?.BaseData == weapon.BaseData;
                slot.UpdateEquipState(isWeaponEquipped);
            }
            else if (slot.Item is Accessory accessory)
            {
                bool isAccessoryEquipped = equipManager.EquippedAccessory?.BaseData == accessory.BaseData;
                slot.UpdateEquipState(isAccessoryEquipped);
            }
        }
    }

    private void PerformBatchFusion()
    {
        bool anyFusionPerformed;

        do
        {
            anyFusionPerformed = false;

            var itemList = isWeaponTabActive
                ? playerInventory.WeaponInventory.GetAllItems().Cast<IFusable>().ToList()
                : playerInventory.AccessoryInventory.GetAllItems().Cast<IFusable>().ToList();

            foreach (var item in itemList)
            {
                if (item == null || item.StackCount <= 1)
                {
                    continue;
                }

                if (!CanFuseItem(item))
                {
                    continue;
                }

                int requiredCount = GetRequiredFuseItemCount(item);
                int usableStackCount = item.StackCount - 1;
                int maxFusionCount = usableStackCount / requiredCount;

                if (maxFusionCount > 0)
                {
                    for (int i = 0; i < maxFusionCount; i++)
                    {
                        bool success = item.Fuse(1);
                        if (success)
                        {
                            anyFusionPerformed = true;
                        }
                    }
                }
            }
        } while (anyFusionPerformed);

        UpdateInventorySlots(isWeaponTabActive);
    }

    private bool CanFuseItem(IFusable item)
    {
        const Grade MaxGradeToAllowFusion = Grade.Legendary;
        const int MinWeaponRankToAllowFusion = 2;
        const int MinAccessoryRankToAllowFusion = 2;

        var baseData = item.BaseData;
        if (baseData == null) return false;

        if (baseData.grade > MaxGradeToAllowFusion)
        {
            return false;
        }

        if (baseData is WeaponDataSO weaponData)
        {
            if (baseData.grade == Grade.Legendary)
            {
                return weaponData.rank >= MinWeaponRankToAllowFusion;
            }

            return true;
        }
        else if (baseData is AccessoryDataSO accessoryData)
        {
            if (baseData.grade == Grade.Legendary)
            {
                return accessoryData.rank >= MinAccessoryRankToAllowFusion;
            }

            return true;
        }

        return false;
    }


    private int GetRequiredFuseItemCount(IFusable item)
    {
        return item.BaseData is WeaponDataSO weaponData
            ? weaponData.requireFuseItemCount
            : (item.BaseData as AccessoryDataSO)?.requireFuseItemCount ?? 0;
    }
}