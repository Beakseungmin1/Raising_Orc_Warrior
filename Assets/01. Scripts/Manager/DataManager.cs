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

        // 스킬 데이터 로드
        foreach (var skill in Resources.LoadAll<SkillDataSO>("Skills"))
        {
            if (!skillDataDict.ContainsKey(skill.grade))
            {
                skillDataDict[skill.grade] = new List<SkillDataSO>();
            }
            skillDataDict[skill.grade].Add(skill);
            Debug.Log($"스킬 로드 완료: {skill.name}, Grade: {skill.grade}");
        }

        // 존재하는 등급만 검증
        foreach (Grade grade in skillDataDict.Keys)
        {
            Debug.Log($"스킬 데이터 확인: Grade {grade}, Count: {skillDataDict[grade].Count}");
        }

        Debug.Log("스킬 데이터 로드 완료");
    }

    // 무기 데이터 검색
    public WeaponDataSO GetWeaponByGradeAndRank(Grade grade, int rank)
    {
        if (grade == Grade.Ultimate)
        {
            rank = 1;
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
            rank = 1;
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
            // 요청된 등급에 데이터가 있는 경우 무작위 반환
            int randomIndex = Random.Range(0, skillDataDict[grade].Count);
            return skillDataDict[grade][randomIndex];
        }

        // 요청된 등급에 데이터가 없는 경우 다른 등급에서 스킬 반환
        Debug.LogWarning($"스킬 데이터 없음: Grade {grade}, 다른 등급에서 스킬 검색 시도");

        foreach (var keyValue in skillDataDict)
        {
            if (keyValue.Value.Count > 0)
            {
                int randomIndex = Random.Range(0, keyValue.Value.Count);
                return keyValue.Value[randomIndex];
            }
        }

        // 모든 등급에 데이터가 없는 경우 기본 스킬 반환
        Debug.LogError("모든 등급에 스킬 데이터가 없습니다. 기본 스킬 반환");
        return Resources.Load<SkillDataSO>("DefaultSkill");
    }

    // 스킬 다음 단계 데이터 반환
    public SkillDataSO GetNextSkill(Grade grade)
    {
        Grade nextGrade = grade + 1;

        if (!skillDataDict.ContainsKey(nextGrade) || skillDataDict[nextGrade].Count == 0)
        {
            Debug.Log($"다음 스킬 데이터 없음: Grade {nextGrade}");
            return null;
        }

        int randomIndex = Random.Range(0, skillDataDict[nextGrade].Count);
        return skillDataDict[nextGrade][randomIndex];
    }

    // 무기 다음 단계 데이터 반환
    public WeaponDataSO GetNextWeapon(Grade grade, int rank)
    {
        // Rank가 1이면 다음 등급으로 이동
        if (rank == 1)
        {
            Grade nextGrade = grade + 1;

            // Ultimate 등급은 다음 등급이 없음
            if (nextGrade > Grade.Ultimate)
            {
                Debug.LogWarning("Ultimate 등급은 다음 등급이 없습니다.");
                return null;
            }

            return GetWeaponByGradeAndRank(nextGrade, 4);
        }

        return GetWeaponByGradeAndRank(grade, rank - 1);
    }


    // 악세사리 다음 단계 데이터 반환
    public AccessoryDataSO GetNextAccessory(Grade grade, int rank)
    {
        // Rank가 1이면 다음 등급으로 이동
        if (rank == 1)
        {
            Grade nextGrade = grade + 1;

            // Ultimate 등급은 다음 등급이 없음
            if (nextGrade > Grade.Ultimate)
            {
                Debug.LogWarning("Ultimate 등급은 다음 등급이 없습니다.");
                return null;
            }

            return GetAccessoryByGradeAndRank(nextGrade, 4);
        }

        return GetAccessoryByGradeAndRank(grade, rank - 1);
    }
}