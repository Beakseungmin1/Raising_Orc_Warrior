using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_ShopUI : UIBase
{
    private Summon Summon;

    private void Awake()
    {
        Summon = GetComponent<Summon>();
    }

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
                break;
            case 4:
                Debug.Log("모험캔버스열기");
                break;
            case 5:
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }
        Hide();
    }

    public void OnWeaponSummonBtnClick(int SummonCount)
    {
        Summon.SummonWeapon(SummonCount);
    }

    public void OnAccSummonBtnClick(int SummonCount)
    {
        Summon.SummonAccessary(SummonCount);
    }

    public void OnSkillCardSummonBtnClick(int SummonCount)
    {
        Summon.SummonSkillCard(SummonCount);
    }
}
