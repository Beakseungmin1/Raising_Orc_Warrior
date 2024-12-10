using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public Weapon EquippedWeapon { get; private set; }
    public Accessory EquippedAccessory { get; private set; }
    public List<Skill> EquippedSkills { get; private set; } = new List<Skill>();

    public event Action OnEquippedChanged;

    private PlayerStat playerStat;
    private PlayerSkillHandler skillHandler; // PlayerSkillHandler 참조 추가

    private void Start()
    {
        playerStat = PlayerobjManager.Instance?.Player.stat;
        skillHandler = PlayerobjManager.Instance?.Player.GetComponent<PlayerSkillHandler>();

        InitializeSkillSlots(0); // 초기화 메서드 호출
    }

    public void InitializeSkillSlots(int slotCount)
    {
        EquippedSkills = new List<Skill>();
        for (int i = 0; i < slotCount; i++)
        {
            EquippedSkills.Add(null);
        }
    }

    public void UpdateSkillSlotCount(int newSlotCount)
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

    }

    public void EquipWeapon(WeaponDataSO weaponData)
    {
        if (weaponData == null) return;

        if (EquippedWeapon != null)
        {
            RemoveWeaponEffect(EquippedWeapon);
        }

        EquippedWeapon = new Weapon(weaponData);
        ApplyWeaponEffect(EquippedWeapon);

        OnEquippedChanged?.Invoke();
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null) return;

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;

        OnEquippedChanged?.Invoke();
    }

    public void EquipAccessory(AccessoryDataSO accessoryData)
    {
        if (accessoryData == null) return;

        if (EquippedAccessory != null)
        {
            RemoveAccessoryEffect(EquippedAccessory);
        }

        EquippedAccessory = new Accessory(accessoryData);
        ApplyAccessoryEffect(EquippedAccessory);

        OnEquippedChanged?.Invoke();
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null) return;

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;

        OnEquippedChanged?.Invoke();
    }

    public void EquipSkill(Skill skill, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            return;
        }

        if (EquippedSkills[slotIndex] != null)
        {
            UnequipSkill(slotIndex);
        }

        EquippedSkills[slotIndex] = skill;
        ApplySkillEffect(skill);

        // PlayerSkillHandler와 동기화
        skillHandler.SyncWithEquipManager();

        OnEquippedChanged?.Invoke();
    }

    public void UnequipSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            return;
        }

        Skill skill = EquippedSkills[slotIndex];
        if (skill == null) return;

        RemoveSkillEffect(skill);
        EquippedSkills[slotIndex] = null;

        // PlayerSkillHandler와 동기화
        skillHandler.SyncWithEquipManager();

        OnEquippedChanged?.Invoke();
    }

    public List<Skill> GetAllEquippedSkills()
    {
        return EquippedSkills.FindAll(skill => skill != null);
    }

    private void ApplyWeaponEffect(Weapon weapon)
    {
        // 무기 장착 효과를 PlayerStat에 반영
        // playerStat?.ApplyWeaponEffect(weapon.BaseData);
    }

    private void RemoveWeaponEffect(Weapon weapon)
    {
        // 무기 해제 효과를 PlayerStat에서 제거
        // playerStat?.RemoveWeaponEffect(weapon.BaseData);
    }

    private void ApplyAccessoryEffect(Accessory accessory)
    {
        // 악세사리 장착 효과를 PlayerStat에 반영
        // playerStat?.ApplyAccessoryEffect(accessory.BaseData);
    }

    private void RemoveAccessoryEffect(Accessory accessory)
    {
        // 악세사리 해제 효과를 PlayerStat에서 제거
        // playerStat?.RemoveAccessoryEffect(accessory.BaseData);
    }

    private void ApplySkillEffect(Skill skill)
    {
        // 스킬 장착 효과를 PlayerStat에 반영
        // playerStat?.ApplySkillEffect(skill);
    }

    private void RemoveSkillEffect(Skill skill)
    {
        // 스킬 해제 효과를 PlayerStat에서 제거
        // playerStat?.RemoveSkillEffect(skill);
    }
}