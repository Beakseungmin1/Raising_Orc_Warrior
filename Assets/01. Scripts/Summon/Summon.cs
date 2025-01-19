using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    /// <summary>
    /// ��ȯ Ȯ���� ���� ��ް� ��ũ ��ȯ
    /// </summary>
    private (Grade grade, int rank) GetGradeAndRankBySummonRate(ItemType itemType, bool isWeapon, bool isSkill = false)
    {
        float totalRate = 0f;

        // ��ü Ȯ�� ���
        var adjustedRates = SummonDataManager.Instance.GetAdjustedSummonRates(
            SummonDataManager.Instance.GetLevel(itemType),
            isWeapon,
            isSkill
        );

        foreach (var rate in adjustedRates.Values)
        {
            totalRate += rate;
        }

        // 0���� ��ü Ȯ�� ������ ���� �� ����
        float randomValue = Random.Range(0f, totalRate);
        float accumulatedRate = 0f;

        // ���� ���� ���� ��� �� ��ũ ����
        foreach (var itemKey in adjustedRates.Keys)
        {
            accumulatedRate += adjustedRates[itemKey];
            if (randomValue <= accumulatedRate)
            {
                // Skill�� ��� Rank�� �ʿ� �����Ƿ� �⺻�� ��ȯ
                if (isSkill)
                {
                    if (System.Enum.TryParse(itemKey, out Grade grade))
                    {
                        return (grade, 0); // Rank�� 0���� ����
                    }
                }

                // Weapon/Accessory�� ���� ��Ĵ�� ó��
                string gradeString = itemKey.Substring(0, itemKey.Length - 1); // ��� (��: "Rare")
                int rank = int.Parse(itemKey[^1].ToString()); // ��ũ (��: "3")

                if (System.Enum.TryParse(gradeString, out Grade gradeResult))
                {
                    return (gradeResult, rank);
                }
            }
        }

        // �⺻�� ��ȯ (���� �߻� ��)
        return (Grade.Normal, 4); // �⺻ ��ް� ��ũ
    }


    /// <summary>
    /// ���� ��ȯ
    /// </summary>
    public List<WeaponDataSO> SummonWeaponDataSOList(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = new List<WeaponDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            var (grade, rank) = GetGradeAndRankBySummonRate(ItemType.Weapon, isWeapon: true);
            WeaponDataSO weaponDataSO = DataManager.Instance.GetWeaponByGradeAndRank(grade, rank);

            // ���⸦ ��ȯ�ϰ� �κ��丮�� �߰�
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(weaponDataSO);
            weaponDataSOs.Add(weaponDataSO);
        }

        // ��ȯ �̺�Ʈ ȣ��
        GameEventsManager.Instance.summonEvents.WeaponSummoned(summonCount);
        return weaponDataSOs;
    }

    /// <summary>
    /// ��ų ��ȯ (��޸� ���)
    /// </summary>
    public List<SkillDataSO> SummonSkillDataSOList(int summonCount)
    {
        List<SkillDataSO> skillDataSOs = new List<SkillDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            // Skill�� Rank�� �����Ƿ� isSkill = true�� ȣ��
            var (grade, _) = GetGradeAndRankBySummonRate(ItemType.Skill, isWeapon: false, isSkill: true);
            SkillDataSO skillDataSO = DataManager.Instance.GetRandomSkillByGrade(grade);

            // ��ų�� ��ȯ�ϰ� �κ��丮�� �߰�
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(skillDataSO);
            skillDataSOs.Add(skillDataSO);
        }

        // ��ȯ �̺�Ʈ ȣ��
        GameEventsManager.Instance.summonEvents.SkillSummoned(summonCount);
        return skillDataSOs;
    }


    /// <summary>
    /// �Ǽ����� ��ȯ
    /// </summary>
    public List<AccessoryDataSO> SummonAccessoryDataSOList(int summonCount)
    {
        List<AccessoryDataSO> accessoryDataSOs = new List<AccessoryDataSO>();

        for (int i = 0; i < summonCount; i++)
        {
            var (grade, rank) = GetGradeAndRankBySummonRate(ItemType.Accessory, isWeapon: false);
            AccessoryDataSO accessoryDataSO = DataManager.Instance.GetAccessoryByGradeAndRank(grade, rank);

            // �Ǽ������� ��ȯ�ϰ� �κ��丮�� �߰�
            PlayerObjManager.Instance.Player.inventory.AddItemToInventory(accessoryDataSO);
            accessoryDataSOs.Add(accessoryDataSO);
        }

        // ��ȯ �̺�Ʈ ȣ��
        GameEventsManager.Instance.summonEvents.AccessorySummoned(summonCount);
        return accessoryDataSOs;
    }
}
