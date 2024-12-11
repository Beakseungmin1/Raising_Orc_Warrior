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
            Debug.LogError("[EquipmentInventorySlotManager] PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        weaponTabButton.onClick.AddListener(() => ShowTab(true));
        accessoryTabButton.onClick.AddListener(() => ShowTab(false));

        // 전체 데이터 기준으로 슬롯 초기화
        InitializeSlots();

        // 인벤토리 변화 이벤트 구독
        playerInventory.OnWeaponsChanged += UpdateInventorySlots;
        playerInventory.OnAccessoriesChanged += UpdateInventorySlots;

        ShowTab(true); // 기본 무기 탭 활성화
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
        // DataManager에서 데이터 가져오기
        var weaponDataList = DataManager.Instance.GetAllWeapons();
        var accessoryDataList = DataManager.Instance.GetAllAccessories();

        // 무기와 악세사리 데이터를 등급과 랭크 순서에 맞게 정렬
        weaponDataList.Sort((a, b) =>
        {
            int gradeComparison = a.grade.CompareTo(b.grade); // 등급 기준 오름차순
            if (gradeComparison == 0)
            {
                return b.rank.CompareTo(a.rank); // 같은 등급이면 랭크 내림차순 (4 -> 3 -> 2 -> 1)
            }
            return gradeComparison;
        });

        accessoryDataList.Sort((a, b) =>
        {
            int gradeComparison = a.grade.CompareTo(b.grade); // 등급 기준 오름차순
            if (gradeComparison == 0)
            {
                return b.rank.CompareTo(a.rank); // 같은 등급이면 랭크 내림차순 (4 -> 3 -> 2 -> 1)
            }
            return gradeComparison;
        });

        int totalSlots = weaponDataList.Count + accessoryDataList.Count;

        // 슬롯 생성
        CreateSlotsIfNeeded(totalSlots);

        int slotIndex = 0;

        // 무기 데이터 슬롯 할당
        foreach (var weaponData in weaponDataList)
        {
            inventorySlots[slotIndex].AssignItem(new Weapon(weaponData), true); // 무기 슬롯 초기화
            slotIndex++;
        }

        // 악세사리 데이터 슬롯 할당
        foreach (var accessoryData in accessoryDataList)
        {
            inventorySlots[slotIndex].AssignItem(new Accessory(accessoryData), false); // 액세서리 슬롯 초기화
            slotIndex++;
        }

        // 남은 슬롯 초기화
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
                // 실제 StackCount를 가져와서 업데이트
                int stackCount = playerInventory.GetItemStackCount(slot.Item as Weapon);
                slot.UpdateSlotState(stackCount);
                slot.gameObject.SetActive(true);
            }
            else if (!isWeaponTabActive && !slot.IsWeaponSlot)
            {
                // 실제 StackCount를 가져와서 업데이트
                int stackCount = playerInventory.GetItemStackCount(slot.Item as Accessory);
                slot.UpdateSlotState(stackCount);
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false); // 현재 탭과 관련 없는 슬롯 비활성화
            }
        }
    }
}