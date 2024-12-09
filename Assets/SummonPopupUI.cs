using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots 배열 33개 전부 연결하기
    public List<WeaponDataSO> weaponDataSOs;

    private Coroutine coroutine;

    private void Start()
    {
        summon = GetComponent<Summon>();
    }

    public void StartSetWeaponDataSOs(List<WeaponDataSO> SOs)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SetWeaponDataSOs(SOs));
    }

    private IEnumerator SetWeaponDataSOs(List<WeaponDataSO> SOs)
    {
        weaponDataSOs = SOs;
        for (int i = 0; i < summonSlotObjs.Count; i++)
        {
            if (weaponDataSOs[i] != null)
            {
                summonSlotObjs[i].GetComponent<SummonSlot>().SetSlot(weaponDataSOs[i]);
                yield return new WaitForSeconds(0.05f);
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

    public void OnClickSummonBtn(int summonCount) //acc, skill 추가할때 enum Type값도 추가해야겠다.
    {
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeapon(summonCount); //웨폰데이터 리스트가 세팅된다.
        StartSetWeaponDataSOs(weaponDataSOs); //그 생성된 웨폰데이터를 바탕으로 웨폰데이터를 세팅해준다.
    }

}
