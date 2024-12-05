using System.Collections.Generic;
using UnityEngine;

public class SkillInventorySlotManager : UIBase
{
    [SerializeField] private List<SkillInventorySlot> inventorySlots;
    [SerializeField] private SkillEquipSlotManager skillEquipSlotManager;
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerobjManager.Instance?.Player?.inventory;
        Initialize(playerInventory, skillEquipSlotManager);

        // SkillEquipSlotManager �̺�Ʈ ����
        skillEquipSlotManager.OnSkillEquipped += UpdateSkillSlotState;
        skillEquipSlotManager.OnSkillUnequipped += UpdateSkillSlotState; // ���� ���� �̺�Ʈ ����
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
                Debug.Log($"[SkillInventorySlotManager] ��ų {skill.BaseData.itemName} ���� ������Ʈ: ���� ���� - {isEquipped}");
                return;
            }
        }
    }
}