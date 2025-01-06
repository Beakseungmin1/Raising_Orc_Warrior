using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("ChapterSOList")]
    public List<ChapterSO> chapterSOs;

    [Header("Current Chapter's StageSO")]
    public List<StageSO> stageSOs; //�� ���������� ��������SO

    [Header("Current Chapter's BossStageSO")]
    public BossStageSO bossStageSO;

    [Header("Info")]
    public string chapterName;
    public string stageName;
    public Sprite bgSprite;

    public int curChapterIndex = 0; //��ü é�ͷμ��� ���� ex) 99999é�ͱ��� ����
    public int curStageIndex = 0; //��ü ���������μ��� ���� ex) 99999�������� ���� ����

    public int curStageIndexInThisChapter = 0; //���� é�Ϳ����� �������� �ε��������� ex) é��5�� 10��° ��������
    public int MaxStageIndexInThisChapter = 0; //�� é���� �������� �ε��� �ִ�ġ ex) é�� 5���� �ִ� 20���������� ����.

    public Action OnChapterChanged;

    int savedCurStageIndexInThisChapter = 0;

    public Timer timer;

    private void Awake()
    {
        bgSprite = GetComponent<Sprite>();

        SetChapterList();
        SetStageList();
        SetBossStage();

        GameEventsManager.Instance.stageEvents.onStageChange += RefreshStage;
        GameEventsManager.Instance.stageEvents.onChapterChange += RefreshChapter;
    }

    private void SetChapterList()
    {
        chapterSOs.Clear();
        foreach (var chapter in Resources.LoadAll<ChapterSO>("Chapters")) //���ʿ� �ѹ��� LoadAll �ϴ��� ���ҽ� ���鿡�� �����ʴ�.
        {
            chapterSOs.Add(chapter);
        }
    }

    private void SetStageList()
    {
        stageSOs.Clear();
        //�ش� é�Ϳ� �ش��ϴ� �������� ����Ʈ�� �� �̾Ƽ� �޾ƿ´�.
        for (int i = 0; i < chapterSOs[curChapterIndex].stageSOs.Count; i++)
        {
            stageSOs.Add(chapterSOs[curChapterIndex].stageSOs[i]);
        }
        MaxStageIndexInThisChapter = chapterSOs[curChapterIndex].stageSOs.Count - 1; //�ִ� �������� �� ����. //Length�� 1���� ����. index�� 0���� �����̹Ƿ� -1.
    }

    private void SetBossStage()
    {
        for (int i = 0; i < chapterSOs.Count; i++)
        {
            bossStageSO = chapterSOs[curChapterIndex].bossStageSO;
        }
    }

    public void StageClear()
    {
        Debug.LogWarning("StageClear");

        if (curStageIndexInThisChapter < MaxStageIndexInThisChapter) //é�Ϳ� ���� ���������� ���Ҵٸ� ���� ���������� �̵�
        {
            curStageIndex++;
            curStageIndexInThisChapter++;
            GoToStage();
        }
        else //���簡 é���� ������ ����������� �ش� �������� �ݺ�
        {
            savedCurStageIndexInThisChapter = curStageIndexInThisChapter;

            curStageIndexInThisChapter = savedCurStageIndexInThisChapter;
            GoToStage();
        }
    }

    public void RefreshChapter()
    {
        bgSprite = chapterSOs[curChapterIndex].bgSprite;
        chapterName = chapterSOs[curChapterIndex].chapterName;
    }

    public void RefreshStage()
    {
        stageName = stageSOs[curStageIndexInThisChapter].stageName;
    }

    public void GoToStage()
    {
        RegenManager.Instance.CacheEnemies();
        RegenManager.Instance.RegenStagesEnemy();
        GameEventsManager.Instance.stageEvents.ChangeStage();
    }

    public void GoToBossStage()
    {
        savedCurStageIndexInThisChapter = curStageIndexInThisChapter;
        curStageIndex++;
        UIManager.Instance.Hide<StageInfoUI>();
        UIManager.Instance.Show<BossStageInfoUI>();
        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        RegenManager.Instance.CacheEnemyBoss();
        RegenManager.Instance.RegenStagesBossEnemy();
        SetTimer(bossStageSO.bossEnemySO.bossTimeLimit);
        GameEventsManager.Instance.stageEvents.ChangeStage();
    }


    public void GoToDungeonStage(DungeonType dungeonType, int level)
    {
        //�������� �̵��ϱ� �� ������ é�Ϳ� �������� ���� ���̺�
        savedCurStageIndexInThisChapter = curStageIndexInThisChapter;
        curStageIndexInThisChapter = savedCurStageIndexInThisChapter;

        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        Dungeon dungeon = DungeonManager.Instance.GetDungeonByTypeAndLevel(dungeonType, level);
        DungeonManager.Instance.currentDungeonInfo = dungeon.info;
        RegenManager.Instance.CacheDungeonBoss(dungeon);
        RegenManager.Instance.RegenStagesEnemyDungeonBoss(dungeon.info);
        SetTimer(dungeon.info.dungeonBoss.bossTimeLimit);
        GameEventsManager.Instance.stageEvents.ChangeStage();
        DungeonManager.Instance.playerIsInDungeon = true;
    }


    private void GoToNextChapter()
    {
        UIManager.Instance.Show<StageInfoUI>();
        curChapterIndex++;
        curStageIndexInThisChapter = 0;
        SetStageList();
        SetBossStage();
        RefreshChapter();
        RefreshStage();
        RegenManager.Instance.CacheEnemies();
        RegenManager.Instance.RegenStagesEnemy();
        GameEventsManager.Instance.stageEvents.ChangeStage();//���� �ִ����, ���� ���� �� ���� �����ؾ��ϹǷ�, RegenStagesEnemy()������ ����.
    }

    public void BossStageClear()
    {
        if (curChapterIndex < chapterSOs.Count - 1)
        {
            UIManager.Instance.Hide<BossStageInfoUI>();
            GoToNextChapter();
        }
        else //���簡 é���� ������ ����������� �ش� �������� �ݺ�
        {
            BackToLastStage();
        }

        if (timer != null)
        {
            Destroy(timer);
        }
    }

    private void SetTimer(float limitTime)
    {
        timer = Instantiate(gameObject, this.transform).AddComponent<Timer>();
        timer.SetLimitTime(limitTime);
    }

    public void BackToLastStage()
    {
        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        curStageIndexInThisChapter = savedCurStageIndexInThisChapter;
        UIManager.Instance.Hide<BossStageInfoUI>();
        UIManager.Instance.Show<StageInfoUI>();
        GoToStage();
    }
}
