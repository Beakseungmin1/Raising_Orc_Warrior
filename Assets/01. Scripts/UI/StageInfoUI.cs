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
    public TextMeshProUGUI currentGoldTxt;

    private void Awake()
    {
        StageManager.Instance.OnStageChanged += RefreshUI;
        RegenManager.Instance.OnEnemyCountDown += RefreshUI;
        RegenManager.Instance.OnEnemyCountZero += RefreshUI;
        GameEventsManager.Instance.currencyEvents.onGoldChanged += RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
        StageManager.Instance.OnChapterChanged?.Invoke();
        StageManager.Instance.OnStageChanged?.Invoke();
    }

    public string GetGoldAmountAsString()
    {
        string goldAmount = BigIntegerManager.Instance.FormatBigInteger(CurrencyManager.Instance.GetGold());
        return goldAmount.ToString();
    }

    private void RefreshUI()
    {
        stageTxt.text = StageManager.Instance.stageName;
        stageNumTxt.text = $"STAGE {StageManager.Instance.curStageIndex + 1}";
        stageProgressSlider.value = (float)RegenManager.Instance.killedEnemies / RegenManager.Instance.totalEnemies;
        currentGoldTxt.text = CurrencyManager.Instance.GetGold().ToString();
    }
}
