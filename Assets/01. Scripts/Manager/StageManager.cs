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


    public Action OnStageChanged;
    public Action OnChapterChanged;

    private void Awake()
    {
        bgSprite = GetComponent<Sprite>();

        SetChapterList();
        SetStageList();
        SetBossStage();

        OnStageChanged += RefreshStage;
        OnChapterChanged += RefreshChapter;
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
        for (int i = 0; i < chapterSOs[curChapterIndex].stageSOs.Length; i++)
        {
            stageSOs.Add(chapterSOs[curChapterIndex].stageSOs[i]);
        }
        MaxStageIndexInThisChapter = chapterSOs[curChapterIndex].stageSOs.Length - 1; //�ִ� �������� �� ����. //Length�� 1���� ����. index�� 0���� �����̹Ƿ� -1.
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
            GoToNextStage();
        }
        else //���簡 é���� ������ ����������� �ش� �������� �ݺ�
        {
            int savedCurStageIndexInThisChapter = curStageIndexInThisChapter;

            curStageIndexInThisChapter = savedCurStageIndexInThisChapter;
            GoToNextStage();
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

    public void GoToBossStage()
    {
        UIManager.Instance.Hide<StageInfoUI>();
        curStageIndex++;
        RegenManager.Instance.RegenBossStagesEnemy();
        OnStageChanged?.Invoke();
    }

    public void GoToNextChapter()
    {
        UIManager.Instance.Show<StageInfoUI>();
        curChapterIndex++;
        curStageIndexInThisChapter = 0;
        SetStageList();
        SetBossStage();
        RefreshChapter();
        RefreshStage();
        RegenManager.Instance.RegenStagesEnemy();
        OnStageChanged?.Invoke(); //���� �ִ����, ���� ���� �� ���� �����ؾ��ϹǷ�, RegenStagesEnemy()������ ����.
    }
}
