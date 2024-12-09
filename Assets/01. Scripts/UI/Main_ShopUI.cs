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

    public void OnWeaponSummonBtnClick(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeapon(summonCount); //���������� ����Ʈ�� ���õȴ�.
        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.StartSetWeaponDataSOs(weaponDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.
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
