using System;
using System.Collections;
using System.Collections.Generic;

public class SummonDataManager : Singleton<SummonDataManager>
{
    //현재 소환 씬은 무슨 씬인가. //이 코드를 여기서 써야하는가?
    public ItemType curSummoningItemType;

    //소환 레벨
    //소환 레벨에 따른 등급, 랭크별 소환 확률, 소환 레벨 경험치만 관리하는 스크립트.
    private Dictionary<ItemType, SummonLevelProgress> summonLevelProgressDictionary;

    public Action OnExpChanged;
    public Action OnLevelChanged;

    // 등급별 소환 확률 변수
    public float normalGradeSummonRate = 60.5f;
    public float uncommonGradeSummonRate = 20f;
    public float rareGradeSummonRate = 10f;
    public float heroGradeSummonRate = 5f;
    public float legendaryGradeSummonRate = 3f;
    public float mythicGradeSummonRate = 1f;
    public float ultimateGradeSummonRate = 0.5f;

    //4,3,2,1랭크에 따른 소환 확률
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
        OnExpChanged.Invoke();

        while (progress.Exp >= progress.ExpToNextLevel)
        {
            progress.Exp -= progress.ExpToNextLevel;
            progress.Level++;
            progress.ExpToNextLevel *= 1.2f; // 레벨 업 경험치 증가 (예제)
            OnLevelChanged.Invoke();
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
}
