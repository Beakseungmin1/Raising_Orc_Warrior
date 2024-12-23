using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonsUI : UIBase
{
    public void ShowMainUI(int index)
    {
        UIManager.Instance.Hide<Main_PlayerUpgradeUI>();
        UIManager.Instance.Hide<Main_SkillUI>();
        UIManager.Instance.Hide<Main_EquipmentUI>();
        UIManager.Instance.Hide<Main_DungeonUI>();
        UIManager.Instance.Hide<Main_ShopUI>();

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
                UIManager.Instance.Show<Main_DungeonUI>();
                break;
            case 4:
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }
    }
}
