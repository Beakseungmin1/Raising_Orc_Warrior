using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataManager : Singleton<SummonDataManager>
{
    private Dictionary<ItemType, SummonLevelProgress> summonLevelProgressDictionary;

    public Action OnExpChanged;
    public Action OnLevelChanged;

    private void Awake()
    {
        summonLevelProgressDictionary = new Dictionary<ItemType, SummonLevelProgress>
        {
            { ItemType.Weapon, new SummonLevelProgress(1, 0f, 10f) },
            { ItemType.Skill, new SummonLevelProgress(1, 0f, 10f) },
            { ItemType.Accessory, new SummonLevelProgress(1, 0f, 10f) }
        };
    }

    public SummonLevelProgress GetProgress(ItemType type)
    {
        return summonLevelProgressDictionary.TryGetValue(type, out var progress) ? progress : null;
    }

    public void AddExperience(ItemType type, float experience)
    {
        if (!summonLevelProgressDictionary.ContainsKey(type)) return;

        var progress = summonLevelProgressDictionary[type];

        // ��ȯ ������ �ִ밪(50)�� ��� ����ġ�� �߰����� ����
        if (progress.Level >= 50)
        {
            progress.Exp = 0f; // 50������ �� ����ġ�� �׻� 0���� ����
            OnExpChanged?.Invoke();
            return;
        }

        progress.Exp += experience;
        OnExpChanged?.Invoke();

        while (progress.Exp >= progress.ExpToNextLevel)
        {
            progress.Exp -= progress.ExpToNextLevel;
            progress.Level++;

            // ���� �� ����ġ ����
            progress.ExpToNextLevel += 5f;

            // ��ȯ ���� �ִ밪(50) üũ
            if (progress.Level >= 50)
            {
                progress.Level = 50; // �ִ� ���� ����
                progress.Exp = 0f;   // 50���� ���� �� ����ġ 0���� ����
                OnLevelChanged?.Invoke();
                return;
            }

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

    public void SetLevel(ItemType type, int value)
    {
        var progress = summonLevelProgressDictionary[type];
        progress.Level = value;
        OnExpChanged?.Invoke();
        OnLevelChanged?.Invoke();
    }

    public void SetExp(ItemType type, float value)
    {
        var progress = summonLevelProgressDictionary[type];
        progress.Exp = value;
        OnExpChanged?.Invoke();
        OnLevelChanged?.Invoke();
    }

    public void SetExpToNextLevel(ItemType type, float value)
    {
        var progress = summonLevelProgressDictionary[type];
        progress.ExpToNextLevel = value;
        OnExpChanged?.Invoke();
        OnLevelChanged?.Invoke();
    }

    /// <summary>
    /// ��ȯ ������ ��� �������� �ʱ� Ȯ���� ����
    /// </summary>
    public Dictionary<string, float> GetAdjustedSummonRates(int summonLevel, bool isWeapon, bool isSkill = false)
    {
        // �⺻ Ȯ�� (���� 1)
        Dictionary<string, float> baseRates = isSkill
            ? new Dictionary<string, float> // Skill ���� Ȯ�� ���̺�
            {
            { "Normal", 30f },
            { "Uncommon", 20f },
            { "Rare", 18f },
            { "Hero", 15f },
            { "Legendary", 12f },
            { "Mythic", 8f },
            { "Ultimate", 5f }
            }
            : new Dictionary<string, float> // Weapon/Accessory Ȯ�� ���̺�
            {
            { "Normal4", 15f }, { "Normal3", 10f }, { "Normal2", 8f }, { "Normal1", 7f },
            { "Uncommon4", 10f }, { "Uncommon3", 8f }, { "Uncommon2", 7f }, { "Uncommon1", 5f },
            { "Rare4", 8f }, { "Rare3", 5f }, { "Rare2", 4f }, { "Rare1", 3f },
            { "Hero4", 2f }, { "Hero3", 1.5f }, { "Hero2", 1f }, { "Hero1", 0.5f },
            { "Legendary4", 1f }, { "Legendary3", 0.8f }, { "Legendary2", 0.7f }, { "Legendary1", 0.5f },
            { "Mythic4", 0.76f }, { "Mythic3", 0.57f }, { "Mythic2", 0.38f }, { "Mythic1", 0.19f },
            { "Ultimate1", 0.001f }
            };

        // ��ȯ ���� 50�� ��ǥ Ȯ��
        Dictionary<string, float> maxRates = isSkill
            ? new Dictionary<string, float> // Skill ���� Ȯ�� ���̺� (���� 50 ��ǥ)
            {
            { "Normal", 5f },
            { "Uncommon", 10f },
            { "Rare", 30f },
            { "Hero", 25f },
            { "Legendary", 20f },
            { "Mythic", 9f },
            { "Ultimate", 10f }
            }
            : new Dictionary<string, float> // Weapon/Accessory Ȯ�� ���̺�
            {
            { "Normal4", 10f }, { "Normal3", 7f }, { "Normal2", 5f }, { "Normal1", 3f },
            { "Uncommon4", 15f }, { "Uncommon3", 10f }, { "Uncommon2", 7f }, { "Uncommon1", 5f },
            { "Rare4", 20f }, { "Rare3", 15f }, { "Rare2", 10f }, { "Rare1", 5f },
            { "Hero4", 7f }, { "Hero3", 5f }, { "Hero2", 3f }, { "Hero1", 2f },
            { "Legendary4", 2f }, { "Legendary3", 1.5f }, { "Legendary2", 1f }, { "Legendary1", 0.8f },
            { "Mythic4", 1f }, { "Mythic3", 0.8f }, { "Mythic2", 0.6f }, { "Mythic1", 0.5f },
            { "Ultimate1", 0.2f }
            };

        // ���� ���൵ ���
        float levelProgress = Mathf.Clamp01(summonLevel / 50f);

        // Ȯ�� ���
        Dictionary<string, float> adjustedRates = new Dictionary<string, float>();
        foreach (var pair in baseRates)
        {
            string itemKey = pair.Key;

            // Ȯ�� ���
            float baseRate = baseRates[itemKey];
            float maxRate = maxRates[itemKey];
            adjustedRates[itemKey] = Mathf.Lerp(baseRate, maxRate, levelProgress);
        }

        // Ȯ�� ��й�
        return NormalizeRates(adjustedRates);
    }


    /// <summary>
    /// Ȯ�� ��й� (�հ谡 100%�� �ǵ��� ����)
    /// </summary>
    private Dictionary<string, float> NormalizeRates(Dictionary<string, float> rates)
    {
        float total = 0f;

        foreach (var rate in rates.Values)
        {
            total += rate;
        }

        if (total <= 0f) return rates; // ������ 0�̸� �״�� ��ȯ

        Dictionary<string, float> normalizedRates = new Dictionary<string, float>();
        foreach (var pair in rates)
        {
            if (pair.Value > 0f)
            {
                normalizedRates[pair.Key] = (pair.Value / total) * 100f;
            }
            else
            {
                normalizedRates[pair.Key] = 0f;
            }
        }

        return normalizedRates;
    }
}
