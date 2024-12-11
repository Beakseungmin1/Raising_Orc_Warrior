using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("ChapterSO")]
    public List<ChapterSO> chapterSOs;

    [Header("StageSO")]
    public List<StageSO> stageSOs; //이 스테이지의 스테이지SO

    [Header("Info")]
    public string chapterName;
    public string stageName;
    [SerializeField] private Sprite bgSprite;

    public int curChapterNum = 0;
    public int curStageNum = 0;

    public Action onStageChanged; 

    private void Awake()
    {
        SetChapterList();
        SetStageList();
        chapterName = chapterSOs[curChapterNum].name;
        stageName = stageSOs[curStageNum].name;

        bgSprite = GetComponent<Sprite>();
        bgSprite = chapterSOs[curChapterNum].bgSprite;
    }

    private void SetChapterList()
    {
        foreach (var chapter in Resources.LoadAll<ChapterSO>("Chapters"))
        {
            chapterSOs.Add(chapter);
        }
    }

    private void SetStageList()
    {
        //해당 챕터에 해당하는 스테이지 리스트를 쫙 뽑아서 받아온다.
        for (int i = 0; i < chapterSOs[curChapterNum].stageSOs.Length; i++)
        {
            stageSOs.Add(chapterSOs[curChapterNum].stageSOs[i]);
        }
    }

    public void StageClear()
    {
        AddStageCount();
    }

    public void AddStageCount()
    {
        curStageNum++;
    }

    public void StageChange()
    {
        SetStageList();
        SetChapterList();
        onStageChanged?.Invoke();
    }
}
