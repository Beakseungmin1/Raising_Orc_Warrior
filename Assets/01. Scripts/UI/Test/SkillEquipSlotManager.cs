using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillEquipSlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private int initialSlotCount = 8;

    private List<SkillEquipSlot> equipSlots = new List<SkillEquipSlot>();
    private Skill skillToEquip;

    public event Action<Skill> OnSkillEquipped;
    public event Action<Skill> OnSkillUnequipped;
    public event Action<List<Skill>> OnSlotsUpdated;

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

    public void PrepareSkillForEquip(Skill skill)
    {
        skillToEquip = skill;
    }

    public void TryEquipSkillToSlot(int slotIndex)
    {
        if (skillToEquip != null && slotIndex >= 0 && slotIndex < equipSlots.Count)
        {
            Skill previouslyEquippedSkill = equipSlots[slotIndex].GetEquippedSkill();
            if (previouslyEquippedSkill != null)
            {
                RemoveSkillFromPreviousSlot(previouslyEquippedSkill);
            }

            if (IsSkillEquipped(skillToEquip.BaseData))
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

    private void RemoveSkillFromPreviousSlot(Skill skill)
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
            if (slot.GetEquippedSkill()?.BaseData == skillData)
                return true;
        }
        return false;
    }

    private void NotifySlotsUpdated()
    {
        List<Skill> updatedSkills = new List<Skill>();
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

    public List<Skill> GetAllEquippedSkills()
    {
        List<Skill> equippedSkills = new List<Skill>();
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