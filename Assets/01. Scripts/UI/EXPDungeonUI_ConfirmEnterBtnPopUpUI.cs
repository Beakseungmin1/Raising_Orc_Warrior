using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPDungeonUI_ConfirmEnterBtnPopUpUI : UIBase
{
    public DungeonInfoSO dungeonInfoSO;

    public void ExitBtn()
    {
        Hide();
        UIManager.Instance.Hide<DimmedUI>();
    }

    public void GoToDungeonStage()
    {
        StageManager.Instance.GoToDungeonStage(dungeonInfoSO.type, dungeonInfoSO.level);
        Hide();
        UIManager.Instance.Hide<EXPDungeonUI>();
        UIManager.Instance.Show<Main_PlayerUpgradeUI>();
        UIManager.Instance.Hide<DimmedUI>();
    }
}
