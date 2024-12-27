using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    Dictionary<DungeonType, Dictionary<int, Dungeon>> dungeonMap;
    //던전 클리어시 다음 던전레벨
    //던전 난이도 상승 매서드 
    //던전 레벨업 관리 매서드
    //던전에 들어가면 몬스터가 소환된다.

    //관리 기준: 던전 타입, 레벨.

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
        CurrencyManager.Instance.AddCurrency(dungeon.info.currenyType, dungeon.info.rewardAmount);
    }

    private void ClearDungeon(DungeonType dungeonType, int level)
    {
        Dungeon dungeon = GetDungeonByTypeAndLevel(dungeonType, level);
        ClaimRewards(dungeon);
        //다음 레벨 던전 오픈
    }

    private void FinishDungeon(DungeonType dungeonType, int level)
    {
        //Dungeon dungeon = GetDungeonById(id);
        //ClaimRewards(dungeon);
    }

}