using TMPro;

public class DungeonUI_ConfirmEnterBtnPopUpUI : UIBase
{
    public DungeonInfoSO dungeonInfoSO;

    public TextMeshProUGUI curLevel;
    public TextMeshProUGUI dungeonTicketCount;

    public void Init()
    {
        curLevel.text = $"{dungeonInfoSO.level}구역";
        dungeonTicketCount.text = $"{CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket)}/1";
    }

    public void ExitBtn()
    {
        Hide();
        UIManager.Instance.Hide<DimmedUI>();
    }

    public void OnGoToDungeonBtnClick()
    {
        if (CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket) >= 1)
        {
            Hide();

            string uiName = dungeonInfoSO.type.ToString() + "UI";
            UIManager.Instance.Hide(uiName);
            UIManager.Instance.Hide<StageInfoUI>();
            UIManager.Instance.Hide<DimmedUI>();
            UIManager.Instance.Show<Main_PlayerUpgradeUI>();
            UIManager.Instance.Show<BossStageInfoUI>();
 
            GameEventsManager.Instance.dungeonEvents.DungeonUIChanged();

            CurrencyManager.Instance.SubtractCurrency(CurrencyType.DungeonTicket, 1);
            StageManager.Instance.GoToDungeonStage(dungeonInfoSO.type, dungeonInfoSO.level);
        }
    }
}