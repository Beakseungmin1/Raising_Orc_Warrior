using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonDontDestroy<DataManager>
{
    [Header("Weapons")]
    private Dictionary<Grade, Dictionary<int, WeaponDataSO>> weaponDataDict = new Dictionary<Grade, Dictionary<int, WeaponDataSO>>();

    [Header("Accessories")]
    private Dictionary<Grade, Dictionary<int, AccessoryDataSO>> accessoryDataDict = new Dictionary<Grade, Dictionary<int, AccessoryDataSO>>();

    [Header("Skills")]
    private Dictionary<Grade, List<SkillDataSO>> skillDataDict = new Dictionary<Grade, List<SkillDataSO>>();

    protected override void Awake()
    {
        base.Awake();
        LoadAllData();
    }

    private void LoadAllData()
    {
        // ���� ������ �ε�
        foreach (var weapon in Resources.LoadAll<WeaponDataSO>("Weapons"))
        {
            if (!weaponDataDict.ContainsKey(weapon.grade))
            {
                weaponDataDict[weapon.grade] = new Dictionary<int, WeaponDataSO>();
            }
            weaponDataDict[weapon.grade][weapon.rank] = weapon;
        }

        // �Ǽ��縮 ������ �ε�
        foreach (var accessory in Resources.LoadAll<AccessoryDataSO>("Accessories"))
        {
            if (!accessoryDataDict.ContainsKey(accessory.grade))
            {
                accessoryDataDict[accessory.grade] = new Dictionary<int, AccessoryDataSO>();
            }
            accessoryDataDict[accessory.grade][accessory.rank] = accessory;
        }

        foreach (var skill in Resources.LoadAll<SkillDataSO>("Skills"))
        {
            if (!skillDataDict.ContainsKey(skill.grade))
            {
                skillDataDict[skill.grade] = new List<SkillDataSO>();
            }
            skillDataDict[skill.grade].Add(skill);
        }

    }

    // ���� ������ �˻�
    public WeaponDataSO GetWeaponByGradeAndRank(Grade grade, int rank)
    {
        if (grade == Grade.Ultimate)
        {
            rank = 4;
        }

        if (weaponDataDict.ContainsKey(grade) && weaponDataDict[grade].ContainsKey(rank))
        {
            return weaponDataDict[grade][rank];
        }

        return null;
    }

    // �Ǽ��縮 ������ �˻�
    public AccessoryDataSO GetAccessoryByGradeAndRank(Grade grade, int rank)
    {
        if (grade == Grade.Ultimate)
        {
            rank = 4;
        }

        if (accessoryDataDict.ContainsKey(grade) && accessoryDataDict[grade].ContainsKey(rank))
        {
            return accessoryDataDict[grade][rank];
        }

        return null;
    }

    // ��ų ������ ������ �˻�
    public SkillDataSO GetRandomSkillByGrade(Grade grade)
    {
        if (skillDataDict.ContainsKey(grade) && skillDataDict[grade].Count > 0)
        {
            int randomIndex = Random.Range(0, skillDataDict[grade].Count);
            return skillDataDict[grade][randomIndex];
        }

        return null;
    }

    // ���� ���� �ܰ� ������ ��ȯ
    public WeaponDataSO GetNextWeapon(Grade grade, int rank)
    {
        // Rank�� 1�̸� ���� ������� �̵�
        if (rank == 1)
        {
            Grade nextGrade = grade + 1;

            if (nextGrade > Grade.Ultimate)
            {
                return null;
            }

            return GetWeaponByGradeAndRank(nextGrade, 4); // ���� ����� �ְ� ��ũ ����
        }

        return GetWeaponByGradeAndRank(grade, rank - 1); // ���� ����� ���� ��ũ
    }

    // �Ǽ��縮 ���� �ܰ� ������ ��ȯ
    public AccessoryDataSO GetNextAccessory(Grade grade, int rank)
    {
        if (rank == 1)
        {
            Grade nextGrade = grade + 1;

            if (nextGrade > Grade.Ultimate)
            {
                return null;
            }

            return GetAccessoryByGradeAndRank(nextGrade, 4); // ���� ����� �ְ� ��ũ �Ǽ��縮
        }

        return GetAccessoryByGradeAndRank(grade, rank - 1); // ���� ����� ���� ��ũ
    }

    // ��ų ���� �ܰ� ������ ��ȯ
    public SkillDataSO GetNextSkill(Grade grade)
    {
        Grade nextGrade = grade + 1;

        if (!skillDataDict.ContainsKey(nextGrade) || skillDataDict[nextGrade].Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, skillDataDict[nextGrade].Count);
        return skillDataDict[nextGrade][randomIndex];
    }

    public List<WeaponDataSO> GetAllWeapons()
    {
        List<WeaponDataSO> allWeapons = new List<WeaponDataSO>();
        foreach (var gradeDict in weaponDataDict.Values)
        {
            allWeapons.AddRange(gradeDict.Values);
        }
        return allWeapons;
    }

    public List<AccessoryDataSO> GetAllAccessories()
    {
        List<AccessoryDataSO> allAccessories = new List<AccessoryDataSO>();
        foreach (var gradeDict in accessoryDataDict.Values)
        {
            allAccessories.AddRange(gradeDict.Values);
        }
        return allAccessories;
    }

    public List<SkillDataSO> GetAllSkills()
    {
        List<SkillDataSO> allSkills = new List<SkillDataSO>();

        foreach (var skillList in skillDataDict.Values)
        {
            allSkills.AddRange(skillList);
        }

        return allSkills;
    }
}