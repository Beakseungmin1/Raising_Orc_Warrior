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

        // 소환 레벨이 최대값(50)인 경우 경험치를 추가하지 않음
        if (progress.Level >= 50)
        {
            progress.Exp = 0f; // 50레벨일 때 경험치는 항상 0으로 유지
            OnExpChanged?.Invoke();
            return;
        }

        progress.Exp += experience;
        OnExpChanged?.Invoke();

        while (progress.Exp >= progress.ExpToNextLevel)
        {
            progress.Exp -= progress.ExpToNextLevel;
            progress.Level++;

            // 레벨 업 경험치 증가
            progress.ExpToNextLevel += 5f;

            // 소환 레벨 최대값(50) 체크
            if (progress.Level >= 50)
            {
                progress.Level = 50; // 최대 레벨 고정
                progress.Exp = 0f;   // 50레벨 도달 시 경험치 0으로 고정
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
    /// 소환 가능한 모든 아이템의 초기 확률을 설정
    /// </summary>
    public Dictionary<string, float> GetAdjustedSummonRates(int summonLevel, bool isWeapon, bool isSkill = false)
    {
        // 기본 확률 (레벨 1)
        Dictionary<string, float> baseRates = isSkill
            ? new Dictionary<string, float> // Skill 전용 확률 테이블
            {
            { "Normal", 30f },
            { "Uncommon", 20f },
            { "Rare", 18f },
            { "Hero", 15f },
            { "Legendary", 12f },
            { "Mythic", 8f },
            { "Ultimate", 5f }
            }
            : new Dictionary<string, float> // Weapon/Accessory 확률 테이블
            {
            { "Normal4", 15f }, { "Normal3", 10f }, { "Normal2", 8f }, { "Normal1", 7f },
            { "Uncommon4", 10f }, { "Uncommon3", 8f }, { "Uncommon2", 7f }, { "Uncommon1", 5f },
            { "Rare4", 8f }, { "Rare3", 5f }, { "Rare2", 4f }, { "Rare1", 3f },
            { "Hero4", 2f }, { "Hero3", 1.5f }, { "Hero2", 1f }, { "Hero1", 0.5f },
            { "Legendary4", 1f }, { "Legendary3", 0.8f }, { "Legendary2", 0.7f }, { "Legendary1", 0.5f },
            { "Mythic4", 0.76f }, { "Mythic3", 0.57f }, { "Mythic2", 0.38f }, { "Mythic1", 0.19f },
            { "Ultimate1", 0.001f }
            };

        // 소환 레벨 50의 목표 확률
        Dictionary<string, float> maxRates = isSkill
            ? new Dictionary<string, float> // Skill 전용 확률 테이블 (레벨 50 목표)
            {
            { "Normal", 5f },
            { "Uncommon", 10f },
            { "Rare", 30f },
            { "Hero", 25f },
            { "Legendary", 20f },
            { "Mythic", 9f },
            { "Ultimate", 10f }
            }
            : new Dictionary<string, float> // Weapon/Accessory 확률 테이블
            {
            { "Normal4", 10f }, { "Normal3", 7f }, { "Normal2", 5f }, { "Normal1", 3f },
            { "Uncommon4", 15f }, { "Uncommon3", 10f }, { "Uncommon2", 7f }, { "Uncommon1", 5f },
            { "Rare4", 20f }, { "Rare3", 15f }, { "Rare2", 10f }, { "Rare1", 5f },
            { "Hero4", 7f }, { "Hero3", 5f }, { "Hero2", 3f }, { "Hero1", 2f },
            { "Legendary4", 2f }, { "Legendary3", 1.5f }, { "Legendary2", 1f }, { "Legendary1", 0.8f },
            { "Mythic4", 1f }, { "Mythic3", 0.8f }, { "Mythic2", 0.6f }, { "Mythic1", 0.5f },
            { "Ultimate1", 0.2f }
            };

        // 레벨 진행도 계산
        float levelProgress = Mathf.Clamp01(summonLevel / 50f);

        // 확률 계산
        Dictionary<string, float> adjustedRates = new Dictionary<string, float>();
        foreach (var pair in baseRates)
        {
            string itemKey = pair.Key;

            // 확률 계산
            float baseRate = baseRates[itemKey];
            float maxRate = maxRates[itemKey];
            adjustedRates[itemKey] = Mathf.Lerp(baseRate, maxRate, levelProgress);
        }

        // 확률 재분배
        return NormalizeRates(adjustedRates);
    }


    /// <summary>
    /// 확률 재분배 (합계가 100%가 되도록 조정)
    /// </summary>
    private Dictionary<string, float> NormalizeRates(Dictionary<string, float> rates)
    {
        float total = 0f;

        foreach (var rate in rates.Values)
        {
            total += rate;
        }

        if (total <= 0f) return rates; // 총합이 0이면 그대로 반환

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
