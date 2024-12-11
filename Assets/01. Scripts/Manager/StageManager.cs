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
    }

    private void SetChapterList()
    {
        // Resources.LoadAll로 로드된 배열
        var loadedChapters = Resources.LoadAll<ChapterSO>("Chapters");

        foreach (var chapter in loadedChapters)
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
        AddStageCount();
    }

    public void AddStageCount()
    {
        curStageIndex++;
    }

    public void RefreshChapter()
    {
        bgSprite = chapterSOs[curChapterIndex].bgSprite;
        chapterName = chapterSOs[curChapterIndex].name;
    }

    public void RefreshStage()
    {
        stageName = stageSOs[curStageIndex].name;
    }
}
