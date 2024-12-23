using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_EquipmentUI : UIBase
{
    public void ShowMainUI(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.Instance.Show<Main_PlayerUpgradeUI>();
                break;
            case 1:
                UIManager.Instance.Show<Main_SkillUI>();
                break;
            case 2:
                UIManager.Instance.Show<Main_EquipmentUI>();
                break;
            case 3:
                Debug.Log("동료캔버스열기");
                UIManager.Instance.Show<Main_EquipmentUI>();
                break;
            case 4:
                UIManager.Instance.Show<Main_DungeonUI>();
                break;
            case 5:
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }
        Hide();
    }

    public void ShowEquipmentUpgradePopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<EquipmentUpgradePopupUI>();
    }
}