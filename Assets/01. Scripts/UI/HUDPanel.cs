using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDPanel : UIBase
{
    private PlayerStat stat;

    private void Start()
    {
        stat = PlayerobjManager.Instance.Player.stat;
    }

    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<PlayerInfoPopupUI>();
        stat.UpdateUserInformationUI();
    }
}
