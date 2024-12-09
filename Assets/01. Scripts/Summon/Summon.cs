using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.Progress;

public class Summon : MonoBehaviour
{
    // 등급별 소환 확률 딕셔너리
    private Dictionary<Grade, float> gradeSummonRates;

    //4,3,2,1 랭크별 소환 확률 딕셔너리
    private Dictionary<int, float> rankSummonRates;

    private void Awake()
    {
        // 등급 딕셔너리 초기화
        gradeSummonRates = new Dictionary<Grade, float>
        {
            { Grade.Normal, SummonDataManager.Instance.normalGradeSummonRate },
            { Grade.Uncommon, SummonDataManager.Instance.uncommonGradeSummonRate },
            { Grade.Rare, SummonDataManager.Instance.rareGradeSummonRate },
            { Grade.Hero, SummonDataManager.Instance.heroGradeSummonRate },
            { Grade.Legendary, SummonDataManager.Instance.legendaryGradeSummonRate },
            { Grade.Mythic, SummonDataManager.Instance.mythicGradeSummonRate },
            { Grade.Ultimate, SummonDataManager.Instance.ultimateGradeSummonRate }
        };

        // 랭크 딕셔너리 초기화
        rankSummonRates = new Dictionary<int, float>
        {
            { 4, SummonDataManager.Instance.rank4SummonRate },
            { 3, SummonDataManager.Instance.rank3SummonRate },
            { 2, SummonDataManager.Instance.rank2SummonRate },
            { 1, SummonDataManager.Instance.rank1SummonRate }
        };
    }

    private Grade GetGradeBySummonRate()
    {
        float totalRate = 0f;

        // 전체 확률 계산
        foreach (var rate in gradeSummonRates.Values)
        {
            totalRate += rate;
        }

        // 0부터 전체 확률 사이의 랜덤 값 생성
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // 랜덤 값에 따라 등급 선택
        foreach (var grade in gradeSummonRates)
        {
            accumulatedRate += grade.Value;
            if (randomValue <= accumulatedRate)
            {
                return grade.Key;
            }
        }

        // 기본값 반환 (문제 발생 시)
        return Grade.Normal;
    }

    private int GetRankBySummonRate()
    {
        float totalRate = 0f;

        // 전체 확률 계산
        foreach (var rate in rankSummonRates.Values)
        {
            totalRate += rate;
        }

        // 0부터 전체 확률 사이의 랜덤 값 생성
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // 랜덤 값에 따라 등급 선택
        foreach (var rank in rankSummonRates)
        {
            accumulatedRate += rank.Value;
            if (randomValue <= accumulatedRate)
            {
                return rank.Key;
            }
        }

        // 기본값 반환 (문제 발생 시)
        return 4;
    }

    public List<WeaponDataSO> SummonWeaponDataSOList(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = new List<WeaponDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            Grade grade = GetGradeBySummonRate();
            int rank = GetRankBySummonRate();
            WeaponDataSO weaponDataSO = DataManager.Instance.GetWeaponByGradeAndRank(grade, rank);
            PlayerobjManager.Instance.Player.inventory.AddItemToInventory(weaponDataSO);
            weaponDataSOs.Add(weaponDataSO);
        }
        return weaponDataSOs;
    }

    public void SummonSkillCard(int summonCount)
    {
        for (int i = 0; i < summonCount; i++)
        {
            Grade grade = GetGradeBySummonRate();
            SkillDataSO skillDataSO = DataManager.Instance.GetRandomSkillByGrade(grade);
            PlayerobjManager.Instance.Player.inventory.AddItemToInventory(skillDataSO);
            Debug.Log(skillDataSO);
        }
    }

    public void SummonAccessary(int summonCount)
    {
        for (int i = 0; i < summonCount; i++)
        {
            Grade grade = GetGradeBySummonRate();
            int rank = GetRankBySummonRate();
            AccessoryDataSO accessoryDataSO = DataManager.Instance.GetAccessoryByGradeAndRank(grade, rank);
            PlayerobjManager.Instance.Player.inventory.AddItemToInventory(accessoryDataSO);
            Debug.Log(accessoryDataSO);
        }
    }
}
