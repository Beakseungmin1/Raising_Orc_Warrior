using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradePopupUI : MonoBehaviour
{
    public void ShowWeaponFusionPopupUI()
    {
        UIManager.Instance.Hide("WeaponUpgradePopupUI");
        UIManager.Instance.Show("WeaponFusionPopupUI");
    }

    public void ExitBtn()
    {
        UIManager.Instance.Hide("DimmedImage");
        UIManager.Instance.Hide("WeaponUpgradePopupUI");
    }
}
