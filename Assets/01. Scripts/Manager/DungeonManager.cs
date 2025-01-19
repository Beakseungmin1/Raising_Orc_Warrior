using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class DungeonManager : Singleton<DungeonManager>
{
    Dictionary<DungeonType, Dictionary<int, Dungeon>> dungeonMap;

    public DungeonInfoSO currentDungeonInfo;

    public bool isPlayerInDungeon = false;

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

    public BigInteger ClaimRewards(Dungeon dungeon, float enemysHplostPercentage)
    {
        BigInteger lastRewardAmount = (BigInteger)(dungeon.info.rewardAmount * enemysHplostPercentage * 100);

        switch (dungeon.type)
        {
            case DungeonType.CubeDungeon:
                CurrencyManager.Instance.AddCurrency(CurrencyType.Cube, (float)lastRewardAmount);
                break; 
            case DungeonType.GoldDungeon:
                CurrencyManager.Instance.AddGold(lastRewardAmount);
                break;
            default:
                PlayerObjManager.Instance.Player.stat.AddExp(lastRewardAmount);
                break;
        }

        return lastRewardAmount;
    }


    public void FinishDungeon(DungeonType dungeonType, int level, BigInteger maxHP, BigInteger hp, bool isCleared, EnemyDungeonBoss enemyDungeonBoss)
    {
        GameEventsManager.Instance.bossEvents.TimerStop();
        enemyDungeonBoss.canAttack = false;

        if (!PlayerObjManager.Instance.Player.PlayerBattle.GetIsDead()) //���� �ʾҴٸ�
        {
            PlayerObjManager.Instance.Player.PlayerBattle.SetPlayerStateStoppedIdle();
        }

        Dungeon dungeon = GetDungeonByTypeAndLevel(dungeonType, level);

        if (isCleared)
        {
            ChangeDungeonState(dungeon.type, level, DungeonState.CLEARED);

            // ���� ���� ���� ����
            if (GetDungeonByTypeAndLevel(dungeonType, level + 1) != null)
            {
                ChangeDungeonState(dungeon.type, level + 1, DungeonState.OPENED);
            }
        }

        // BigInteger�� double�� ��ȯ�Ͽ� ���� ���
        double maxHpDouble = (double)maxHP;
        double currentHpDouble = (double)hp;
        // �սǵ� ü�� ���� ���
        double lostPercentage = (maxHpDouble - currentHpDouble) / maxHpDouble * 100.0;

        BigInteger lastRewardAmount = ClaimRewards(dungeon, (float)lostPercentage);
        
        DungeonRewardPopupUI dungeonRewardPopupUI = UIManager.Instance.Show<DungeonRewardPopupUI>();
        dungeonRewardPopupUI.SetUI(currentDungeonInfo, lastRewardAmount);
    }

    public void ExitDungeon()
    {
        if (PlayerObjManager.Instance.Player.PlayerBattle.GetIsDead() == true)
        {
            StartCoroutine(PlayerObjManager.Instance.Player.PlayerBattle.DelayBeforeResurrection(2f));
        }

        StageManager.Instance.curStageIndexInThisChapter = StageManager.Instance.savedCurStageIndexInThisChapter;

        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        UIManager.Instance.ShowFadePanel<FadeInFadeOutUI>(FadeType.FadeOutFadeIn);
        UIManager.Instance.Hide<BossStageInfoUI>();
        UIManager.Instance.Show<StageInfoUI>();
        StageManager.Instance.GoToStage();

        currentDungeonInfo = null;
        isPlayerInDungeon = false;

        SetCamera camera = Camera.main.gameObject.GetComponent<SetCamera>();
        camera.SetCameraPosY(0f);
        camera.SetCameraSize(5f);

        PlayerObjManager.Instance.Player.PlayerBattle.SetPlayerStateIdle();
    }


    public void ChangeDungeonState(DungeonType dungeonType, int level, DungeonState state)
    {
        Dungeon dungeon = GetDungeonByTypeAndLevel(dungeonType, level);
        dungeon.state = state;
    }


}