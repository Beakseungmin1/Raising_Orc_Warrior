using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoUI : UIBase
{
    public TextMeshProUGUI stageTxt;
    public TextMeshProUGUI stageNumTxt;
    //public Slider stageProgressSlider;

    private void Awake()
    {
        StageManager.Instance.onStageChanged += RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        stageTxt.text = StageManager.Instance.stageName;
        stageNumTxt.text = StageManager.Instance.curStageNumIndex.ToString();
    }
}
