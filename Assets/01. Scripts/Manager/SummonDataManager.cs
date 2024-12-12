using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SummonDataManager : Singleton<SummonDataManager>
{
    //소환 레벨에 따른 등급, 랭크별 소환 확률, 소환 레벨 경험치만 관리하는 스크립트.
    private Dictionary<ItemType, SummonLevelProgress> summonLevelProgressDictionary;

    public Action OnExpChanged;
    public Action OnLevelChanged;

    private Dictionary<Grade, float> summonRates;

    //4,3,2,1랭크에 따른 소환 확률 (고정)
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

    public void AddExperience(ItemType type, float experience) //소환횟수 한번당 경험치 +1
    {
        if (!summonLevelProgressDictionary.ContainsKey(type)) return;

        var progress = summonLevelProgressDictionary[type];
        progress.Exp += experience;
        OnExpChanged?.Invoke();

        while (progress.Exp >= progress.ExpToNextLevel)
        {
            progress.Exp -= progress.ExpToNextLevel;
            progress.Level++;
            progress.ExpToNextLevel *= 1.2f; // 레벨 업 경험치 증가 (예제)
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
        // 기본 소환 확률 (초기 값)
        float normalBaseRate = 60.5f;
        float uncommonBaseRate = 20f;
        float rareBaseRate = 10f;
        float heroBaseRate = 5f;
        float legendaryBaseRate = 3f;
        float mythicBaseRate = 1f;
        float ultimateBaseRate = 0.5f;

        // 레벨 비례 증가율
        float levelMultiplier = Mathf.Clamp01(summonLevel / 50f); // 0~1 사이 값
        float totalRate = 100f;

        // 상위 등급 확률 계산
        float ultimateRate = ultimateBaseRate + (levelMultiplier * 4f); // 최대 4% 증가
        float mythicRate = Mathf.Lerp(mythicBaseRate, 10f, levelMultiplier); // 목표값 10%로 수렴
        float legendaryRate = legendaryBaseRate + (levelMultiplier * 7f); // 최대 7% 증가

        // 상위 등급 증가량 계산
        float adjustment = ultimateRate + mythicRate + legendaryRate
            - (ultimateBaseRate + mythicBaseRate + legendaryBaseRate);

        // 하위 등급 확률 조정
        float normalRate = Mathf.Lerp(normalBaseRate, 20f, levelMultiplier); // 목표값 20%로 수렴
        float uncommonRate = uncommonBaseRate - (adjustment * 0.3f); // 30% 비중 감소
        float rareRate = rareBaseRate - (adjustment * 0.2f); // 20% 비중 감소

        // 딕셔너리로 반환
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
        50레벨 최종값
        등급 확률
        Normal  20.0 %
        Uncommon    약 13.0 %
        Rare    약 6.5 %
        Hero    5.0 %
        Legendary   약 10.0 %
        Mythic  10.0 %
        Ultimate    약 4.5 %
        */
    }
}
