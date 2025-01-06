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
    public SpriteRenderer WeaponImage;

    public event Action OnEquippedChanged;
    public event Action OnWaitingSkillChanged;
    public event Action<BaseSkill, int, bool> OnSkillEquippedChanged;

    private PlayerStat playerStat;
    private PlayerSkillHandler skillHandler;
    private PlayerStatCalculator playerStatCalculator;

    private void Start()
    {
        playerStat = PlayerObjManager.Instance?.Player.stat;
        skillHandler = PlayerObjManager.Instance?.Player.SkillHandler;
        playerStatCalculator = PlayerObjManager.Instance?.Player.StatCalculator;

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
        WeaponImage.sprite = weaponData.BaseData.inGameImage;

        OnEquippedChanged?.Invoke();
    }

    public void EquipAccessory(Accessory accessory)
    {
        if (accessory == null) return;

        EquippedAccessory = accessory;
        playerStatCalculator.UpdateValue();

        OnEquippedChanged?.Invoke();
    }

    public void EquipSkill(BaseSkill skill, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            return;
        }

        int existingSlotIndex = EquippedSkills.IndexOf(skill);
        if (existingSlotIndex != -1 && existingSlotIndex != slotIndex)
        {
            UnequipSkill(existingSlotIndex);
        }

        if (EquippedSkills[slotIndex] != null)
        {
            UnequipSkill(slotIndex);
        }

        EquippedSkills[slotIndex] = skill;
        skill.IsEquipped = true;

        skill.ResetCondition();

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

        skill.Deactivate();
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