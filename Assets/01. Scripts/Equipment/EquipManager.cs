using System;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public WeaponDataSO EquippedWeapon { get; private set; }
    public AccessoryDataSO EquippedAccessory { get; private set; }
    public Skill EquippedSkill { get; private set; }

    public event Action OnEquippedSkillsChanged;

    private PlayerStat playerStat;

    private void Start()
    {
        playerStat = PlayerobjManager.Instance?.Player.stat;
        if (playerStat == null)
        {
            Debug.LogError("EquipManager: PlayerStat을 찾을 수 없습니다.");
        }
    }

    public void EquipWeapon(WeaponDataSO weapon)
    {
        if (weapon == null) return;

        if (EquippedWeapon != null)
        {
            RemoveWeaponEffect(EquippedWeapon);
        }

        EquippedWeapon = weapon;
        ApplyWeaponEffect(weapon);

        Debug.Log($"무기 {weapon.itemName} 장착 완료.");
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null) return;

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;

        Debug.Log("무기 해제 완료.");
    }

    public void EquipAccessory(AccessoryDataSO accessory)
    {
        if (accessory == null) return;

        if (EquippedAccessory != null)
        {
            RemoveAccessoryEffect(EquippedAccessory);
        }

        EquippedAccessory = accessory;
        ApplyAccessoryEffect(accessory);

        Debug.Log($"악세사리 {accessory.itemName} 장착 완료.");
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null) return;

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;

        Debug.Log("악세사리 해제 완료.");
    }

    public void EquipSkill(Skill skill)
    {
        if (skill == null) return;

        if (EquippedSkill != null)
        {
            RemoveSkillEffect(EquippedSkill);
        }

        EquippedSkill = skill;
        ApplySkillEffect(skill);

        // 스킬 변경 이벤트 트리거
        OnEquippedSkillsChanged?.Invoke();

        Debug.Log($"스킬 {skill.BaseData.itemName} 장착 완료.");
    }

    public void UnequipSkill()
    {
        if (EquippedSkill == null) return;

        RemoveSkillEffect(EquippedSkill);
        EquippedSkill = null;

        // 스킬 변경 이벤트 트리거
        OnEquippedSkillsChanged?.Invoke();

        Debug.Log("스킬 해제 완료.");
    }

    private void ApplyWeaponEffect(WeaponDataSO weapon)
    {
        //playerStat?.ApplyWeaponEffect(weapon);
    }

    private void RemoveWeaponEffect(WeaponDataSO weapon)
    {
        //playerStat?.RemoveWeaponEffect(weapon);
    }

    private void ApplyAccessoryEffect(AccessoryDataSO accessory)
    {
        //playerStat?.ApplyAccessoryEffect(accessory);
    }

    private void RemoveAccessoryEffect(AccessoryDataSO accessory)
    {
        //playerStat?.RemoveAccessoryEffect(accessory);
    }

    private void ApplySkillEffect(Skill skill)
    {
        //playerStat?.ApplySkillEffect(skill);
    }

    private void RemoveSkillEffect(Skill skill)
    {
        //playerStat?.RemoveSkillEffect(skill);
    }
}