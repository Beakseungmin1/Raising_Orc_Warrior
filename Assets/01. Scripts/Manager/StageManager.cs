using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("ChapterSO")]
    [SerializeField] private List<ChapterSO> chapterSOs;

    [Header("StageSO")]
    [SerializeField] private List<StageSO> stageSOs;

    [Header("Info")]
    [SerializeField] private int curChapterNum = 1;
    [SerializeField] private int curStageNum = 1;
    [SerializeField] private string chapterName;
    [SerializeField] private string stageName;
    [SerializeField] private Sprite bgSprite;

    private void Awake()
    {
        chapterName = chapterSOs[curChapterNum].name;
        stageName = stageSOs[curStageNum].name;

        bgSprite = GetComponent<Sprite>();
        bgSprite = chapterSOs[curChapterNum].bgSprite;
    }

    private void NextStage()
    {
        curStageNum++;
    }
}
