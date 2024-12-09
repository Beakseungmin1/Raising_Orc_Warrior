using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonPopupUI : UIBase
{
    private Summon summon;

    public List<GameObject> summonSlotObjs; //SummonSlots 배열 33개 전부 연결하기
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
        summonSlotObjs.Clear(); //한번 클리어해주고 시작.
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

    public void OnClickSummonBtn(int summonCount) //acc, skill 추가할때 enum Type값도 추가해야겠다.
    {
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeaponDataSOList(summonCount); //웨폰데이터 리스트가 세팅된다.
        SetSlotAsCount(summonCount);
        StartSetWeaponDataSOs(weaponDataSOs); //그 생성된 웨폰데이터를 바탕으로 웨폰데이터를 세팅해준다.
    }

}
