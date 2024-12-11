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
        StageManager.Instance.onStageChanged += RefreshUI;
        RegenManager.Instance.OnEnemyCountDown += RefreshUI;
        RegenManager.Instance.OnEnemyCountZero += RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
        StageManager.Instance.onChapterChanged.Invoke();
        StageManager.Instance.onStageChanged.Invoke();
    }

    private void RefreshUI()
    {
        stageTxt.text = StageManager.Instance.stageName;
        stageNumTxt.text = StageManager.Instance.curStageIndex + 1.ToString();
        stageProgressSlider.value = RegenManager.Instance.curEnemyCount / RegenManager.Instance.stagesEnemyCount;
    }
}
