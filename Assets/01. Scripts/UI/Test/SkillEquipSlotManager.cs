using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillEquipSlotManager : MonoBehaviour
{
    private List<SkillEquipSlot> equipSlots = new List<SkillEquipSlot>();
    private Skill skillToEquip;

    public event Action<Skill> OnSkillEquipped;
    public event Action<Skill> OnSkillUnequipped;
    public event Action<List<Skill>> OnSlotsUpdated;

    private EquipManager equipManager;

    private void Start()
    {
        equipManager = PlayerobjManager.Instance.Player.GetComponent<EquipManager>();
        if (equipManager == null)
        {
            Debug.LogError("[SkillEquipSlotManager] EquipManager를 찾을 수 없습니다.");
            return;
        }

        InitializeSlots();
        SyncEquipManager(); // EquipManager와 동기화
    }

    private void InitializeSlots()
    {
        equipSlots = new List<SkillEquipSlot>(GetComponentsInChildren<SkillEquipSlot>());

        for (int i = 0; i < equipSlots.Count; i++)
        {
            equipSlots[i].InitializeSlot(i, this);
        }

        Debug.Log($"[SkillEquipSlotManager] {equipSlots.Count}개의 슬롯이 초기화되었습니다.");
        NotifySlotsUpdated();
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

            equipSlots[slotIndex].EquipSkill(skillToEquip);

            equipManager.EquipSkill(skillToEquip, slotIndex);

            Debug.Log($"스킬 {skillToEquip.BaseData.itemName}이(가) 슬롯 {slotIndex}에 장착되었습니다.");

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

                Debug.Log($"스킬 {skill.BaseData.itemName}이(가) 이전 슬롯에서 제거되었습니다.");
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

    private void SyncEquipManager() // EquipManager의 스킬과 동기화
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

    public int GetSlotCount()
    {
        return equipSlots.Count;
    }
}