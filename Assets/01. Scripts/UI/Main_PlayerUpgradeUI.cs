using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_PlayerUpgradeUI : UIBase
{
    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<PlayerInfoPopupUI>();
    }
}
