using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots �迭 33�� ���� �����ϱ�
    public List<WeaponDataSO> weaponDataSOs;


    private void Start()
    {
        summon = GetComponent<Summon>();
    }

    public void SetWeaponDataSOs(List<WeaponDataSO> SOs)
    {
        Debug.LogWarning("SetWeaponDataSOs");
        weaponDataSOs = SOs;
        for (int i = 0; i < summonSlotObjs.Count; i++)
        {
            if (weaponDataSOs[i] != null)
            {
                summonSlotObjs[i].GetComponent<SummonSlot>().SetSlot(weaponDataSOs[i]);
            }
        }
    }

    private void SetSummonSlots()
    {
        Debug.Log("SetSummonSlots");
    }

    public void OnExitBtn()
    {
        Hide();
    }

    public void OnClickSummonBtn(int summonCount) //acc, skill �߰��Ҷ� enum Type���� �߰��ؾ߰ڴ�.
    {
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeapon(summonCount);
        SetWeaponDataSOs(weaponDataSOs);
    }

}
