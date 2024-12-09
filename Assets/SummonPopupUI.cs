using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots �迭 33�� ���� �����ϱ�
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

    public void OnClickSummonBtn(int summonCount) //acc, skill �߰��Ҷ� enum Type���� �߰��ؾ߰ڴ�.
    {
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeapon(summonCount); //���������� ����Ʈ�� ���õȴ�.
        StartSetWeaponDataSOs(weaponDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.
    }

}
