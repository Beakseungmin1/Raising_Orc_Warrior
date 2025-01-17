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
    public List<BossStageSO> bossStageSOs;

    [Header("Info")]
    public string chapterName;
    public string stageName;

    public int curChapterIndex = 0; //��ü é�ͷμ��� ���� ex) 99999é�ͱ��� ����
    public int curStageIndex = 0; //��ü ���������μ��� ���� ex) 99999�������� ���� ����

    public int curStageIndexInThisChapter = 0; //���� é�Ϳ����� �������� �ε��������� ex) é��5�� 10��° ��������
    public int MaxStageIndexInThisChapter = 0; //�� é���� �������� �ε��� �ִ�ġ ex) é�� 5���� �ִ� 20���������� ����.

    public int savedCurStageIndexInThisChapter = 0;

    public Action OnChapterChanged;

    public Timer timer;

    private bool isThisBossStageFirstTry = true;

    private void Awake()
    {
        SetChapterList();
        SetStageList();
        SetBossStageList();

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

    private void SetBossStageList()
    {
        bossStageSOs.Clear();
        for (int i = 0; i < chapterSOs[curChapterIndex].bossStageSOs.Count; i++)
        {
            bossStageSOs.Add(chapterSOs[curChapterIndex].bossStageSOs[i]);
        }
    }

    public void StageClear() //�������������� ������ �Ϲݽ������������� �� �����
    {
        if(isThisBossStageFirstTry)
        {
            GoToBossStage();
        }
        else
        {
            GoToStage(); //���� �������� �ݺ�
        }
        BattleManager.Instance.EndBattle();
        isThisBossStageFirstTry = false;
    }

    public void BossStageClear()
    {
        if (timer != null)
        {
            Destroy(timer);
        }

        if (curStageIndexInThisChapter < MaxStageIndexInThisChapter) //é�Ϳ� ���� ���������� ���Ҵٸ� ���� ���������� �̵�
        {
            UIManager.Instance.Hide<BossStageInfoUI>();
            UIManager.Instance.Show<StageInfoUI>();
            curStageIndex++;
            curStageIndexInThisChapter++;
            isThisBossStageFirstTry = true;
            GoToStage();
        }
        else if (curChapterIndex < chapterSOs.Count - 1)
        {
            UIManager.Instance.Hide<BossStageInfoUI>();
            GoToNextChapter();
            isThisBossStageFirstTry = true;
        }
        else  //���簡 é���� ������ ����������� �ش� �������� �ݺ�
        {
            Debug.LogWarning("������Ʈ�� �ȵż� ���̻� é�Ͱ� �����ϴ�. ������ ���������� ���ư��ϴ�");
            BackToLastStage();
            isThisBossStageFirstTry = false;
        }
    }

    public void RefreshChapter()
    {
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
        UIManager.Instance.ShowFadePanel<FadeInFadeOutUI>(FadeType.FadeOutFadeIn);
        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        savedCurStageIndexInThisChapter = curStageIndexInThisChapter;
        UIManager.Instance.Hide<StageInfoUI>();
        UIManager.Instance.Show<BossStageInfoUI>();
        RegenManager.Instance.CacheEnemyBoss();
        RegenManager.Instance.RegenStagesBossEnemy();
        SetTimer(bossStageSOs[curStageIndexInThisChapter].bossEnemySO.bossTimeLimit);
        GameEventsManager.Instance.stageEvents.ChangeStage();
    }


    public void GoToDungeonStage(DungeonType dungeonType, int level)
    {
        SetCamera camera = Camera.main.gameObject.GetComponent<SetCamera>();
        camera.SetCameraPosY(-0.2f);
        camera.SetCameraSize(6f);

        UIManager.Instance.ShowFadePanel<FadeInFadeOutUI>(FadeType.FadeOutFadeIn);
        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        //�������� �̵��ϱ� �� ������ é�Ϳ� �������� ���� ���̺�
        savedCurStageIndexInThisChapter = curStageIndexInThisChapter;
        curStageIndexInThisChapter = savedCurStageIndexInThisChapter;
        Dungeon dungeon = DungeonManager.Instance.GetDungeonByTypeAndLevel(dungeonType, level);
        DungeonManager.Instance.currentDungeonInfo = dungeon.info;
        RegenManager.Instance.CacheDungeonBoss(dungeon);
        RegenManager.Instance.RegenStagesEnemyDungeonBoss(dungeon.info);
        SetTimer(dungeon.info.dungeonBoss.bossTimeLimit);
        GameEventsManager.Instance.stageEvents.ChangeStage();
        DungeonManager.Instance.playerIsInDungeon = true;
        BattleManager.Instance.EndBattle();
    }


    private void GoToNextChapter()
    {
        UIManager.Instance.ShowFadePanel<FadeInFadeOutUI>(FadeType.FadeOutFadeIn);
        UIManager.Instance.Show<StageInfoUI>();
        curChapterIndex++;
        curStageIndex++;
        curStageIndexInThisChapter = 0;
        BackgroundManager.Instance.ChangeBackGround();
        SetStageList();
        SetBossStageList();
        RefreshChapter();
        RefreshStage();
        RegenManager.Instance.CacheEnemies();
        RegenManager.Instance.RegenStagesEnemy();
        GameEventsManager.Instance.stageEvents.ChangeStage();//���� �ִ����, ���� ���� �� ���� �����ؾ��ϹǷ�, RegenStagesEnemy()������ ����.
    }

    private void SetTimer(float limitTime)
    {
        timer = Instantiate(gameObject, this.transform).AddComponent<Timer>();
        timer.SetLimitTime(limitTime);
    }

    public void BackToLastStage()
    {
        UIManager.Instance.Hide<BossStageInfoUI>();
        UIManager.Instance.ShowFadePanel<FadeInFadeOutUI>(FadeType.FadeOutFadeIn);
        GameEventsManager.Instance.enemyEvents.ClearEnemy();
        curStageIndexInThisChapter = savedCurStageIndexInThisChapter;
        UIManager.Instance.Show<StageInfoUI>();
        GoToStage();
    }

    public void ResetStage()
    {
        SetStageList();
        SetBossStageList();
        RefreshChapter();
        RefreshStage();
    }

}
