using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EXPDungeonEnterSlotUI : UIBase
{
    public DungeonInfoSO dungeonInfoSO;

    public TextMeshProUGUI dungeonLabel;

    public Dungeon dungeon;

    private void Awake()
    {
        if (dungeonInfoSO != null)
        {
            dungeonLabel.text = $"{dungeonInfoSO.level}±¸¿ª";
        }

        dungeon = DungeonManager.Instance.GetDungeonByTypeAndLevel(dungeonInfoSO.type, dungeonInfoSO.level);
        if(dungeon.state == DungeonState.CLOSED)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void ShowEXPDungeonUI_ConfirmEnterBtnPopUpUI()
    {
        EXPDungeonUI_ConfirmEnterBtnPopUpUI btnPopupUI = UIManager.Instance.Show<EXPDungeonUI_ConfirmEnterBtnPopUpUI>(dungeonInfoSO);
        btnPopupUI.dungeonInfoSO = null;
        btnPopupUI.dungeonInfoSO = this.dungeonInfoSO;
        btnPopupUI.Init();
    }
}
