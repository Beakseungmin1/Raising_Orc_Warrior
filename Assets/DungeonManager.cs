using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    public List<DungeonSO> expDungeonSOs;
    public List<DungeonSO> goldDungeonSOs;
    public List<DungeonSO> cubeDungeonSOs;

    Dictionary<DungeonType, int> dungeonLevelMap = new Dictionary<DungeonType, int>();
    //던전 보상지급 매서드 //ClaimReward
    //던전 클리어시 다음 던전레벨
    //던전 난이도 상승 매서드 
    //던전 레벨업 관리 매서드
    //던전에 들어가면 몬스터가 소환된다.

    private void Awake()
    {
        
    }

    public void GoToDungeonStage(DungeonType dungeonType, int dungeonLevel)
    {
        UIManager.Instance.Hide<StageInfoUI>();
        RegenManager.Instance.ClearEnemies();
        RegenManager.Instance.CacheEnemyBoss();
        RegenManager.Instance.RegenStagesBossEnemy();
        StageManager.Instance.OnStageChanged?.Invoke();
    }

    private void SetDungeonStage()
    {
        
    }

    public void ClaimRewards()
    {

    }

    public void OpenNextDungeonLevel()
    {

    }


}