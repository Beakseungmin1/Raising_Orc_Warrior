using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradePopupUI : UIBase
{
    public void ShowWeaponFusionPopupUI()
    {
        Hide();
        UIManager.Instance.Show<WeaponFusionPopupUI>();
    }

    public void ExitBtn()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}
