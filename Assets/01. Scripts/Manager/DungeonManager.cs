using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class DungeonManager : Singleton<DungeonManager>
{
    Dictionary<DungeonType, Dictionary<int, Dungeon>> dungeonMap;

    private void Awake()
    {
        dungeonMap = CreateDungeonMap();
    }

    Dictionary<DungeonType, Dictionary<int, Dungeon>> CreateDungeonMap()
    {
        Dictionary<DungeonType, Dictionary<int, Dungeon>> dungeonMap = new Dictionary<DungeonType, Dictionary<int, Dungeon>>();

        DungeonInfoSO[] goldDungeons = Resources.LoadAll<DungeonInfoSO>("Dungeons/GoldDungeons");
        Dictionary<int, Dungeon> levelToGoldDungeonMap = new Dictionary<int, Dungeon>();
        foreach (DungeonInfoSO dungeonInfo in goldDungeons)
        {
            if (levelToGoldDungeonMap.ContainsKey(dungeonInfo.level))
            {
                Debug.LogWarning("던전맵을 생성하던 중 중복된 레벨를 찾았습니다: " + dungeonInfo.level);
            }
            levelToGoldDungeonMap.Add(dungeonInfo.level, LoadDungeon(dungeonInfo));
        }
        dungeonMap[DungeonType.GoldDungeon] = levelToGoldDungeonMap;


        DungeonInfoSO[] cubeDungeons = Resources.LoadAll<DungeonInfoSO>("Dungeons/CubeDungeons");
        Dictionary<int, Dungeon> levelToCubeDungeonMap = new Dictionary<int, Dungeon>();
        foreach (DungeonInfoSO dungeonInfo in cubeDungeons)
        {
            if (levelToCubeDungeonMap.ContainsKey(dungeonInfo.level))
            {
                Debug.LogWarning("던전맵을 생성하던 중 중복된 레벨를 찾았습니다: " + dungeonInfo.level);
            }
            levelToCubeDungeonMap.Add(dungeonInfo.level, LoadDungeon(dungeonInfo));
        }
        dungeonMap[DungeonType.CubeDungeon] = levelToCubeDungeonMap;


        DungeonInfoSO[] expDungeons = Resources.LoadAll<DungeonInfoSO>("Dungeons/ExpDungeons");
        Dictionary<int, Dungeon> levelToExpDungeonMap = new Dictionary<int, Dungeon>();
        foreach (DungeonInfoSO dungeonInfo in expDungeons)
        {
            if (levelToExpDungeonMap.ContainsKey(dungeonInfo.level))
            {
                Debug.LogWarning("던전맵을 생성하던 중 중복된 레벨를 찾았습니다: " + dungeonInfo.level);
            }
            levelToExpDungeonMap.Add(dungeonInfo.level, LoadDungeon(dungeonInfo));
        }
        dungeonMap[DungeonType.EXPDungeon] = levelToExpDungeonMap;

        return dungeonMap;
    }

    private Dungeon LoadDungeon(DungeonInfoSO dungeonInfo)
    {
        Dungeon dungeon = null;
        try
        {
            dungeon = new Dungeon(dungeonInfo);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load dungeon with level " + dungeonInfo.level + ": " + e);
        }
        return dungeon;
    }

    public Dungeon GetDungeonByTypeAndLevel(DungeonType dungeonType, int level)
    {
        // dungeonMap에 주어진 던전 타입이 있는지 확인
        if (dungeonMap.TryGetValue(dungeonType, out Dictionary<int, Dungeon> levelToDungeonMap))
        {
            // 내부 딕셔너리에 주어진 레벨이 있는지 확인
            if (levelToDungeonMap.TryGetValue(level, out Dungeon dungeon))
            {
                return dungeon; // 찾은 던전을 반환
            }
            else
            {
                Debug.LogWarning($"타입 {dungeonType} 및 레벨 {level}에 해당하는 던전을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning($"던전 타입 {dungeonType}이(가) 던전 맵에 존재하지 않습니다.");
        }

        return null; // 던전을 찾지 못한 경우 null 반환
    }

    private void ClaimRewards(Dungeon dungeon)
    {
        switch(dungeon.type)
        {
            case DungeonType.CubeDungeon:
                CurrencyManager.Instance.AddCurrency(CurrencyType.Cube, dungeon.info.rewardAmount);
                break;
            case DungeonType.GoldDungeon:
                CurrencyManager.Instance.AddCurrency(CurrencyType.Gold, dungeon.info.rewardAmount);
                break;
            default:
                PlayerObjManager.Instance.Player.stat.AddExp((BigInteger)dungeon.info.rewardAmount);
                break;
        }
    }

    public void ClearDungeon(DungeonType dungeonType, int level)
    {
        Dungeon dungeon = GetDungeonByTypeAndLevel(dungeonType, level);
        ChangeDungeonState(dungeon.type, level, DungeonState.CLEARED);
        ClaimRewards(dungeon);

        //다음 레벨 던전 오픈
        if (GetDungeonByTypeAndLevel(dungeonType, level+1) != null)
        {
            ChangeDungeonState(dungeon.type, level + 1, DungeonState.OPENED);
        }

        UIManager.Instance.Show<StageInfoUI>();
        StageManager.Instance.GoToNextStage();
    }

    public void ChangeDungeonState(DungeonType dungeonType, int level, DungeonState state)
    {
        Dungeon dungeon = GetDungeonByTypeAndLevel(dungeonType, level);
        dungeon.state = state;
    }

}