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

    public int curChapterIndex = 0;
    public int curStageIndex = 0;

    public Action onStageChanged;
    public Action onChapterChanged;

    private void Awake()
    {
        bgSprite = GetComponent<Sprite>();

        SetChapterList();
        SetStageList();

        onStageChanged += RefreshStage;
        onChapterChanged += RefreshChapter;
        RegenManager.Instance.OnEnemyCountZero += StageClear;
    }

    private void SetChapterList()
    {
        // Resources.LoadAll로 로드된 배열

        foreach (var chapter in Resources.LoadAll<ChapterSO>("Chapters")) //최초에 한번만 LoadAll 하더라도 리소스 측면에서 좋지않다.
        {
            chapterSOs.Add(chapter);
        }
    }

    private void SetStageList()
    {
        //해당 챕터에 해당하는 스테이지 리스트를 쫙 뽑아서 받아온다.
        for (int i = 0; i < chapterSOs[curChapterIndex].stageSOs.Length; i++)
        {
            stageSOs.Add(chapterSOs[curChapterIndex].stageSOs[i]);
        }
    }

    public void StageClear()
    {
        curStageIndex++;
        onStageChanged.Invoke();
    }

    public void RefreshChapter()
    {
        bgSprite = chapterSOs[curChapterIndex].bgSprite;
        chapterName = chapterSOs[curChapterIndex].chapterName;
    }

    public void RefreshStage()
    {
        stageName = stageSOs[curStageIndex].stageName;
    }
}
