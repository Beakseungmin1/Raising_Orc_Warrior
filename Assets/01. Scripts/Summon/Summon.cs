using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.Progress;

public class Summon : MonoBehaviour
{
    //4,3,2,1 랭크별 소환 확률 딕셔너리
    private Dictionary<int, float> rankSummonRates;

    private void Awake()
    {
        // 랭크 딕셔너리 초기화
        rankSummonRates = new Dictionary<int, float>
        {
            { 4, SummonDataManager.Instance.rank4SummonRate },
            { 3, SummonDataManager.Instance.rank3SummonRate },
            { 2, SummonDataManager.Instance.rank2SummonRate },
            { 1, SummonDataManager.Instance.rank1SummonRate }
        };
    }

    private Grade GetGradeBySummonRate(ItemType itemType)
    {
        float totalRate = 0f;

        // 전체 확률 계산
        foreach (var rate in SummonDataManager.Instance.GetAdjustedSummonRates(SummonDataManager.Instance.GetLevel(itemType)).Values)
        {
            totalRate += rate;
        }

        // 0부터 전체 확률 사이의 랜덤 값 생성
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // 랜덤 값에 따라 등급 선택
        foreach (var grade in SummonDataManager.Instance.GetAdjustedSummonRates(SummonDataManager.Instance.GetLevel(itemType)))
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

    //확률 디버그로그 찍는 용도의 매서드
    private void ReturnRateByDebugLog(ItemType itemType)
    {
        var valuesList = SummonDataManager.Instance.GetAdjustedSummonRates(SummonDataManager.Instance.GetLevel(itemType)).Values.ToList();

        Debug.Log("------------------------------------");
        Debug.Log($"현재 타입: {itemType}");
        Debug.Log($"현재 소환 레벨:{SummonDataManager.Instance.GetLevel(itemType)}");
        Debug.Log($"현재 노말 소환 확률: {valuesList[0]}");
        Debug.Log($"현재 언커먼 소환 확률: {valuesList[1]}");
        Debug.Log($"현재 레어 소환 확률: {valuesList[2]}");
        Debug.Log($"현재 히어로 소환 확률: {valuesList[3]}");
        Debug.Log($"현재 레전더리 소환 확률: {valuesList[4]}");
        Debug.Log($"현재 미딕 소환 확률: {valuesList[5]}");
        Debug.Log($"현재 얼티밋 소환 확률: {valuesList[6]}");
        Debug.Log("------------------------------------");
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
            Grade grade = GetGradeBySummonRate(ItemType.Weapon);
            int rank = GetRankBySummonRate();
            WeaponDataSO weaponDataSO = DataManager.Instance.GetWeaponByGradeAndRank(grade, rank);
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(weaponDataSO);
            weaponDataSOs.Add(weaponDataSO);
        }
        ReturnRateByDebugLog(ItemType.Weapon); //확률 변환 테스트용
        GameEventsManager.Instance.summonEvents.WeaponSummoned(summonCount);
        return weaponDataSOs;
    }

    public List<SkillDataSO> SummonSkillDataSOList(int summonCount)
    {
        List<SkillDataSO> skillDataSOs = new List<SkillDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            Grade grade = GetGradeBySummonRate(ItemType.Skill);
            SkillDataSO skillDataSO = DataManager.Instance.GetRandomSkillByGrade(grade);
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(skillDataSO);
            skillDataSOs.Add(skillDataSO);
        }
        ReturnRateByDebugLog(ItemType.Skill); //확률 변환 테스트용
        GameEventsManager.Instance.summonEvents.SkillSummoned(summonCount);
        return skillDataSOs;
    }

    public List<AccessoryDataSO> SummonAccessoryDataSOList(int summonCount)
    {
        List<AccessoryDataSO> accessoryDataSOs = new List<AccessoryDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            Grade grade = GetGradeBySummonRate(ItemType.Accessory);
            int rank = GetRankBySummonRate();
            AccessoryDataSO accessoryDataSO = DataManager.Instance.GetAccessoryByGradeAndRank(grade, rank);
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(accessoryDataSO);
            accessoryDataSOs.Add(accessoryDataSO);
        }
        ReturnRateByDebugLog(ItemType.Accessory); //확률 변환 테스트용
        GameEventsManager.Instance.summonEvents.AccessorySummoned(summonCount);
        return accessoryDataSOs;
    }
}
