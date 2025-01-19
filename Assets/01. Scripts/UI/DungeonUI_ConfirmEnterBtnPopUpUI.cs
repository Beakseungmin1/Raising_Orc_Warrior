using System.Collections;
using TMPro;
using UnityEngine;

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
    }

    public void OnGoToDungeonBtnClick()
    {
        if (CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket) >= 1)
        {
            string uiName = dungeonInfoSO.type.ToString() + "UI";

            Hide();
            UIManager.Instance.Hide(uiName);
            UIManager.Instance.Hide<StageInfoUI>();
            UIManager.Instance.Show<Main_PlayerUpgradeUI>();
            UIManager.Instance.Show<BossStageInfoUI>();

            GameEventsManager.Instance.dungeonEvents.DungeonUIChanged();
            CurrencyManager.Instance.SubtractCurrency(CurrencyType.DungeonTicket, 1);
            GameEventsManager.Instance.currencyEvents.DungeonTicketChanged();
            StageManager.Instance.GoToDungeonStage(dungeonInfoSO.type, dungeonInfoSO.level);
        }
        else
        {
            GameEventsManager.Instance.messageEvents.ShowMessage(MessageTextType.DungeonTicketNotEnough, 0.4f, 145);
        }
    }
}