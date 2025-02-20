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
    public GameObject redDot;

    private void OnEnable()
    {
        GameEventsManager.Instance.stageEvents.onStageChange += RefreshUI;
        RegenManager.Instance.OnEnemyCountDown += RefreshUI;
        RegenManager.Instance.OnEnemyCountZero += RefreshUI;
        GameEventsManager.Instance.currencyEvents.onGoldChanged += RefreshUI;

        if (!StageManager.Instance.isThisBossStageFirstTry) //두번째이상 도전이라면 redDot true
        {
            redDot.SetActive(true);
        }
        else
        {
            redDot.SetActive(false);
        }
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.stageEvents.onStageChange -= RefreshUI;
        RegenManager.Instance.OnEnemyCountDown -= RefreshUI;
        RegenManager.Instance.OnEnemyCountZero -= RefreshUI;
        GameEventsManager.Instance.currencyEvents.onGoldChanged -= RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
        StageManager.Instance.OnChapterChanged?.Invoke();
        GameEventsManager.Instance.stageEvents.ChangeStage();
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
        currentGoldTxt.text = CurrencyManager.Instance.GetGold().ToString("N0");
    }

    public void OnClickBossStageBtn()
    {
        StageManager.Instance.GoToBossStage();
    }
}
