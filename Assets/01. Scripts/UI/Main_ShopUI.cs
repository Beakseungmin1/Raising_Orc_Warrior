using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Main_ShopUI : UIBase
{
    private Summon summon;

    private void Awake()
    {
        summon = GetComponent<Summon>();
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

    public void OnWeaponSummonBtnClick(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeapon(summonCount); //웨폰데이터 리스트가 세팅된다.
        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.StartSetWeaponDataSOs(weaponDataSOs); //그 생성된 웨폰데이터를 바탕으로 웨폰데이터를 세팅해준다.
    }

    public void OnAccSummonBtnClick(int summonCount)
    {
        summon.SummonAccessary(summonCount);
    }

    public void OnSkillCardSummonBtnClick(int summonCount)
    {
        summon.SummonSkillCard(summonCount);
    }
}
