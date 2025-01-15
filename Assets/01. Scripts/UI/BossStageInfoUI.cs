using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class BossStageInfoUI : UIBase
{
    public Slider timeSlider;
    public Slider hpSlider;
    public TextMeshProUGUI remainingCountTxt;
    public TextMeshProUGUI dungeonNameLabel;

    private BigInteger bossMaxHP;

    public float maxLimitTime;

    private void OnEnable()
    {
        GameEventsManager.Instance.bossEvents.onSetBossHp += InitUI;
        GameEventsManager.Instance.bossEvents.onBossHpChanged += RefreshUI;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.bossEvents.onSetBossHp -= InitUI;
        GameEventsManager.Instance.bossEvents.onBossHpChanged -= RefreshUI;
    }

    private void Awake()
    {
        //remainingCountTxt.gameObject.SetActive(false);
    }

    private void Start()
    {
        maxLimitTime = StageManager.Instance.timer.GetMaxTime();
    }

    private void FixedUpdate()
    {
        timeSlider.value = StageManager.Instance.timer.GetLimitTime() / maxLimitTime;
    }

    private void InitUI(BigInteger maxHP)
    {
        bossMaxHP = maxHP;
        hpSlider.value = (float)bossMaxHP/(float)bossMaxHP;
        remainingCountTxt.text = $"{bossMaxHP}/{bossMaxHP}";

        if (DungeonManager.Instance.currentDungeonInfo != null)
        {
            switch (DungeonManager.Instance.currentDungeonInfo.type)
            {
                case DungeonType.GoldDungeon:
                    dungeonNameLabel.text = $"폐허가된 광산 {DungeonManager.Instance.currentDungeonInfo.level}단계";
                    break;
                case DungeonType.CubeDungeon:
                    dungeonNameLabel.text = $"큐브 던전 {DungeonManager.Instance.currentDungeonInfo.level}단계";
                    break;
                case DungeonType.EXPDungeon:
                    dungeonNameLabel.text = $"경험의 숲 {DungeonManager.Instance.currentDungeonInfo.level}단계";
                    break;
            }
        }
        else
        {
            dungeonNameLabel.text = StageManager.Instance.stageName;
        }
    }

    private void RefreshUI(BigInteger curHP)
    {
        hpSlider.value = (float)curHP / (float)bossMaxHP;
        remainingCountTxt.text = $"{curHP}/{bossMaxHP}";
    }

    public void OnRunBtnClick()
    {
        StageManager.Instance.BackToLastStage();

        if (DungeonManager.Instance.playerIsInDungeon == true)
        {
            SetCamera camera = Camera.main.gameObject.GetComponent<SetCamera>();
            camera.SetCameraPosY(0f);
            camera.SetCameraSize(5f);
        }
    }
}
