using System.Collections.Generic;
using UnityEngine;

public class SkillInventorySlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int initialSlotCount = 8;

    private List<SkillInventorySlot> inventorySlots = new List<SkillInventorySlot>();
    [SerializeField] private SkillEquipSlotManager skillEquipSlotManager;
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerObjManager.Instance?.Player?.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("[SkillInventorySlotManager] PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        CreateSlotsIfNeeded(initialSlotCount);

        Initialize(playerInventory, skillEquipSlotManager);

        skillEquipSlotManager.OnSkillEquipped += UpdateSkillSlotState;
        skillEquipSlotManager.OnSkillUnequipped += UpdateSkillSlotState;
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnSkillsChanged -= UpdateSkillInventory;
        }

        if (skillEquipSlotManager != null)
        {
            skillEquipSlotManager.OnSkillEquipped -= UpdateSkillSlotState;
            skillEquipSlotManager.OnSkillUnequipped -= UpdateSkillSlotState;
        }
    }

    private void CreateSlotsIfNeeded(int requiredSlotCount)
    {
        // 현재 슬롯 개수 확인
        int currentSlotCount = inventorySlots.Count;

        // 부족한 슬롯만 추가 생성
        for (int i = currentSlotCount; i < requiredSlotCount; i++)
        {
            GameObject slotObj = Instantiate(skillSlotPrefab, contentParent);
            SkillInventorySlot slot = slotObj.GetComponent<SkillInventorySlot>();
            if (slot != null)
            {
                inventorySlots.Add(slot);
            }
        }

        Debug.Log($"[SkillInventorySlotManager] {requiredSlotCount - currentSlotCount}개의 슬롯이 추가로 생성되었습니다.");
    }

    public void Initialize(PlayerInventory inventory, SkillEquipSlotManager equipSlotManager)
    {
        playerInventory = inventory;
        skillEquipSlotManager = equipSlotManager;

        playerInventory.OnSkillsChanged += UpdateSkillInventory;
        UpdateSkillInventory();
    }

    private void UpdateSkillInventory()
    {
        List<Skill> skillList = playerInventory.SkillInventory.GetAllItems();

        // 필요한 만큼 슬롯을 동적으로 생성
        CreateSlotsIfNeeded(skillList.Count);

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < skillList.Count)
            {
                var skill = skillList[i];
                int currentAmount = playerInventory.SkillInventory.GetItemStackCount(skill);
                int requiredAmount = skill.BaseData.requireSkillCardsForUpgrade;

                bool isEquipped = skillEquipSlotManager.IsSkillEquipped(skill.BaseData);

                inventorySlots[i].InitializeSlot(skill, currentAmount, requiredAmount, isEquipped, skillEquipSlotManager);
            }
            else
            {
                inventorySlots[i].InitializeSlot(null, 0, 0, false, skillEquipSlotManager);
            }
        }
    }

    private void UpdateSkillSlotState(Skill skill)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.MatchesSkill(skill.BaseData))
            {
                bool isEquipped = skillEquipSlotManager.IsSkillEquipped(skill.BaseData);
                slot.SetEquippedState(isEquipped);
                Debug.Log($"[SkillInventorySlotManager] 스킬 {skill.BaseData.itemName} 상태 업데이트: 장착 여부 - {isEquipped}");
                return;
            }
        }
    }
}