using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class SummonManager : Singleton<SummonManager>
{
    //�Ϲ�,���,���� ��� ��޿� ���� ��ȯ Ȯ��
    private float normalGradeSummonRate = 60.5f;
    private float uncommonGradeSummonRate = 20f;
    private float rareGradeSummonRate = 10f;
    private float heroGradeSummonRate = 5f;
    private float legendaryGradeSummonRate = 3f;
    private float mythicGradeSummonRate = 1f;
    private float ultimateGradeSummonRate = 0.5f;

    private float[] summonGradeRates;

    //4,3,2,1��ũ�� ���� ��ȯ Ȯ��
    private float rank4SummonRate = 65f;
    private float rank3SummonRate = 20f;
    private float rank2SummonRate = 10f;
    private float rank1SummonRate = 5f;

    private float[] summonRankRates;

    private void Awake()
    {
        // Ȯ�� �迭 �ʱ�ȭ
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

        // ��ü Ȯ�� ���
        foreach (float rate in summonGradeRates)
        {
            totalRate += rate;
        }

        // 0���� ��ü Ȯ�� ������ ���� �� ����
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // ���� ���� ���� ��� ����
        for (int i = 0; i < summonGradeRates.Length; i++)
        {
            accumulatedRate += summonGradeRates[i];
            if (randomValue <= accumulatedRate)
            {
                return (Grade)i;
            }
        }

        // �⺻�� ��ȯ (���� �߻� ��)
        return Grade.Normal;
    }

    private int GetRankBySummonRate()
    {
        float totalRate = 0f;

        // ��ü Ȯ�� ���
        foreach (float rate in summonRankRates)
        {
            totalRate += rate;
        }

        // 0���� ��ü Ȯ�� ������ ���� �� ����
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // ���� ���� ���� ��� ����
        for (int i = 0; i < summonRankRates.Length; i++)
        {
            accumulatedRate += summonRankRates[i];
            if (randomValue <= accumulatedRate)
            {
                return 4 - i; // Rank 4���� Rank 1���� ����
            }
        }

        // �⺻�� ��ȯ (���� �߻� ��)
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
