using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_ShopUI : UIBase
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
                Debug.Log("����ĵ��������");
                break;
            case 4:
                Debug.Log("����ĵ��������");
                break;
            case 5:
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }
        Hide();
    }

    public void OnWeaponSummonBtnClick(int SummonCount)
    {
        SummonManager.Instance.SummonWeapon(SummonCount);
    }

    public void OnAccSummonBtnClick(int SummonCount)
    {
        SummonManager.Instance.SummonAccessary(SummonCount);
    }

    public void OnSkillCardSummonBtnClick(int SummonCount)
    {
        SummonManager.Instance.SummonSkillCard(SummonCount);
    }
}
