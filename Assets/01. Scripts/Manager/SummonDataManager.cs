using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
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

            // 레벨 업 경험치 증가 (예제)
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

    /// <summary>
    /// 현재 소환 확률을 반환. 레벨, 랭크, 등급을 모두 고려하여 계산
    /// </summary>
    public Dictionary<string, float> GetAdjustedSummonRates(int summonLevel, bool isWeapon)
    {
        // 기본 확률 (레벨 1)
        Dictionary<string, float> baseRates = new Dictionary<string, float>
    {
        // Normal 총합: 40
        { "Normal4", 15f },
        { "Normal3", 10f },
        { "Normal2", 8f },
        { "Normal1", 7f },

        // Uncommon 총합: 30
        { "Uncommon4", 10f },
        { "Uncommon3", 8f },
        { "Uncommon2", 7f },
        { "Uncommon1", 5f },

        // Rare 총합: 20
        { "Rare4", 8f },
        { "Rare3", 5f },
        { "Rare2", 4f },
        { "Rare1", 3f },

        // Hero 총합: 5
        { "Hero4", 2f },
        { "Hero3", 1.5f },
        { "Hero2", 1f },
        { "Hero1", 0.5f },

        // Legendary 총합: 3
        { "Legendary4", 1f },
        { "Legendary3", 0.8f },
        { "Legendary2", 0.7f },
        { "Legendary1", 0.5f },

        // Ultimate: 0.1
        { "Ultimate1", 0.1f },

        // Mythic 총합: 1.9
        { "Mythic4", 0.76f },
        { "Mythic3", 0.57f },
        { "Mythic2", 0.38f },
        { "Mythic1", 0.19f }
    };

        // 소환 레벨 50의 목표 확률
        Dictionary<string, float> maxRates = new Dictionary<string, float>
    {
        { "Normal4", 10f },
        { "Normal3", 7f },
        { "Normal2", 5f },
        { "Normal1", 3f },

        { "Uncommon4", 15f },
        { "Uncommon3", 10f },
        { "Uncommon2", 7f },
        { "Uncommon1", 5f },

        { "Rare4", 20f },
        { "Rare3", 15f },
        { "Rare2", 10f },
        { "Rare1", 5f },

        { "Hero4", 7f },
        { "Hero3", 5f },
        { "Hero2", 3f },
        { "Hero1", 2f },

        { "Legendary4", 2f },
        { "Legendary3", 1.5f },
        { "Legendary2", 1f },
        { "Legendary1", 0.8f },

        { "Mythic4", 1f },
        { "Mythic3", 0.8f },
        { "Mythic2", 0.5f },
        { "Mythic1", 0.2f },

        { "Ultimate1", 0.5f }
    };

        // 현재 레벨 기반 확률 조정
        float levelProgress = Mathf.Clamp01(summonLevel / 50f);

        Dictionary<string, float> adjustedRates = new Dictionary<string, float>();
        foreach (var pair in baseRates)
        {
            string itemKey = pair.Key;
            float baseRate = pair.Value;
            float maxRate = maxRates[itemKey];

            // Lerp를 통해 레벨 기반 확률 조정
            adjustedRates[itemKey] = Mathf.Lerp(baseRate, maxRate, levelProgress);
        }

        // 소지 여부 기반으로 확률을 조정
        foreach (var itemKey in new List<string>(adjustedRates.Keys))
        {
            if (!CanSummonItem(itemKey, isWeapon))
            {
                adjustedRates[itemKey] = 0f; // 소환 불가능한 아이템은 확률 0%
            }
        }

        // 확률 재분배
        return NormalizeRates(adjustedRates);
    }


    /// <summary>
    /// 소환 가능 여부를 판단 (다음 등급의 4랭크를 소환하려면 이전 등급의 1랭크가 필요)
    /// </summary>
    private bool CanSummonItem(string itemKey, bool isWeapon)
    {
        // itemKey는 "GradeRank" 형식 (예: "Rare4")
        string grade = itemKey.Substring(0, itemKey.Length - 1); // 등급 추출 (예: "Rare")
        int rank = int.Parse(itemKey[^1].ToString()); // 랭크 추출 (예: "4")

        // Rank == 4인 경우: 이전 등급의 1랭크 필요
        if (rank == 4)
        {
            string previousGrade = GetPreviousGrade(grade); // 이전 등급 추출
            if (!string.IsNullOrEmpty(previousGrade))
            {
                string requiredItemKey = $"{previousGrade}1"; // 이전 등급의 1랭크
                return PlayerObjManager.Instance.Player.inventory.HasRequiredItem(requiredItemKey, isWeapon);
            }
        }
        // Rank != 4인 경우: 같은 등급의 Rank+1 아이템 필요
        else
        {
            string requiredItemKey = $"{grade}{rank + 1}"; // 같은 등급의 상위 Rank
            return PlayerObjManager.Instance.Player.inventory.HasRequiredItem(requiredItemKey, isWeapon);
        }

        return true; // 기본적으로 소환 가능
    }


    /// <summary>
    /// 이전 등급 반환
    /// </summary>
    private string GetPreviousGrade(string currentGrade)
    {
        switch (currentGrade)
        {
            case "Uncommon": return "Normal";
            case "Rare": return "Uncommon";
            case "Hero": return "Rare";
            case "Legendary": return "Hero";
            case "Mythic": return "Legendary";
            case "Ultimate": return "Mythic";
            default: return null; // Normal은 이전 등급 없음
        }
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
            normalizedRates[pair.Key] = (pair.Value / total) * 100f; // 비율 조정
        }

        return normalizedRates;
    }
}