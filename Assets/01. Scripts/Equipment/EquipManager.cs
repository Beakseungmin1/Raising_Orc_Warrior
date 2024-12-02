using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private PlayerStat playerStat; // 플레이어 스탯

    public WeaponDataSO EquippedWeapon { get; private set; }
    public AccessoryDataSO EquippedAccessory { get; private set; }
    public Skill EquippedSkill { get; private set; } // SkillDataSO -> Skill로 변경

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        if (playerStat == null)
        {
            Debug.LogError("EquipManager: PlayerStats를 찾을 수 없습니다.");
        }
    }

    // 무기 장착
    public void EquipWeapon(WeaponDataSO weapon)
    {
        if (weapon == null)
        {
            Debug.LogWarning("장착하려는 무기가 null입니다.");
            return;
        }

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
        if (EquippedWeapon == null)
        {
            Debug.Log("장착된 무기가 없습니다.");
            return;
        }

        RemoveWeaponEffect(EquippedWeapon);
        EquippedWeapon = null;
        Debug.Log("무기 해제 완료.");
    }

    // 악세사리 장착
    public void EquipAccessory(AccessoryDataSO accessory)
    {
        if (accessory == null)
        {
            Debug.LogWarning("장착하려는 악세사리가 null입니다.");
            return;
        }

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
        if (EquippedAccessory == null)
        {
            Debug.Log("장착된 악세사리가 없습니다.");
            return;
        }

        RemoveAccessoryEffect(EquippedAccessory);
        EquippedAccessory = null;
        Debug.Log("악세사리 해제 완료.");
    }

    // 스킬 장착
    public void EquipSkill(Skill skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("장착하려는 스킬이 null입니다.");
            return;
        }

        if (EquippedSkill != null)
        {
            RemoveSkillEffect(EquippedSkill);
        }

        EquippedSkill = skill;
        ApplySkillEffect(skill);
        Debug.Log($"스킬 {skill.BaseData.itemName} 장착 완료.");
    }

    public void UnequipSkill()
    {
        if (EquippedSkill == null)
        {
            Debug.Log("장착된 스킬이 없습니다.");
            return;
        }

        RemoveSkillEffect(EquippedSkill);
        EquippedSkill = null;
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