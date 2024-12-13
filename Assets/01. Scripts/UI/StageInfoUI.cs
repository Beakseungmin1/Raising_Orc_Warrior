using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : UIBase
{
    public TextMeshProUGUI stageTxt;
    public TextMeshProUGUI stageNumTxt;
    public Slider stageProgressSlider;

    private void Awake()
    {
        StageManager.Instance.OnStageChanged += RefreshUI;
        RegenManager.Instance.OnEnemyCountDown += RefreshUI;
        RegenManager.Instance.OnEnemyCountZero += RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
        StageManager.Instance.OnChapterChanged?.Invoke();
        StageManager.Instance.OnStageChanged?.Invoke();
    }

    private void RefreshUI()
    {
        stageTxt.text = StageManager.Instance.stageName;
        stageNumTxt.text = $"STAGE {StageManager.Instance.curStageIndex + 1}";
        stageProgressSlider.value = (float)RegenManager.Instance.killedEnemies / RegenManager.Instance.totalEnemies;
    }
}
