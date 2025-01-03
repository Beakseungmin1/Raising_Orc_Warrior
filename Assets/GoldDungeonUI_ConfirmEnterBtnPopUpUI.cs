using UnityEngine;
using TMPro;

public class GoldDungeonUI_ConfirmEnterBtnPopUpUI : UIBase
{
    public DungeonInfoSO dungeonInfoSO;

    public TextMeshProUGUI curLevel;
    public TextMeshProUGUI dungeonTicketCount;

    public void Init()
    {
        curLevel.text = $"{dungeonInfoSO.level}±¸¿ª";
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
            UIManager.Instance.Hide<EXPDungeonUI>();
            UIManager.Instance.Hide<StageInfoUI>();
            UIManager.Instance.Hide<DimmedUI>();
            UIManager.Instance.Show<Main_PlayerUpgradeUI>();
            UIManager.Instance.Show<BossStageInfoUI>();
            CurrencyManager.Instance.SubtractCurrency(CurrencyType.DungeonTicket, 1);
            StageManager.Instance.GoToDungeonStage(dungeonInfoSO.type, dungeonInfoSO.level);
        }
    }
}
