using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillEquipSlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int initialSlotCount = 8;

    private List<SkillEquipSlot> equipSlots = new List<SkillEquipSlot>();
    private BaseSkill skillToEquip; // Skill → BaseSkill로 변경

    public event Action<BaseSkill> OnSkillEquipped;   // Skill → BaseSkill
    public event Action<BaseSkill> OnSkillUnequipped; // Skill → BaseSkill
    public event Action<List<BaseSkill>> OnSlotsUpdated;

    private EquipManager equipManager;

    public int SlotCount => equipSlots.Count;

    private void Start()
    {
        equipManager = PlayerObjManager.Instance.Player.GetComponent<EquipManager>();
        if (equipManager == null)
        {
            Debug.LogError("[SkillEquipSlotManager] EquipManager를 찾을 수 없습니다.");
            return;
        }

        CreateSlots(initialSlotCount);
        SyncEquipManager();
    }

    private void CreateSlots(int slotCount)
    {
        foreach (var slot in equipSlots)
        {
            Destroy(slot.gameObject);
        }
        equipSlots.Clear();

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(skillSlotPrefab, contentParent);
            SkillEquipSlot slot = slotObj.GetComponent<SkillEquipSlot>();
            if (slot != null)
            {
                slot.InitializeSlot(i, this);
                equipSlots.Add(slot);
            }
        }

        NotifySlotsUpdated();
        equipManager?.UpdateSkillSlotCount(SlotCount);
    }

    public void PrepareSkillForEquip(BaseSkill skill) // Skill → BaseSkill
    {
        skillToEquip = skill;
    }

    public void TryEquipSkillToSlot(int slotIndex)
    {
        if (skillToEquip != null && slotIndex >= 0 && slotIndex < equipSlots.Count)
        {
            BaseSkill previouslyEquippedSkill = equipSlots[slotIndex].GetEquippedSkill();
            if (previouslyEquippedSkill != null)
            {
                RemoveSkillFromPreviousSlot(previouslyEquippedSkill);
            }

            if (IsSkillEquipped(skillToEquip.SkillData))
            {
                RemoveSkillFromPreviousSlot(skillToEquip);
            }

            equipSlots[slotIndex].EquipSkill(skillToEquip);

            equipManager.EquipSkill(skillToEquip, slotIndex);

            OnSkillEquipped?.Invoke(skillToEquip);
            NotifySlotsUpdated();

            skillToEquip = null;
        }
    }

    private void RemoveSkillFromPreviousSlot(BaseSkill skill)
    {
        foreach (var slot in equipSlots)
        {
            if (slot.GetEquippedSkill() == skill)
            {
                slot.EquipSkill(null);

                int slotIndex = equipSlots.IndexOf(slot);
                equipManager.UnequipSkill(slotIndex);

                OnSkillUnequipped?.Invoke(skill);
                NotifySlotsUpdated();
                return;
            }
        }
    }

    public bool HasSkillToEquip()
    {
        return skillToEquip != null;
    }

    public bool IsSkillEquipped(SkillDataSO skillData)
    {
        foreach (var slot in equipSlots)
        {
            if (slot.GetEquippedSkill()?.SkillData == skillData)
                return true;
        }
        return false;
    }

    private void NotifySlotsUpdated()
    {
        List<BaseSkill> updatedSkills = new List<BaseSkill>();
        foreach (var slot in equipSlots)
        {
            if (slot.GetEquippedSkill() != null)
            {
                updatedSkills.Add(slot.GetEquippedSkill());
            }
        }

        OnSlotsUpdated?.Invoke(updatedSkills);
    }

    private void SyncEquipManager()
    {
        foreach (var slot in equipSlots)
        {
            var equippedSkill = slot.GetEquippedSkill();
            if (equippedSkill != null)
            {
                int slotIndex = equipSlots.IndexOf(slot);
                equipManager.EquipSkill(equippedSkill, slotIndex);
            }
        }
    }

    public List<BaseSkill> GetAllEquippedSkills()
    {
        List<BaseSkill> equippedSkills = new List<BaseSkill>();
        foreach (var slot in equipSlots)
        {
            var skill = slot.GetEquippedSkill();
            if (skill != null)
            {
                equippedSkills.Add(skill);
            }
        }
        return equippedSkills;
    }
}