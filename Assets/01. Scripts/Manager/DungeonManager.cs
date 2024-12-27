using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    Dictionary<DungeonType, Dictionary<int, Dungeon>> dungeonMap;
    //���� Ŭ����� ���� ��������
    //���� ���̵� ��� �ż��� 
    //���� ������ ���� �ż���
    //������ ���� ���Ͱ� ��ȯ�ȴ�.

    //���� ����: ���� Ÿ��, ����.

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
                Debug.LogWarning("�������� �����ϴ� �� �ߺ��� ������ ã�ҽ��ϴ�: " + dungeonInfo.level);
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
                Debug.LogWarning("�������� �����ϴ� �� �ߺ��� ������ ã�ҽ��ϴ�: " + dungeonInfo.level);
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
                Debug.LogWarning("�������� �����ϴ� �� �ߺ��� ������ ã�ҽ��ϴ�: " + dungeonInfo.level);
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
        // dungeonMap�� �־��� ���� Ÿ���� �ִ��� Ȯ��
        if (dungeonMap.TryGetValue(dungeonType, out Dictionary<int, Dungeon> levelToDungeonMap))
        {
            // ���� ��ųʸ��� �־��� ������ �ִ��� Ȯ��
            if (levelToDungeonMap.TryGetValue(level, out Dungeon dungeon))
            {
                return dungeon; // ã�� ������ ��ȯ
            }
            else
            {
                Debug.LogWarning($"Ÿ�� {dungeonType} �� ���� {level}�� �ش��ϴ� ������ ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"���� Ÿ�� {dungeonType}��(��) ���� �ʿ� �������� �ʽ��ϴ�.");
        }

        return null; // ������ ã�� ���� ��� null ��ȯ
    }

    private void ClaimRewards(Dungeon dungeon)
    {
        CurrencyManager.Instance.AddCurrency(dungeon.info.currenyType, dungeon.info.rewardAmount);
    }

    private void ClearDungeon(DungeonType dungeonType, int level)
    {
        Dungeon dungeon = GetDungeonByTypeAndLevel(dungeonType, level);
        ClaimRewards(dungeon);
        //���� ���� ���� ����
    }

    private void FinishDungeon(DungeonType dungeonType, int level)
    {
        //Dungeon dungeon = GetDungeonById(id);
        //ClaimRewards(dungeon);
    }

}