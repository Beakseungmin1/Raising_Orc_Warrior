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
        playerStat = PlayerObjManager.Instance?.Player.stat;
        skillHandler = PlayerObjManager.Instance?.Player.GetComponent<PlayerSkillHandler>();

        if (playerStat == null || skillHandler == null)
        {
            Debug.LogError("[EquipManager] PlayerStat 또는 PlayerSkillHandler를 찾을 수 없습니다.");
        }

        InitializeSkillSlots(0); // 초기화 메서드 호출
    }

    public bool IsWeaponEquipped(Weapon weapon)
    {
        return EquippedWeapon == weapon;
    }

    public bool IsAccessoryEquipped(Accessory accessory)
    {
        return EquippedAccessory == accessory;
    }

    public void InitializeSkillSlots(int slotCount)
    {
        EquippedSkills = new List<Skill>();
        for (int i = 0; i < slotCount; i++)
        {
            EquippedSkills.Add(null);
        }

        Debug.Log($"[EquipManager] 슬롯 초기화 완료. 슬롯 개수: {slotCount}");
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

        Debug.Log($"[EquipManager] 슬롯 개수가 {newSlotCount}개로 업데이트되었습니다.");
    }

    public void EquipWeapon(Weapon weaponData)
    {
        if (weaponData == null) return;

        if (EquippedWeapon != null)
        {
            RemoveWeaponEffect(EquippedWeapon);
        }

        EquippedWeapon = weaponData;

        Debug.Log($"[EquipManager] 무기 {weaponData.BaseData.itemName} 장착 완료.");
        OnEquippedChanged?.Invoke();
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null) return;

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;

        Debug.Log("[EquipManager] 무기 해제 완료.");
        OnEquippedChanged?.Invoke();
    }

    public void EquipAccessory(Accessory accessory)
    {
        if (accessory == null) return;

        if (EquippedAccessory != null)
        {
            RemoveAccessoryEffect(EquippedAccessory);
        }

        EquippedAccessory = accessory;

        Debug.Log($"[EquipManager] 악세사리 {accessory.BaseData.itemName} 장착 완료.");
        OnEquippedChanged?.Invoke();
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null) return;

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;

        Debug.Log("[EquipManager] 악세사리 해제 완료.");
        OnEquippedChanged?.Invoke();
    }

    public void EquipSkill(Skill skill, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            Debug.LogError("[EquipManager] 잘못된 슬롯 인덱스입니다.");
            return;
        }

        if (EquippedSkills[slotIndex] != null)
        {
            UnequipSkill(slotIndex);
        }

        EquippedSkills[slotIndex] = skill;

        skillHandler.SyncWithEquipManager();

        Debug.Log($"[EquipManager] 스킬 {skill.BaseData.itemName}이(가) 슬롯 {slotIndex}에 장착되었습니다.");
        OnEquippedChanged?.Invoke();
    }

    public void UnequipSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            Debug.LogError("[EquipManager] 잘못된 슬롯 인덱스입니다.");
            return;
        }

        Skill skill = EquippedSkills[slotIndex];
        if (skill == null) return;

        EquippedSkills[slotIndex] = null;

        skillHandler.SyncWithEquipManager();

        Debug.Log($"[EquipManager] 스킬 {skill.BaseData.itemName}이(가) 슬롯 {slotIndex}에서 해제되었습니다.");
        OnEquippedChanged?.Invoke();
    }

    public List<Skill> GetAllEquippedSkills()
    {
        return EquippedSkills.FindAll(skill => skill != null);
    }

    private void ApplyWeaponEffect(Weapon weapon)
    {
        // 무기 장착 효과를 PlayerStat에 반영
    }

    private void RemoveWeaponEffect(Weapon weapon)
    {
        // 무기 해제 효과를 PlayerStat에서 제거
    }

    private void ApplyAccessoryEffect(Accessory accessory)
    {
        // 악세사리 장착 효과를 PlayerStat에 반영
    }

    private void RemoveAccessoryEffect(Accessory accessory)
    {
        // 악세사리 해제 효과를 PlayerStat에서 제거
    }
}