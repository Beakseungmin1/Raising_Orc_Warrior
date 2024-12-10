using System;
using System.Collections;
using System.Collections.Generic;

public class SummonDataManager : Singleton<SummonDataManager>
{
    //���� ��ȯ ���� ���� ���ΰ�. //�� �ڵ带 ���⼭ ����ϴ°�?
    public ItemType curSummoningItemType;

    //��ȯ ����
    //��ȯ ������ ���� ���, ��ũ�� ��ȯ Ȯ��, ��ȯ ���� ����ġ�� �����ϴ� ��ũ��Ʈ.
    private Dictionary<ItemType, SummonLevelProgress> summonLevelProgressDictionary;

    public Action OnExpChanged;
    public Action OnLevelChanged;

    // ��޺� ��ȯ Ȯ�� ����
    public float normalGradeSummonRate = 60.5f;
    public float uncommonGradeSummonRate = 20f;
    public float rareGradeSummonRate = 10f;
    public float heroGradeSummonRate = 5f;
    public float legendaryGradeSummonRate = 3f;
    public float mythicGradeSummonRate = 1f;
    public float ultimateGradeSummonRate = 0.5f;

    //4,3,2,1��ũ�� ���� ��ȯ Ȯ��
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
        OnExpChanged.Invoke();

        while (progress.Exp >= progress.ExpToNextLevel)
        {
            progress.Exp -= progress.ExpToNextLevel;
            progress.Level++;
            progress.ExpToNextLevel *= 1.2f; // ���� �� ����ġ ���� (����)
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
