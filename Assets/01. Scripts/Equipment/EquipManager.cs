using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private PlayerStat playerStat; // �÷��̾� ����

    public WeaponDataSO EquippedWeapon { get; private set; }
    public AccessoryDataSO EquippedAccessory { get; private set; }
    public Skill EquippedSkill { get; private set; } // SkillDataSO -> Skill�� ����

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        if (playerStat == null)
        {
            Debug.LogError("EquipManager: PlayerStats�� ã�� �� �����ϴ�.");
        }
    }

    // ���� ����
    public void EquipWeapon(WeaponDataSO weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("�����Ϸ��� ���Ⱑ null�Դϴ�.");
            return;
        }

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
        if (EquippedWeapon == null)
        {
            Debug.Log("������ ���Ⱑ �����ϴ�.");
            return;
        }

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;
        Debug.Log("���� ���� �Ϸ�.");
    }

    // �Ǽ��縮 ����
    public void EquipAccessory(AccessoryDataSO accessory)
    {
        if (accessory == null)
        {
            Debug.LogWarning("�����Ϸ��� �Ǽ��縮�� null�Դϴ�.");
            return;
        }

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
        if (EquippedAccessory == null)
        {
            Debug.Log("������ �Ǽ��縮�� �����ϴ�.");
            return;
        }

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;
        Debug.Log("�Ǽ��縮 ���� �Ϸ�.");
    }

    // ��ų ����
    public void EquipSkill(Skill skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("�����Ϸ��� ��ų�� null�Դϴ�.");
            return;
        }

        if (EquippedSkill != null)
        {
            RemoveSkillEffect(EquippedSkill);
        }

        EquippedSkill = skill;
        ApplySkillEffect(skill);
        Debug.Log($"��ų {skill.BaseData.itemName} ���� �Ϸ�.");
    }

    public void UnequipSkill()
    {
        if (EquippedSkill == null)
        {
            Debug.Log("������ ��ų�� �����ϴ�.");
            return;
        }

        RemoveSkillEffect(EquippedSkill);
        EquippedSkill = null;
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