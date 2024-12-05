using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [Header("ChapterSO")]
    public List<ChapterSO> chapterSOs;

    [Header("StageSO")]
    public List<StageSO> stageSOs; //�� ���������� ��������SO

    [Header("Info")]
    [SerializeField] private string chapterName;
    [SerializeField] private string stageName;
    [SerializeField] private Sprite bgSprite;

    public int curChapterNum = 0;
    public int curStageNum = 0;

    private void Awake()
    {
        SetUI();
        SetStageList();
        chapterName = chapterSOs[curChapterNum].name;
        stageName = stageSOs[curStageNum].name;

        bgSprite = GetComponent<Sprite>();
        bgSprite = chapterSOs[curChapterNum].bgSprite;
    }

    private void SetUI()
    {
        UIManager.Instance.Show<HUDPanel>();
        UIManager.Instance.Show<StageInfoUI>();
        UIManager.Instance.Show<Main_PlayerUpgradeUI>();
    }

    private void SetStageList()
    {
        //�ش� é�Ϳ� �ش��ϴ� �������� ����Ʈ�� �� �̾Ƽ� �޾ƿ´�.
        for (int i = 0; i < chapterSOs[curChapterNum].stageSOs.Length; i++)
        {
            stageSOs.Add(chapterSOs[curChapterNum].stageSOs[i]);
        }
    }

    public void NextStage()
    {
        curStageNum++;
    }
}
