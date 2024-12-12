using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("ChapterSOList")]
    public List<ChapterSO> chapterSOs;

    [Header("Current Chapter StageSO")]
    public List<StageSO> stageSOs; //이 스테이지의 스테이지SO

    [Header("Info")]
    public string chapterName;
    public string stageName;
    public Sprite bgSprite;

    public int curChapterIndex = 0; //전체 챕터로서의 순번 ex) 99999챕터까지 가능
    public int curStageIndex = 0; //전체 스테이지로서의 순번 ex) 99999스테이지 까지 가능

    public int curStageIndexInThisChapter = 0; //현재 챕터에서의 스테이지 인덱스정보값 ex) 챕터5의 10번째 스테이지
    public int MaxStageIndexInThisChapter = 0; //현 챕터의 스테이지 인덱스 최대치 ex) 챕터 5에는 최대 20스테이지가 있음.


    public Action OnStageChanged;
    public Action OnChapterChanged;

    private void Awake()
    {
        bgSprite = GetComponent<Sprite>();

        SetChapterList();
        SetStageList();

        OnStageChanged += RefreshStage;
        OnChapterChanged += RefreshChapter;
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
        for (int i = 0; i < chapterSOs[curChapterIndex].stageSOs.Length; i++)
        {
            stageSOs.Add(chapterSOs[curChapterIndex].stageSOs[i]);
        }
        MaxStageIndexInThisChapter = chapterSOs[curChapterIndex].stageSOs.Length - 1; //최대 스테이지 값 세팅. //Length는 1부터 시작. index는 0부터 시작이므로 -1.
    }

    public void StageClear()
    {
        Debug.LogWarning("StageClear");

        curStageIndex++;
        curStageIndexInThisChapter++;

        if (curStageIndexInThisChapter <= MaxStageIndexInThisChapter)
        {
            GoToNextStage();
        }
        else
        {
            curStageIndexInThisChapter = 0;
            GoToNextChapter();
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

    private void GoToNextStage()
    {
        RegenManager.Instance.RegenStagesEnemy();
        OnStageChanged?.Invoke();
    }

    private void GoToNextChapter()
    {
        curChapterIndex++;
        SetStageList();
        RefreshChapter();
        RefreshStage();

        RegenManager.Instance.RegenStagesEnemy();
    }
}
