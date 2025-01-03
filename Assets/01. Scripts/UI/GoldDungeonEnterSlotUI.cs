using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldDungeonEnterSlotUI : UIBase
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
        if (dungeon.state == DungeonState.CLOSED)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void ShowGoldDungeonUI_ConfirmEnterBtnPopUpUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        GoldDungeonUI_ConfirmEnterBtnPopUpUI btnPopupUI = UIManager.Instance.Show<GoldDungeonUI_ConfirmEnterBtnPopUpUI>(dungeonInfoSO);
        btnPopupUI.dungeonInfoSO = null;
        btnPopupUI.dungeonInfoSO = this.dungeonInfoSO;
        btnPopupUI.Init();
    }
}
