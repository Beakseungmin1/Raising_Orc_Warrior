using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventorySlotManager : UIBase
{
    [SerializeField] private GameObject equipmentSlotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int initialSlotCount = 8;

    private List<EquipmentInventorySlot> inventorySlots = new List<EquipmentInventorySlot>();
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerobjManager.Instance?.Player?.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("[EquipmentInventorySlotManager] PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        CreateSlotsIfNeeded(initialSlotCount);
        playerInventory.OnWeaponsChanged += UpdateWeaponInventory;
        UpdateWeaponInventory();
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnWeaponsChanged -= UpdateWeaponInventory;
        }
    }

    private void CreateSlotsIfNeeded(int requiredSlotCount)
    {
        int currentSlotCount = inventorySlots.Count;
        for (int i = currentSlotCount; i < requiredSlotCount; i++)
        {
            GameObject slotObj = Instantiate(equipmentSlotPrefab, contentParent);
            EquipmentInventorySlot slot = slotObj.GetComponent<EquipmentInventorySlot>();
            if (slot != null)
            {
                inventorySlots.Add(slot);
            }
        }
    }

    private void UpdateWeaponInventory()
    {
        List<Weapon> weaponList = playerInventory.WeaponInventory.GetAllItems();
        CreateSlotsIfNeeded(weaponList.Count);

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < weaponList.Count)
            {
                var weapon = weaponList[i];
                int currentAmount = playerInventory.WeaponInventory.GetItemStackCount(weapon);
                int requiredAmount = weapon.BaseData.requiredCurrencyForUpgrade;

                inventorySlots[i].InitializeSlot(weapon, currentAmount, requiredAmount, this);
            }
            else
            {
                inventorySlots[i].InitializeSlot(null, 0, 0, this);
            }
        }
    }
}