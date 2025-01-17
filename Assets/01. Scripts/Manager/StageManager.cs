using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("ChapterSOList")]
    public List<ChapterSO> chapterSOs;

    [Header("Current Chapter's StageSO")]
    public List<StageSO> stageSOs; //이 스테이지의 스테이지SO

    [Header("Current Chapter's BossStageSO")]
    public List<BossStageSO> bossStageSOs;

    [Header("Info")]
    public string chapterName;
    public string stageName;

    public int curChapterIndex = 0; //전체 챕터로서의 순번 ex) 99999챕터까지 가능
    public int curStageIndex = 0; //전체 스테이지로서의 순번 ex) 99999스테이지 까지 가능

    public int curStageIndexInThisChapter = 0; //현재 챕터에서의 스테이지 인덱스정보값 ex) 챕터5의 10번째 스테이지
    public int MaxStageIndexInThisChapter = 0; //현 챕터의 스테이지 인덱스 최대치 ex) 챕터 5에는 최대 20스테이지가 있음.

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
        foreach (var chapter in Resources.LoadAll<ChapterSO>("Chapters")) //최초에 한번만 LoadAll 하더라도 리소스 측면에서 좋지않다.
        {
            chapterSOs.Add(chapter);
        }
    }

    private void SetStageList()
    {
        stageSOs.Clear();
        //해당 챕터에 해당하는 스테이지 리스트를 쫙 뽑아서 받아온다.
        for (int i = 0; i < chapterSOs[curChapterIndex].stageSOs.Count; i++)
        {
            stageSOs.Add(chapterSOs[curChapterIndex].stageSOs[i]);
        }
        MaxStageIndexInThisChapter = chapterSOs[curChapterIndex].stageSOs.Count - 1; //최대 스테이지 값 세팅. //Length는 1부터 시작. index는 0부터 시작이므로 -1.
    }

    private void SetBossStageList()
    {
        bossStageSOs.Clear();
        for (int i = 0; i < chapterSOs[curChapterIndex].bossStageSOs.Count; i++)
        {
            bossStageSOs.Add(chapterSOs[curChapterIndex].bossStageSOs[i]);
        }
    }

    public void StageClear() //보스스테이지는 못깼고 일반스테이지까지만 꺤 경우임
    {
        if(isThisBossStageFirstTry)
        {
            GoToBossStage();
        }
        else
        {
            GoToStage(); //현재 스테이지 반복
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

        if (curStageIndexInThisChapter < MaxStageIndexInThisChapter) //챕터에 다음 스테이지가 남았다면 다음 스테이지로 이동
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
        else  //현재가 챕터의 마지막 스테이지라면 해당 스테이지 반복
        {
            Debug.LogWarning("업데이트가 안돼서 더이상 챕터가 없습니다. 마지막 스테이지로 돌아갑니다");
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
        //던전으로 이동하기 전 마지막 챕터와 스테이지 정보 세이브
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
        GameEventsManager.Instance.stageEvents.ChangeStage();//현재 최대몬스터, 죽인 몬스터 수 정보 갱신해야하므로, RegenStagesEnemy()다음에 실행.
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
