using System.Collections.Generic;
using UnityEngine;

public class SkillInventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> skillSlots;
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerobjManager.Instance.Player.inventory;
        playerInventory.OnSkillsChanged += UpdateSkillInventory;
        UpdateSkillInventory();
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnSkillsChanged -= UpdateSkillInventory;
        }
    }

    private void UpdateSkillInventory()
    {
        List<SkillDataSO> skills = playerInventory.SkillInventory.GetAllItems();
        for (int i = 0; i < skillSlots.Count; i++)
        {
            if (i < skills.Count)
            {
                var skill = skills[i];
                int currentAmount = playerInventory.SkillInventory.GetItemStackCount(skill);
                int requiredAmount = skill.requireSkillCardsForUpgrade;
                skillSlots[i].InitializeSlot(skill, currentAmount, requiredAmount);
            }
            else
            {
                skillSlots[i].InitializeSlot(null, 0, 0);
            }
        }
    }
}