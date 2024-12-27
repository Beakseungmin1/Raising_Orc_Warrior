using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EXPDungeonEnterBtnUI : UIBase
{
    public DungeonInfoSO dungeonInfoSO;

    public TextMeshProUGUI dungeonLabel;

    private void Awake()
    {
        if (dungeonInfoSO != null)
        {
            dungeonLabel.text = $"{dungeonInfoSO.level}±¸¿ª";
        }
    }
    public void ShowEXPDungeonUI_ConfirmEnterBtnPopUpUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        EXPDungeonUI_ConfirmEnterBtnPopUpUI btnPopupUI = UIManager.Instance.Show<EXPDungeonUI_ConfirmEnterBtnPopUpUI>(dungeonInfoSO);
        btnPopupUI.dungeonInfoSO = null;
        btnPopupUI.dungeonInfoSO = this.dungeonInfoSO;
        btnPopupUI.Init();
    }
}
