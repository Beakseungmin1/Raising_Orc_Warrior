using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public Weapon EquippedWeapon { get; private set; }
    public Accessory EquippedAccessory { get; private set; }
    public BaseSkill WaitingSkillForEquip { get; private set; }
    public List<BaseSkill> EquippedSkills { get; private set; } = new List<BaseSkill>();
    public int InitialSlotCount { get; private set; } = 8;

    public event Action OnEquippedChanged;
    public event Action OnWaitingSkillChanged;
    public event Action<BaseSkill, int, bool> OnSkillEquippedChanged;

    private PlayerStat playerStat;
    private PlayerSkillHandler skillHandler;

    private void Start()
    {
        playerStat = PlayerObjManager.Instance?.Player.stat;
        skillHandler = PlayerObjManager.Instance?.Player.SkillHandler;

        if (playerStat == null || skillHandler == null)
        {
            return;
        }
    }

    public void SetWaitingSkillForEquip(BaseSkill skill)
    {
        WaitingSkillForEquip = skill;
        OnWaitingSkillChanged?.Invoke();
    }

    public void ClearWaitingSkillForEquip()
    {
        WaitingSkillForEquip = null;
        OnWaitingSkillChanged?.Invoke();
    }

    public bool IsWeaponEquipped(Weapon weapon)
    {
        return EquippedWeapon == weapon;
    }

    public bool IsAccessoryEquipped(Accessory accessory)
    {
        return EquippedAccessory == accessory;
    }

    public void InitializeSkillSlots()
    {
        EquippedSkills.Clear();
        for (int i = 0; i < InitialSlotCount; i++)
        {
            EquippedSkills.Add(null);
        }
    }

    public void EquipWeapon(Weapon weaponData)
    {
        if (weaponData == null) return;

        EquippedWeapon = weaponData;

        OnEquippedChanged?.Invoke();
    }

    public void EquipAccessory(Accessory accessory)
    {
        if (accessory == null) return;

        EquippedAccessory = accessory;

        OnEquippedChanged?.Invoke();
    }

    public void EquipSkill(BaseSkill skill, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            Debug.LogWarning($"[EquipManager] 슬롯 인덱스 {slotIndex}가 유효하지 않습니다.");
            return;
        }

        // 기존에 스킬이 장착된 슬롯을 찾음
        int existingSlotIndex = EquippedSkills.IndexOf(skill);
        if (existingSlotIndex != -1)
        {
            Debug.Log($"[EquipManager] 스킬 {skill.SkillData.itemName}이 슬롯 {existingSlotIndex}에 이미 장착되어 있습니다. 제거 후 다시 장착합니다.");
            UnequipSkill(existingSlotIndex);
        }

        // 새로운 슬롯에 스킬 장착
        if (EquippedSkills[slotIndex] != null)
        {
            UnequipSkill(slotIndex);
        }

        EquippedSkills[slotIndex] = skill;
        skill.IsEquipped = true;

        Debug.Log($"[EquipManager] 슬롯 {slotIndex}에 스킬 {skill.SkillData.itemName} 장착 완료.");

        skillHandler.SyncWithEquipManager();

        OnSkillEquippedChanged?.Invoke(skill, slotIndex, true);

        ClearWaitingSkillForEquip();
    }

    public void UnequipSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            Debug.LogWarning($"[EquipManager] 슬롯 인덱스 {slotIndex}가 유효하지 않습니다.");
            return;
        }

        BaseSkill skill = EquippedSkills[slotIndex];
        if (skill == null) return;

        EquippedSkills[slotIndex] = null;
        skill.IsEquipped = false;

        Debug.Log($"[EquipManager] 슬롯 {slotIndex}에서 스킬 {skill.SkillData.itemName}이 제거되었습니다.");

        skillHandler.SyncWithEquipManager();

        OnSkillEquippedChanged?.Invoke(skill, slotIndex, false);
    }

    public List<BaseSkill> GetAllEquippedSkills()
    {
        return EquippedSkills.FindAll(skill => skill != null);
    }

    public void UpdateSlotCount(int newSlotCount)
    {
        if (newSlotCount > EquippedSkills.Count)
        {
            int slotsToAdd = newSlotCount - EquippedSkills.Count;
            for (int i = 0; i < slotsToAdd; i++)
            {
                EquippedSkills.Add(null);
            }
        }
        else if (newSlotCount < EquippedSkills.Count)
        {
            EquippedSkills.RemoveRange(newSlotCount, EquippedSkills.Count - newSlotCount);
        }
        InitialSlotCount = newSlotCount;
    }
}