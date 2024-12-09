using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventorySlotManager : UIBase
{
    [SerializeField] private GameObject equipmentSlotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int initialSlotCount = 8;

    [Header("Tabs")]
    [SerializeField] private Button weaponTabButton;
    [SerializeField] private Button accessoryTabButton;

    private List<EquipmentInventorySlot> inventorySlots = new List<EquipmentInventorySlot>();
    private PlayerInventory playerInventory;
    private bool isWeaponTabActive = true;

    private void Start()
    {
        playerInventory = PlayerobjManager.Instance?.Player?.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("[EquipmentInventorySlotManager] PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        weaponTabButton.onClick.AddListener(() => ShowTab(true));
        accessoryTabButton.onClick.AddListener(() => ShowTab(false));

        CreateSlotsIfNeeded(initialSlotCount);
        playerInventory.OnWeaponsChanged += UpdateWeaponInventory;
        playerInventory.OnAccessoriesChanged += UpdateAccessoryInventory;

        ShowTab(true);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnWeaponsChanged -= UpdateWeaponInventory;
            playerInventory.OnAccessoriesChanged -= UpdateAccessoryInventory;
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

    private void ShowTab(bool showWeaponTab)
    {
        isWeaponTabActive = showWeaponTab;
        UpdateInventorySlots();
    }

    public void UpdateInventorySlots()
    {
        if (isWeaponTabActive)
        {
            UpdateWeaponInventory();
        }
        else
        {
            UpdateAccessoryInventory();
        }
    }

    private void UpdateWeaponInventory()
    {
        if (playerInventory == null) return;

        List<Weapon> weaponList = playerInventory.WeaponInventory.GetAllItems();
        CreateSlotsIfNeeded(weaponList.Count);

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < weaponList.Count)
            {
                var weapon = weaponList[i];
                int currentAmount = playerInventory.WeaponInventory.GetItemStackCount(weapon);
                inventorySlots[i].InitializeSlot(weapon, currentAmount, weapon.RequiredCurrencyForUpgrade, this, true);
            }
            else
            {
                inventorySlots[i].InitializeSlot(null, 0, 0, this, true);
            }
        }
    }

    private void UpdateAccessoryInventory()
    {
        if (playerInventory == null) return;

        List<Accessory> accessoryList = playerInventory.AccessoryInventory.GetAllItems();
        CreateSlotsIfNeeded(accessoryList.Count);

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < accessoryList.Count)
            {
                var accessory = accessoryList[i];
                int currentAmount = playerInventory.AccessoryInventory.GetItemStackCount(accessory);
                inventorySlots[i].InitializeSlot(accessory, currentAmount, accessory.RequiredCurrencyForUpgrade, this, false);
            }
            else
            {
                inventorySlots[i].InitializeSlot(null, 0, 0, this, false);
            }
        }
    }
}