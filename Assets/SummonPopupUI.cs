using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots 배열 33개 전부 연결하기
    public List<WeaponDataSO> weaponDataSOs;

    private List<SummonSlot> summonSlots;

    private void Awake()
    {
        summon = GetComponent<Summon>();
    }

    private void SetSummonSlots()
    {
        for(int i = 0; i < summonSlotObjs.Count; i++)
        {
            summonSlots[i] = summonSlotObjs[i].GetComponent<SummonSlot>();
            summonSlots[i].weaponDataSO = weaponDataSOs[i];
        }
    }

    public void OnExitBtn()
    {
        Hide();
    }

    public void OnClickSummonBtn(int summonCount) //acc, skill 추가할때 enum Type값도 추가해야겠다.
    {
        summon.SummonWeapon(summonCount);
    }

}
