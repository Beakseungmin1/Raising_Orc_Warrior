using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots �迭 33�� ���� �����ϱ�
    public List<WeaponDataSO> weaponDataSOs;

    private Coroutine coroutine;

    public Button extBtn;
    public Button summonBtn11;
    public Button summonBtn33;

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
        SetBtnInteractable(false);
        weaponDataSOs = SOs;
        for (int i = 0; i < summonSlotObjs.Count; i++)
        {
            if (weaponDataSOs[i] != null)
            {
                summonSlotObjs[i].GetComponent<SummonSlot>().SetSlot(weaponDataSOs[i]);
                yield return new WaitForSeconds(0.05f);
            }
        }
        SetBtnInteractable(true);
    }

    private void SetBtnInteractable(bool canInteractable)
    {
        extBtn.interactable = canInteractable;
        summonBtn11.interactable = canInteractable;
        summonBtn33.interactable = canInteractable;
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
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeaponDataSOList(summonCount); //���������� ����Ʈ�� ���õȴ�.
        StartSetWeaponDataSOs(weaponDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.
    }

}
