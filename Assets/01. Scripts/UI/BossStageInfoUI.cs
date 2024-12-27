using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class BossStageInfoUI : UIBase
{
    public List<GameObject> dungeonBosses;

    public Slider timeSlider;
    public Slider hpSlider;
    public TextMeshProUGUI remainingCountTxt;
    public Button runBtn;
    public TextMeshProUGUI dungeonNameLabel;

    private BigInteger bossCurHP;
    private BigInteger bossMaxHP;

    private void OnEnable()
    {
        GameEventsManager.Instance.bossEvents.onSetBossHp += SetMaxHP;
        GameEventsManager.Instance.bossEvents.onBossHpChanged += RefreshUI;
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.bossEvents.onSetBossHp -= SetMaxHP;
        GameEventsManager.Instance.bossEvents.onBossHpChanged -= RefreshUI;
    }

    private void Awake()
    {
        remainingCountTxt.gameObject.SetActive(false);
        InitUI();
    }

    private void InitUI()
    {
        //hpSlider.value = (float)bossCurHP/(float)bossMaxHP;
        //remainingCountTxt.text = $"{bossCurHP}/{bossMaxHP}";

        switch (DungeonManager.Instance.currentDungeonInfo.type)
        {
            case DungeonType.GoldDungeon:
                dungeonNameLabel.text = $"���㰡�� ���� {DungeonManager.Instance.currentDungeonInfo.level}�ܰ�";
                break;
            case DungeonType.CubeDungeon:
                dungeonNameLabel.text = $"ť�� ���� {DungeonManager.Instance.currentDungeonInfo.level}�ܰ�";
                break;
            case DungeonType.EXPDungeon:
                dungeonNameLabel.text = $"������ �� {DungeonManager.Instance.currentDungeonInfo.level}�ܰ�";
                break;
        }
    }

    private void SetMaxHP(BigInteger maxHP)
    {
        bossMaxHP = maxHP;
        bossCurHP = maxHP;
    }

    private void RefreshUI(BigInteger curHP)
    {
        hpSlider.value = (float)curHP / (float)bossMaxHP;
        //remainingCountTxt.text = $"{curHP}/{bossMaxHP}";
    }
}
