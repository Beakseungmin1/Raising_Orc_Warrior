using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    /// <summary>
    /// 소환 확률에 따라 등급과 랭크 반환
    /// </summary>
    private (Grade grade, int rank) GetGradeAndRankBySummonRate(ItemType itemType, bool isWeapon, bool isSkill = false)
    {
        float totalRate = 0f;

        // 전체 확률 계산
        var adjustedRates = SummonDataManager.Instance.GetAdjustedSummonRates(
            SummonDataManager.Instance.GetLevel(itemType),
            isWeapon,
            isSkill
        );

        foreach (var rate in adjustedRates.Values)
        {
            totalRate += rate;
        }

        // 0부터 전체 확률 사이의 랜덤 값 생성
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // 랜덤 값에 따라 등급 및 랭크 선택
        foreach (var itemKey in adjustedRates.Keys)
        {
            accumulatedRate += adjustedRates[itemKey];
            if (randomValue <= accumulatedRate)
            {
                // Skill인 경우 Rank는 필요 없으므로 기본값 반환
                if (isSkill)
                {
                    if (System.Enum.TryParse(itemKey, out Grade grade))
                    {
                        return (grade, 0); // Rank는 0으로 고정
                    }
                }

                // Weapon/Accessory는 기존 방식대로 처리
                string gradeString = itemKey.Substring(0, itemKey.Length - 1); // 등급 (예: "Rare")
                int rank = int.Parse(itemKey[^1].ToString()); // 랭크 (예: "3")

                if (System.Enum.TryParse(gradeString, out Grade gradeResult))
                {
                    return (gradeResult, rank);
                }
            }
        }

        // 기본값 반환 (문제 발생 시)
        return (Grade.Normal, 4); // 기본 등급과 랭크
    }


    /// <summary>
    /// 무기 소환
    /// </summary>
    public List<WeaponDataSO> SummonWeaponDataSOList(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = new List<WeaponDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            var (grade, rank) = GetGradeAndRankBySummonRate(ItemType.Weapon, isWeapon: true);
            WeaponDataSO weaponDataSO = DataManager.Instance.GetWeaponByGradeAndRank(grade, rank);

            // 무기를 소환하고 인벤토리에 추가
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(weaponDataSO);
            weaponDataSOs.Add(weaponDataSO);
        }

        // 소환 이벤트 호출
        GameEventsManager.Instance.summonEvents.WeaponSummoned(summonCount);
        return weaponDataSOs;
    }

    /// <summary>
    /// 스킬 소환 (등급만 고려)
    /// </summary>
    public List<SkillDataSO> SummonSkillDataSOList(int summonCount)
    {
        List<SkillDataSO> skillDataSOs = new List<SkillDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            // Skill은 Rank가 없으므로 isSkill = true로 호출
            var (grade, _) = GetGradeAndRankBySummonRate(ItemType.Skill, isWeapon: false, isSkill: true);
            SkillDataSO skillDataSO = DataManager.Instance.GetRandomSkillByGrade(grade);

            // 스킬을 소환하고 인벤토리에 추가
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(skillDataSO);
            skillDataSOs.Add(skillDataSO);
        }

        // 소환 이벤트 호출
        GameEventsManager.Instance.summonEvents.SkillSummoned(summonCount);
        return skillDataSOs;
    }


    /// <summary>
    /// 악세서리 소환
    /// </summary>
    public List<AccessoryDataSO> SummonAccessoryDataSOList(int summonCount)
    {
        List<AccessoryDataSO> accessoryDataSOs = new List<AccessoryDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            var (grade, rank) = GetGradeAndRankBySummonRate(ItemType.Accessory, isWeapon: false);
            AccessoryDataSO accessoryDataSO = DataManager.Instance.GetAccessoryByGradeAndRank(grade, rank);

            // 악세서리를 소환하고 인벤토리에 추가
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(accessoryDataSO);
            accessoryDataSOs.Add(accessoryDataSO);
        }

        // 소환 이벤트 호출
        GameEventsManager.Instance.summonEvents.AccessorySummoned(summonCount);
        return accessoryDataSOs;
    }
}
