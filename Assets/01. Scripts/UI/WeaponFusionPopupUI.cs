using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFusionPopupUI : UIBase
{
    public void ShowWeaponUpgradePopupUI()
    {
        Hide();
        UIManager.Instance.Show<WeaponUpgradePopupUI>();
    }

    public void ExitBtn()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}
