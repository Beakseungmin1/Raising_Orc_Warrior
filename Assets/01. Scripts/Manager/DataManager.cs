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
        // 무기 데이터 로드
        foreach (var weapon in Resources.LoadAll<WeaponDataSO>("Weapons"))
        {
            if (!weaponDataDict.ContainsKey(weapon.grade))
            {
                weaponDataDict[weapon.grade] = new Dictionary<int, WeaponDataSO>();
            }
            weaponDataDict[weapon.grade][weapon.rank] = weapon;
        }

        // 악세사리 데이터 로드
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

    // 무기 데이터 검색
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

    // 악세사리 데이터 검색
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

    // 스킬 데이터 무작위 검색
    public SkillDataSO GetRandomSkillByGrade(Grade grade)
    {
        if (skillDataDict.ContainsKey(grade) && skillDataDict[grade].Count > 0)
        {
            int randomIndex = Random.Range(0, skillDataDict[grade].Count);
            return skillDataDict[grade][randomIndex];
        }

        return null;
    }

    // 무기 다음 단계 데이터 반환
    public WeaponDataSO GetNextWeapon(Grade grade, int rank)
    {
        // Rank가 1이면 다음 등급으로 이동
        if (rank == 1)
        {
            Grade nextGrade = grade + 1;

            if (nextGrade > Grade.Ultimate)
            {
                return null;
            }

            return GetWeaponByGradeAndRank(nextGrade, 4); // 다음 등급의 최고 랭크 무기
        }

        return GetWeaponByGradeAndRank(grade, rank - 1); // 동일 등급의 다음 랭크
    }

    // 악세사리 다음 단계 데이터 반환
    public AccessoryDataSO GetNextAccessory(Grade grade, int rank)
    {
        if (rank == 1)
        {
            Grade nextGrade = grade + 1;

            if (nextGrade > Grade.Ultimate)
            {
                return null;
            }

            return GetAccessoryByGradeAndRank(nextGrade, 4); // 다음 등급의 최고 랭크 악세사리
        }

        return GetAccessoryByGradeAndRank(grade, rank - 1); // 동일 등급의 다음 랭크
    }

    // 스킬 다음 단계 데이터 반환
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