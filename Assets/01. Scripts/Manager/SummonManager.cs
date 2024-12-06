using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class SummonManager : Singleton<SummonManager>
{
    //일반,희귀,영웅 등등 등급에 따른 소환 확률
    private float normalGradeSummonRate = 60.5f;
    private float uncommonGradeSummonRate = 20f;
    private float rareGradeSummonRate = 10f;
    private float heroGradeSummonRate = 5f;
    private float legendaryGradeSummonRate = 3f;
    private float mythicGradeSummonRate = 1f;
    private float ultimateGradeSummonRate = 0.5f;

    private float[] summonGradeRates;

    //4,3,2,1랭크에 따른 소환 확률
    private float rank4SummonRate = 65f;
    private float rank3SummonRate = 20f;
    private float rank2SummonRate = 10f;
    private float rank1SummonRate = 5f;

    private float[] summonRankRates;

    private void Awake()
    {
        // 확률 배열 초기화
        summonGradeRates = new float[]
        {
            normalGradeSummonRate,
            uncommonGradeSummonRate,
            rareGradeSummonRate,
            heroGradeSummonRate,
            legendaryGradeSummonRate,
            mythicGradeSummonRate,
            ultimateGradeSummonRate
        };

        summonRankRates = new float[]
        {
            rank4SummonRate,
            rank3SummonRate,
            rank2SummonRate,
            rank1SummonRate
        };
    }

    private Grade GetGradeBySummonRate()
    {
        float totalRate = 0f;

        // 전체 확률 계산
        foreach (float rate in summonGradeRates)
        {
            totalRate += rate;
        }

        // 0부터 전체 확률 사이의 랜덤 값 생성
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // 랜덤 값에 따라 등급 선택
        for (int i = 0; i < summonGradeRates.Length; i++)
        {
            accumulatedRate += summonGradeRates[i];
            if (randomValue <= accumulatedRate)
            {
                return (Grade)i;
            }
        }

        // 기본값 반환 (문제 발생 시)
        return Grade.Normal;
    }

    private int GetRankBySummonRate()
    {
        float totalRate = 0f;

        // 전체 확률 계산
        foreach (float rate in summonRankRates)
        {
            totalRate += rate;
        }

        // 0부터 전체 확률 사이의 랜덤 값 생성
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // 랜덤 값에 따라 등급 선택
        for (int i = 0; i < summonRankRates.Length; i++)
        {
            accumulatedRate += summonRankRates[i];
            if (randomValue <= accumulatedRate)
            {
                return 4 - i; // Rank 4부터 Rank 1까지 매핑
            }
        }

        // 기본값 반환 (문제 발생 시)
        return 4;
    }


    public void SummonWeapon(int summonCount)
    {
        for (int i = 0; i < summonCount; i++)
        {
            Grade grade = GetGradeBySummonRate();
            int rank = GetRankBySummonRate();
            WeaponDataSO weaponDataSO = DataManager.Instance.GetWeaponByGradeAndRank(grade, rank);
            PlayerobjManager.Instance.Player.inventory.AddItemToInventory(weaponDataSO);
            Debug.Log(weaponDataSO);
        }
    }

    public void SummonSkillCard(int summonCount)
    {

    }

    public void SummonAccessary(int summonCount)
    {

    }
}
