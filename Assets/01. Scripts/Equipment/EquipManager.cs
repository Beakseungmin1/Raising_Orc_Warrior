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
    private PlayerSkillHandler skillHandler; // PlayerSkillHandler ���� �߰�

    private void Start()
    {
        playerStat = PlayerObjManager.Instance?.Player.stat;
        skillHandler = PlayerObjManager.Instance?.Player.GetComponent<PlayerSkillHandler>();

        if (playerStat == null || skillHandler == null)
        {
            Debug.LogError("[EquipManager] PlayerStat �Ǵ� PlayerSkillHandler�� ã�� �� �����ϴ�.");
        }

        InitializeSkillSlots(0); // �ʱ�ȭ �޼��� ȣ��
    }

    public void InitializeSkillSlots(int slotCount)
    {
        EquippedSkills = new List<Skill>();
        for (int i = 0; i < slotCount; i++)
        {
            EquippedSkills.Add(null);
        }

        Debug.Log($"[EquipManager] ���� �ʱ�ȭ �Ϸ�. ���� ����: {slotCount}");
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

        Debug.Log($"[EquipManager] ���� ������ {newSlotCount}���� ������Ʈ�Ǿ����ϴ�.");
    }

    public void EquipWeapon(Weapon weaponData)
    {
        if (weaponData == null) return;

        if (EquippedWeapon != null)
        {
            RemoveWeaponEffect(EquippedWeapon);
        }

        EquippedWeapon = weaponData;
        //ApplyWeaponEffect(EquippedWeapon);

        Debug.Log($"[EquipManager] ���� {weaponData.BaseData.itemName} ���� �Ϸ�.");
        OnEquippedChanged?.Invoke();
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null) return;

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;

        Debug.Log("[EquipManager] ���� ���� �Ϸ�.");
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

        Debug.Log($"[EquipManager] �Ǽ��縮 {accessoryData.itemName} ���� �Ϸ�.");
        OnEquippedChanged?.Invoke();
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null) return;

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;

        Debug.Log("[EquipManager] �Ǽ��縮 ���� �Ϸ�.");
        OnEquippedChanged?.Invoke();
    }

    public void EquipSkill(Skill skill, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            Debug.LogError("[EquipManager] �߸��� ���� �ε����Դϴ�.");
            return;
        }

        if (EquippedSkills[slotIndex] != null)
        {
            UnequipSkill(slotIndex);
        }

        EquippedSkills[slotIndex] = skill;
        ApplySkillEffect(skill);

        // PlayerSkillHandler�� ����ȭ
        skillHandler.SyncWithEquipManager();

        Debug.Log($"[EquipManager] ��ų {skill.BaseData.itemName}��(��) ���� {slotIndex}�� �����Ǿ����ϴ�.");
        OnEquippedChanged?.Invoke();
    }

    public void UnequipSkill(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= EquippedSkills.Count)
        {
            Debug.LogError("[EquipManager] �߸��� ���� �ε����Դϴ�.");
            return;
        }

        Skill skill = EquippedSkills[slotIndex];
        if (skill == null) return;

        RemoveSkillEffect(skill);
        EquippedSkills[slotIndex] = null;

        // PlayerSkillHandler�� ����ȭ
        skillHandler.SyncWithEquipManager();

        Debug.Log($"[EquipManager] ��ų {skill.BaseData.itemName}��(��) ���� {slotIndex}���� �����Ǿ����ϴ�.");
        OnEquippedChanged?.Invoke();
    }

    public List<Skill> GetAllEquippedSkills()
    {
        return EquippedSkills.FindAll(skill => skill != null);
    }

    private void ApplyWeaponEffect(Weapon weapon)
    {
        // ���� ���� ȿ���� PlayerStat�� �ݿ�
        // playerStat?.ApplyWeaponEffect(weapon.BaseData);
    }

    private void RemoveWeaponEffect(Weapon weapon)
    {
        // ���� ���� ȿ���� PlayerStat���� ����
        // playerStat?.RemoveWeaponEffect(weapon.BaseData);
    }

    private void ApplyAccessoryEffect(Accessory accessory)
    {
        // �Ǽ��縮 ���� ȿ���� PlayerStat�� �ݿ�
        // playerStat?.ApplyAccessoryEffect(accessory.BaseData);
    }

    private void RemoveAccessoryEffect(Accessory accessory)
    {
        // �Ǽ��縮 ���� ȿ���� PlayerStat���� ����
        // playerStat?.RemoveAccessoryEffect(accessory.BaseData);
    }

    private void ApplySkillEffect(Skill skill)
    {
        // ��ų ���� ȿ���� PlayerStat�� �ݿ�
        // playerStat?.ApplySkillEffect(skill);
    }

    private void RemoveSkillEffect(Skill skill)
    {
        // ��ų ���� ȿ���� PlayerStat���� ����
        // playerStat?.RemoveSkillEffect(skill);
    }
}