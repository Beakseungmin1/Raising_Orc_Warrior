using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots �迭 33�� ���� �����ϱ�
    public List<WeaponDataSO> weaponDataSOs;

    public GameObject summonSlotListArea33obj;
    public GameObject summonSlotListArea11obj;
    public GameObject summonSlotListArea1obj;

    private Coroutine coroutine;

    public Button extBtn;
    public Button summonBtn11;
    public Button summonBtn33;

    private void Start()
    {
        summon = GetComponent<Summon>();
    }

    public void SetSlotAsCount(int count)
    {
        summonSlotObjs.Clear(); //�ѹ� Ŭ�������ְ� ����.
        summonSlotListArea33obj.SetActive(false);
        summonSlotListArea11obj.SetActive(false);
        summonSlotListArea1obj.SetActive(false);
        switch (count)
        {
            case 1:
                summonSlotListArea1obj.SetActive(true);
                summonSlotObjs = summonSlotListArea1obj.GetComponent<SummonSlotListArea>().summonSlots;
                break;
            case 11:
                summonSlotListArea11obj.SetActive(true);
                summonSlotObjs = summonSlotListArea11obj.GetComponent<SummonSlotListArea>().summonSlots;
                break;
            default:
                summonSlotListArea33obj.SetActive(true);
                summonSlotObjs = summonSlotListArea33obj.GetComponent<SummonSlotListArea>().summonSlots;
                break;
        }
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
        SetSlotAsCount(summonCount);
        StartSetWeaponDataSOs(weaponDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.
    }

}
