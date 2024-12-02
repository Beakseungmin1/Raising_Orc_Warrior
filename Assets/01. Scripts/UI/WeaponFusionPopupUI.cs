using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFusionPopupUI : MonoBehaviour
{
    public void ShowWeaponUpgradePopupUI()
    {
        UIManager.Instance.Hide("WeaponFusionPopupUI");
        UIManager.Instance.Show("WeaponUpgradePopupUI");
    }

    public void ExitBtn()
    {
        UIManager.Instance.Hide("DimmedImage");
        UIManager.Instance.Hide("WeaponFusionPopupUI");
    }
}
