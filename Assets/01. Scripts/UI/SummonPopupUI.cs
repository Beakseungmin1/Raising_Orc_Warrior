using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonPopupUI : UIBase
{
    public ItemType curSummoningItemType; //현재 소환중인 아이템 타입

    private Summon summon;

    public List<GameObject> summonSlotObjs;

    public GameObject summonSlotListArea33obj;
    public GameObject summonSlotListArea11obj;
    public GameObject summonSlotListArea1obj;

    private Dictionary<int, GameObject> summonSlotMapping;
    private Coroutine coroutine;

    public Button extBtn;
    public Button summonBtn11;
    public Button summonBtn33;

    // 데이터 타입별 리스트 딕셔너리
    private Dictionary<System.Type, IList> summonDataMapping;

    private void Awake()
    {
        summon = GetComponent<Summon>();

        // Dictionary 초기화
        summonSlotMapping = new Dictionary<int, GameObject>
        {
            { 1, summonSlotListArea1obj },
            { 11, summonSlotListArea11obj },
            { 33, summonSlotListArea33obj }
        };

        // 데이터 매핑 딕셔너리 초기화
        summonDataMapping = new Dictionary<System.Type, IList>();
    }

    public void SetSlotAsCount(int count)
    {
        foreach (var slotObj in summonSlotMapping.Values)
        {
            slotObj.SetActive(false);
        }

        if (summonSlotMapping.TryGetValue(count, out var selectedSlotObj))
        {
            selectedSlotObj.SetActive(true);
            summonSlotObjs = selectedSlotObj.GetComponent<SummonSlotListArea>().summonSlots;
        }
        else
        {
            Debug.LogWarning($"No slot found for count: {count}");
        }
    }

    public void ClearSlotData()
    {
        foreach (var slotObj in summonSlotObjs)
        {
            slotObj.GetComponent<SummonSlot>().ClearSlot();
        }
    }

    public void StartSetDataSOs<T>(List<T> dataSOs) where T : BaseItemDataSO
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SetDataSOs(dataSOs));
    }

    private IEnumerator SetDataSOs<T>(List<T> dataSOs) where T : BaseItemDataSO
    {
        SetBtnInteractable(false);

        summonDataMapping[typeof(T)] = dataSOs; // 데이터 매핑
        for (int i = 0; i < summonSlotObjs.Count; i++)
        {
            if (i < dataSOs.Count && dataSOs[i] != null)
            {
                summonSlotObjs[i].GetComponent<SummonSlot>().SetSlot(dataSOs[i]);
                yield return new WaitForSeconds(0.05f);
            }
        }

        SetBtnInteractable(true); // 코루틴 진행 동안 버튼 비활성화
    }

    private void SetBtnInteractable(bool canInteractable)
    {
        extBtn.interactable = canInteractable;
        summonBtn11.interactable = canInteractable;
        summonBtn33.interactable = canInteractable;
    }

    public void OnExitBtn()
    {
        Hide();
    }

    public void OnClickMoreBtn(int summonCount)
    {
        switch (curSummoningItemType)
        {
            case(ItemType.Weapon):
                OnWeaponSummon(summonCount);
                break;
            case(ItemType.Accessory):
                OnAccSummon(summonCount);
                break;
            case (ItemType.Skill):
                OnSkillCardSummon(summonCount);
                break;
        }
    }

    public void OnWeaponSummon(int summonCount)
    {
        // 무기 소환 예제
        var weaponDataSOs = summon.SummonWeaponDataSOList(summonCount);
        SetSlotAsCount(summonCount);
        ClearSlotData();
        StartSetDataSOs(weaponDataSOs); // 제너릭 메서드 호출
        SummonDataManager.Instance.AddExperience(ItemType.Weapon, summonCount);
    }

    public void OnAccSummon(int summonCount)
    {
        // 악세서리 소환 예제
        var accessoryDataSOs = summon.SummonAccessoryDataSOList(summonCount);
        SetSlotAsCount(summonCount);
        ClearSlotData();
        StartSetDataSOs(accessoryDataSOs); // 제너릭 메서드 호출
        SummonDataManager.Instance.AddExperience(ItemType.Accessory, summonCount);
    }

    public void OnSkillCardSummon(int summonCount)
    {
        // 스킬 소환 예제
        var skillDataSOs = summon.SummonSkillDataSOList(summonCount);
        SetSlotAsCount(summonCount);
        ClearSlotData();
        StartSetDataSOs(skillDataSOs); // 제너릭 메서드 호출
        SummonDataManager.Instance.AddExperience(ItemType.Skill, summonCount);
    }
}