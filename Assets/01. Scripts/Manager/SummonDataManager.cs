using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SummonDataManager : Singleton<SummonDataManager>
{
    //��ȯ ������ ���� ���, ��ũ�� ��ȯ Ȯ��, ��ȯ ���� ����ġ�� �����ϴ� ��ũ��Ʈ.
    private Dictionary<ItemType, SummonLevelProgress> summonLevelProgressDictionary;

    public Action OnExpChanged;
    public Action OnLevelChanged;

    private Dictionary<Grade, float> summonRates;

    //4,3,2,1��ũ�� ���� ��ȯ Ȯ�� (����)
    public float rank4SummonRate = 65f;
    public float rank3SummonRate = 20f;
    public float rank2SummonRate = 10f;
    public float rank1SummonRate = 5f;

    private void Awake()
    {
        summonLevelProgressDictionary = new Dictionary<ItemType, SummonLevelProgress>
        {
            { ItemType.Weapon, new SummonLevelProgress(1, 0f, 100f) },
            { ItemType.Skill, new SummonLevelProgress(1, 0f, 100f) },
            { ItemType.Accessory, new SummonLevelProgress(1, 0f, 100f) }
        };
    }

    public SummonLevelProgress GetProgress(ItemType type)
    {
        return summonLevelProgressDictionary.TryGetValue(type, out var progress) ? progress : null;
    }

    public void AddExperience(ItemType type, float experience) //��ȯȽ�� �ѹ��� ����ġ +1
    {
        if (!summonLevelProgressDictionary.ContainsKey(type)) return;

        var progress = summonLevelProgressDictionary[type];
        progress.Exp += experience;
        OnExpChanged?.Invoke();

        while (progress.Exp >= progress.ExpToNextLevel)
        {
            progress.Exp -= progress.ExpToNextLevel;
            progress.Level++;
            progress.ExpToNextLevel *= 1.2f; // ���� �� ����ġ ���� (����)
            OnLevelChanged?.Invoke();
        }
    }

    public int GetLevel(ItemType type)
    {
        return summonLevelProgressDictionary.TryGetValue(type, out var progress) ? progress.Level : 0;
    }

    public float GetExp(ItemType type)
    {
        return summonLevelProgressDictionary.TryGetValue(type, out var progress) ? progress.Exp : 0;
    }

    public float GetExpToNextLevel(ItemType type)
    {
        return summonLevelProgressDictionary.TryGetValue(type, out var progress) ? progress.ExpToNextLevel : 0;
    }

    public Dictionary<Grade, float> GetAdjustedSummonRates(int summonLevel)
    {
        // �⺻ ��ȯ Ȯ�� (�ʱ� ��)
        float normalBaseRate = 60.5f;
        float uncommonBaseRate = 20f;
        float rareBaseRate = 10f;
        float heroBaseRate = 5f;
        float legendaryBaseRate = 3f;
        float mythicBaseRate = 1f;
        float ultimateBaseRate = 0.5f;

        // ���� ��� ������
        float levelMultiplier = Mathf.Clamp01(summonLevel / 50f); // 0~1 ���� ��
        float totalRate = 100f;

        // ���� ��� Ȯ�� ���
        float ultimateRate = ultimateBaseRate + (levelMultiplier * 4f); // �ִ� 4% ����
        float mythicRate = Mathf.Lerp(mythicBaseRate, 10f, levelMultiplier); // ��ǥ�� 10%�� ����
        float legendaryRate = legendaryBaseRate + (levelMultiplier * 7f); // �ִ� 7% ����

        // ���� ��� ������ ���
        float adjustment = ultimateRate + mythicRate + legendaryRate
            - (ultimateBaseRate + mythicBaseRate + legendaryBaseRate);

        // ���� ��� Ȯ�� ����
        float normalRate = Mathf.Lerp(normalBaseRate, 20f, levelMultiplier); // ��ǥ�� 20%�� ����
        float uncommonRate = uncommonBaseRate - (adjustment * 0.3f); // 30% ���� ����
        float rareRate = rareBaseRate - (adjustment * 0.2f); // 20% ���� ����

        // ��ųʸ��� ��ȯ
        return new Dictionary<Grade, float>
        {
            { Grade.Normal, Mathf.Max(normalRate, 0f) },
            { Grade.Uncommon, Mathf.Max(uncommonRate, 0f) },
            { Grade.Rare, Mathf.Max(rareRate, 0f) },
            { Grade.Hero, heroBaseRate },
            { Grade.Legendary, legendaryRate },
            { Grade.Mythic, mythicRate },
            { Grade.Ultimate, ultimateRate }
        };

        /*
        50���� ������
        ��� Ȯ��
        Normal  20.0 %
        Uncommon    �� 13.0 %
        Rare    �� 6.5 %
        Hero    5.0 %
        Legendary   �� 10.0 %
        Mythic  10.0 %
        Ultimate    �� 4.5 %
        */
    }
}
