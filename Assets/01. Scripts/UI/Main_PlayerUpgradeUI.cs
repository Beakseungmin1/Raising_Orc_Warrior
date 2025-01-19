using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_PlayerUpgradeUI : UIBase
{
    private PlayerStat stat;
    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;
    }

    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show<PlayerInfoPopupUI>();
        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);
        stat.UpdateUserInformationUI?.Invoke();
    }
}