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
            Debug.LogError("EquipManager: PlayerStat�� ã�� �� �����ϴ�.");
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

        Debug.Log($"���� {weapon.itemName} ���� �Ϸ�.");
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null) return;

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;

        Debug.Log("���� ���� �Ϸ�.");
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

        Debug.Log($"�Ǽ��縮 {accessory.itemName} ���� �Ϸ�.");
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null) return;

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;

        Debug.Log("�Ǽ��縮 ���� �Ϸ�.");
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

        // ��ų ���� �̺�Ʈ Ʈ����
        OnEquippedSkillsChanged?.Invoke();

        Debug.Log($"��ų {skill.BaseData.itemName} ���� �Ϸ�.");
    }

    public void UnequipSkill()
    {
        if (EquippedSkill == null) return;

        RemoveSkillEffect(EquippedSkill);
        EquippedSkill = null;

        // ��ų ���� �̺�Ʈ Ʈ����
        OnEquippedSkillsChanged?.Invoke();

        Debug.Log("��ų ���� �Ϸ�.");
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