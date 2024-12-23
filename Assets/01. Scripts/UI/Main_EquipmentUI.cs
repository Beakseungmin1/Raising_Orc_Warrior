using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_EquipmentUI : UIBase
{
    public void ShowEquipmentUpgradePopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<EquipmentUpgradePopupUI>();
    }
}