using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Main_ShopUI : UIBase
{
    private Summon summon;

    private Dictionary<ItemType, int> summonTypeMapping;

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

    public void OnWeaponSummon(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = new List<WeaponDataSO>();
        weaponDataSOs = summon.SummonWeaponDataSOList(summonCount); //웨폰데이터 리스트가 세팅된다. //OK

        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.SetSlotAsCount(summonCount);
        summonPopupUI.ClearSlotData();
        summonPopupUI.StartSetDataSOs(weaponDataSOs); //그 생성된 웨폰데이터를 바탕으로 웨폰데이터를 세팅해준다.
    }

    public void OnAccSummon(int summonCount)
    {
        List<AccessoryDataSO> accessoryDataSOs = new List<AccessoryDataSO>();
        accessoryDataSOs = summon.SummonAccessoryDataSOList(summonCount);

        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.SetSlotAsCount(summonCount);
        summonPopupUI.ClearSlotData();
        summonPopupUI.StartSetDataSOs(accessoryDataSOs); //그 생성된 웨폰데이터를 바탕으로 웨폰데이터를 세팅해준다.
    }

    public void OnSkillCardSummon(int summonCount)
    {
        List<SkillDataSO> skillDataSOs = new List<SkillDataSO>();
        skillDataSOs = summon.SummonSkillDataSOList(summonCount);

        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.SetSlotAsCount(summonCount);
        summonPopupUI.ClearSlotData();
        summonPopupUI.StartSetDataSOs(skillDataSOs); //그 생성된 웨폰데이터를 바탕으로 웨폰데이터를 세팅해준다.
    }
}
