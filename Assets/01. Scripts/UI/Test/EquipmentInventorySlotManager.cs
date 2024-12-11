using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventorySlotManager : UIBase
{
    [SerializeField] private GameObject equipmentSlotPrefab;
    [SerializeField] private Transform contentParent;

    [Header("Tabs")]
    [SerializeField] private Button weaponTabButton;
    [SerializeField] private Button accessoryTabButton;

    private List<EquipmentInventorySlot> inventorySlots = new List<EquipmentInventorySlot>();
    private List<IEnhanceable> weaponItems = new List<IEnhanceable>();
    private List<IEnhanceable> accessoryItems = new List<IEnhanceable>();
    private PlayerInventory playerInventory;
    private bool isWeaponTabActive = true;

    private void Start()
    {
        playerInventory = PlayerObjManager.Instance?.Player?.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("[EquipmentInventorySlotManager] PlayerInventory�� ã�� �� �����ϴ�.");
            return;
        }

        weaponTabButton.onClick.AddListener(() => ShowTab(true));
        accessoryTabButton.onClick.AddListener(() => ShowTab(false));

        // ��ü ������ �������� ���� �ʱ�ȭ
        InitializeSlots();

        // �κ��丮 ��ȭ �̺�Ʈ ����
        playerInventory.OnWeaponsChanged += UpdateInventorySlots;
        playerInventory.OnAccessoriesChanged += UpdateInventorySlots;

        ShowTab(true); // �⺻ ���� �� Ȱ��ȭ
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnWeaponsChanged -= UpdateInventorySlots;
            playerInventory.OnAccessoriesChanged -= UpdateInventorySlots;
        }
    }

    private void InitializeSlots()
    {
        // DataManager���� ������ ��������
        var weaponDataList = DataManager.Instance.GetAllWeapons();
        var accessoryDataList = DataManager.Instance.GetAllAccessories();

        // ����� �Ǽ��縮 �����͸� ��ް� ��ũ ������ �°� ����
        weaponDataList.Sort((a, b) =>
        {
            int gradeComparison = a.grade.CompareTo(b.grade); // ��� ���� ��������
            if (gradeComparison == 0)
            {
                return b.rank.CompareTo(a.rank); // ���� ����̸� ��ũ �������� (4 -> 3 -> 2 -> 1)
            }
            return gradeComparison;
        });

        accessoryDataList.Sort((a, b) =>
        {
            int gradeComparison = a.grade.CompareTo(b.grade); // ��� ���� ��������
            if (gradeComparison == 0)
            {
                return b.rank.CompareTo(a.rank); // ���� ����̸� ��ũ �������� (4 -> 3 -> 2 -> 1)
            }
            return gradeComparison;
        });

        int totalSlots = weaponDataList.Count + accessoryDataList.Count;

        // ���� ����
        CreateSlotsIfNeeded(totalSlots);

        int slotIndex = 0;

        // ���� ������ ���� �Ҵ�
        foreach (var weaponData in weaponDataList)
        {
            inventorySlots[slotIndex].AssignItem(new Weapon(weaponData), true); // ���� ���� �ʱ�ȭ
            slotIndex++;
        }

        // �Ǽ��縮 ������ ���� �Ҵ�
        foreach (var accessoryData in accessoryDataList)
        {
            inventorySlots[slotIndex].AssignItem(new Accessory(accessoryData), false); // �׼����� ���� �ʱ�ȭ
            slotIndex++;
        }

        // ���� ���� �ʱ�ȭ
        for (int i = slotIndex; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].ClearSlot();
        }
    }

    private void CreateSlotsIfNeeded(int requiredSlotCount)
    {
        int currentSlotCount = inventorySlots.Count;

        for (int i = currentSlotCount; i < requiredSlotCount; i++)
        {
            GameObject slotObj = Instantiate(equipmentSlotPrefab, contentParent);
            var slot = slotObj.GetComponent<EquipmentInventorySlot>();
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
        foreach (var slot in inventorySlots)
        {
            if (isWeaponTabActive && slot.IsWeaponSlot)
            {
                // ���� StackCount�� �����ͼ� ������Ʈ
                int stackCount = playerInventory.GetItemStackCount(slot.Item as Weapon);
                slot.UpdateSlotState(stackCount);
                slot.gameObject.SetActive(true);
            }
            else if (!isWeaponTabActive && !slot.IsWeaponSlot)
            {
                // ���� StackCount�� �����ͼ� ������Ʈ
                int stackCount = playerInventory.GetItemStackCount(slot.Item as Accessory);
                slot.UpdateSlotState(stackCount);
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false); // ���� �ǰ� ���� ���� ���� ��Ȱ��ȭ
            }
        }
    }
}