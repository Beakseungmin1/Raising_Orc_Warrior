using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonDontDestroy<DataManager>
{
    [Header("Weapons")]
    private List<WeaponDataSO> weaponDataList = new List<WeaponDataSO>();

    [Header("Accessories")]
    private List<AccessoryDataSO> accessoryDataList = new List<AccessoryDataSO>();

    [Header("Skills")]
    private List<SkillDataSO> skillDataList = new List<SkillDataSO>();

    protected override void Awake()
    {
        base.Awake();
        LoadAllData();
    }

    private void LoadAllData()
    {
        weaponDataList.AddRange(Resources.LoadAll<WeaponDataSO>("Weapons"));
        accessoryDataList.AddRange(Resources.LoadAll<AccessoryDataSO>("Accessories"));
        skillDataList.AddRange(Resources.LoadAll<SkillDataSO>("Skills"));

        Debug.Log($"���� {weaponDataList.Count}��, �Ǽ��縮 {accessoryDataList.Count}��, ��ų {skillDataList.Count}�� �ε� �Ϸ�");
    }

    public WeaponDataSO GetWeaponData(string itemName)
    {
        return weaponDataList.Find(weapon => weapon.itemName == itemName);
    }

    public AccessoryDataSO GetAccessoryData(string itemName)
    {
        return accessoryDataList.Find(accessory => accessory.itemName == itemName);
    }

    public SkillDataSO GetSkillData(string itemName)
    {
        return skillDataList.Find(skill => skill.itemName == itemName);
    }

    public List<WeaponDataSO> GetAllWeaponData()
    {
        return new List<WeaponDataSO>(weaponDataList); // ����Ʈ ���纻 ��ȯ
    }

    public List<AccessoryDataSO> GetAllAccessoryData()
    {
        return new List<AccessoryDataSO>(accessoryDataList);
    }

    public List<SkillDataSO> GetAllSkillData()
    {
        return new List<SkillDataSO>(skillDataList);
    }
}