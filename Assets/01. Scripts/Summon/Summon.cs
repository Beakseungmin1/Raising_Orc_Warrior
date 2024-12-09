using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.Progress;

public class Summon : MonoBehaviour
{
    // ��޺� ��ȯ Ȯ�� ��ųʸ�
    private Dictionary<Grade, float> gradeSummonRates;

    //4,3,2,1 ��ũ�� ��ȯ Ȯ�� ��ųʸ�
    private Dictionary<int, float> rankSummonRates;

    private void Awake()
    {
        // ��� ��ųʸ� �ʱ�ȭ
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

        // ��ũ ��ųʸ� �ʱ�ȭ
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

        // ��ü Ȯ�� ���
        foreach (var rate in gradeSummonRates.Values)
        {
            totalRate += rate;
        }

        // 0���� ��ü Ȯ�� ������ ���� �� ����
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // ���� ���� ���� ��� ����
        foreach (var grade in gradeSummonRates)
        {
            accumulatedRate += grade.Value;
            if (randomValue <= accumulatedRate)
            {
                return grade.Key;
            }
        }

        // �⺻�� ��ȯ (���� �߻� ��)
        return Grade.Normal;
    }

    private int GetRankBySummonRate()
    {
        float totalRate = 0f;

        // ��ü Ȯ�� ���
        foreach (var rate in rankSummonRates.Values)
        {
            totalRate += rate;
        }

        // 0���� ��ü Ȯ�� ������ ���� �� ����
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // ���� ���� ���� ��� ����
        foreach (var rank in rankSummonRates)
        {
            accumulatedRate += rank.Value;
            if (randomValue <= accumulatedRate)
            {
                return rank.Key;
            }
        }

        // �⺻�� ��ȯ (���� �߻� ��)
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
