using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.Progress;

public class Summon : MonoBehaviour
{
    //4,3,2,1 ��ũ�� ��ȯ Ȯ�� ��ųʸ�
    private Dictionary<int, float> rankSummonRates;

    private void Awake()
    {
        // ��ũ ��ųʸ� �ʱ�ȭ
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

        // ��ü Ȯ�� ���
        foreach (var rate in SummonDataManager.Instance.GetAdjustedSummonRates(SummonDataManager.Instance.GetLevel(itemType)).Values)
        {
            totalRate += rate;
        }

        // 0���� ��ü Ȯ�� ������ ���� �� ����
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // ���� ���� ���� ��� ����
        foreach (var grade in SummonDataManager.Instance.GetAdjustedSummonRates(SummonDataManager.Instance.GetLevel(itemType)))
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

    //Ȯ�� ����׷α� ��� �뵵�� �ż���
    private void ReturnRateByDebugLog(ItemType itemType)
    {
        var valuesList = SummonDataManager.Instance.GetAdjustedSummonRates(SummonDataManager.Instance.GetLevel(itemType)).Values.ToList();

        Debug.Log("------------------------------------");
        Debug.Log($"���� Ÿ��: {itemType}");
        Debug.Log($"���� ��ȯ ����:{SummonDataManager.Instance.GetLevel(itemType)}");
        Debug.Log($"���� �븻 ��ȯ Ȯ��: {valuesList[0]}");
        Debug.Log($"���� ��Ŀ�� ��ȯ Ȯ��: {valuesList[1]}");
        Debug.Log($"���� ���� ��ȯ Ȯ��: {valuesList[2]}");
        Debug.Log($"���� ����� ��ȯ Ȯ��: {valuesList[3]}");
        Debug.Log($"���� �������� ��ȯ Ȯ��: {valuesList[4]}");
        Debug.Log($"���� �̵� ��ȯ Ȯ��: {valuesList[5]}");
        Debug.Log($"���� ��Ƽ�� ��ȯ Ȯ��: {valuesList[6]}");
        Debug.Log("------------------------------------");
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
            Grade grade = GetGradeBySummonRate(ItemType.Weapon);
            int rank = GetRankBySummonRate();
            WeaponDataSO weaponDataSO = DataManager.Instance.GetWeaponByGradeAndRank(grade, rank);
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(weaponDataSO);
            weaponDataSOs.Add(weaponDataSO);
        }
        ReturnRateByDebugLog(ItemType.Weapon); //Ȯ�� ��ȯ �׽�Ʈ��
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
        ReturnRateByDebugLog(ItemType.Skill); //Ȯ�� ��ȯ �׽�Ʈ��
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
        ReturnRateByDebugLog(ItemType.Accessory); //Ȯ�� ��ȯ �׽�Ʈ��
        GameEventsManager.Instance.summonEvents.AccessorySummoned(summonCount);
        return accessoryDataSOs;
    }
}
