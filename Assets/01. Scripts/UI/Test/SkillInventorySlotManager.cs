using System.Collections.Generic;
using UnityEngine;

public class SkillInventorySlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;

    private List<SkillInventorySlot> inventorySlots = new List<SkillInventorySlot>();
    private PlayerInventory playerInventory;
    private EquipManager equipManager;

    private void Start()
    {
        var player = PlayerObjManager.Instance.Player;
        playerInventory = player?.inventory;
        equipManager = player?.EquipManager;

        if (playerInventory == null || equipManager == null)
        {
            return;
        }

        InitializeSlots();
        UpdateSkillSlots();

        playerInventory.OnSkillsChanged += UpdateSkillSlots;
        equipManager.OnSkillEquippedChanged += UpdateEquipStates;
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnSkillsChanged -= UpdateSkillSlots;
        }

        if (equipManager != null)
        {
            equipManager.OnSkillEquippedChanged -= UpdateEquipStates;
        }
    }

    private void InitializeSlots()
    {
        List<SkillDataSO> allSkills = DataManager.Instance.GetAllSkills();
        allSkills.Sort((a, b) => a.grade.CompareTo(b.grade));

        foreach (var slot in inventorySlots)
        {
            Destroy(slot.gameObject);
        }
        inventorySlots.Clear();

        foreach (var skillDataSO in allSkills)
        {
            GameObject slotObj = Instantiate(skillSlotPrefab, contentParent);
            SkillInventorySlot slot = slotObj.GetComponent<SkillInventorySlot>();
            if (slot != null)
            {
                BaseSkill ownedSkill = playerInventory.SkillInventory.GetItem(skillDataSO.itemName);
                int currentAmount = ownedSkill != null ? Mathf.Max(0, playerInventory.GetItemStackCount(ownedSkill) - 1) : 0;
                int requiredAmount = ownedSkill != null ? ownedSkill.GetRuntimeRequiredSkillCards() : 1;
                bool isEquipped = ownedSkill != null && equipManager.EquippedSkills.Contains(ownedSkill);

                slot.InitializeSlot(ownedSkill, skillDataSO, currentAmount, requiredAmount, isEquipped);
                inventorySlots.Add(slot);
            }
        }
    }

    private void UpdateSkillSlots()
    {
        foreach (var slot in inventorySlots)
        {
            SkillDataSO skillDataSO = slot.GetSkillDataSO();
            BaseSkill ownedSkill = playerInventory.SkillInventory.GetItem(skillDataSO?.itemName);

            int currentAmount = ownedSkill != null ? Mathf.Max(0, playerInventory.GetItemStackCount(ownedSkill) - 1) : 0;
            int requiredAmount = ownedSkill != null ? ownedSkill.GetRuntimeRequiredSkillCards() : 1;
            bool isEquipped = ownedSkill != null && equipManager.EquippedSkills.Contains(ownedSkill);

            slot.InitializeSlot(ownedSkill, skillDataSO, currentAmount, requiredAmount, isEquipped);

            if (isEquipped)
            {
                slot.SetEquippedState(true);
            }
        }
    }

    private void UpdateEquipStates(BaseSkill skill, int slotIndex, bool isEquipped)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.SkillData == skill)
            {
                slot.SetEquippedState(isEquipped);
                break;
            }
        }
    }
}