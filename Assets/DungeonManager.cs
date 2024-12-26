using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : Singleton<DungeonManager>
{
    public List<DungeonSO> expDungeonSOs;
    public List<DungeonSO> goldDungeonSOs;
    public List<DungeonSO> cubeDungeonSOs;

    Dictionary<DungeonType, int> dungeonLevelMap = new Dictionary<DungeonType, int>();
    //���� �������� �ż��� //ClaimReward
    //���� Ŭ����� ���� ��������
    //���� ���̵� ��� �ż��� 
    //���� ������ ���� �ż���
    //������ ���� ���Ͱ� ��ȯ�ȴ�.

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