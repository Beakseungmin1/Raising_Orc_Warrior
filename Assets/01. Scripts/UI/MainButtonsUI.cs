using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonsUI : UIBase
{
    private void OnEnable()
    {
        UIBase ui = this;
        //ui.canvas.sortingOrder = 7;
    }

    public void ShowMainUI(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.Instance.Hide<Main_SkillUI>();
                UIManager.Instance.Hide<Main_EquipmentUI>();
                UIManager.Instance.Hide<Main_DungeonUI>();
                UIManager.Instance.Hide<Main_ShopUI>();
                UIManager.Instance.Show<Main_PlayerUpgradeUI>();
                break;
            case 1:
                UIManager.Instance.Hide<Main_PlayerUpgradeUI>();
                UIManager.Instance.Hide<Main_EquipmentUI>();
                UIManager.Instance.Hide<Main_DungeonUI>();
                UIManager.Instance.Hide<Main_ShopUI>();
                UIManager.Instance.Show<Main_SkillUI>();
                break;
            case 2:
                UIManager.Instance.Hide<Main_PlayerUpgradeUI>();
                UIManager.Instance.Hide<Main_SkillUI>();
                UIManager.Instance.Hide<Main_DungeonUI>();
                UIManager.Instance.Hide<Main_ShopUI>();
                UIManager.Instance.Show<Main_EquipmentUI>();
                break;
            case 3:
                if(!DungeonManager.Instance.playerIsInDungeon)
                {
                    UIManager.Instance.Hide<Main_PlayerUpgradeUI>();
                    UIManager.Instance.Hide<Main_SkillUI>();
                    UIManager.Instance.Hide<Main_EquipmentUI>();
                    UIManager.Instance.Hide<Main_ShopUI>();
                    UIManager.Instance.Show<Main_DungeonUI>();
                }
                break;
            case 4:
                UIManager.Instance.Hide<Main_PlayerUpgradeUI>();
                UIManager.Instance.Hide<Main_SkillUI>();
                UIManager.Instance.Hide<Main_EquipmentUI>();
                UIManager.Instance.Hide<Main_DungeonUI>();
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }
    }
}
