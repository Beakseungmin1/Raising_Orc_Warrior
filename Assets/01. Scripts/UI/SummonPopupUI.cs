using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI diaAmountLabel11;
    public TextMeshProUGUI diaAmountLabel33;

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

    public void Start()
    {
        SetDiaAmountLabel();
    }

    private void SetDiaAmountLabel()
    {
        switch (curSummoningItemType)
        {
            case ItemType.Skill:
                diaAmountLabel11.text = 3000.ToString();
                diaAmountLabel33.text = 9000.ToString();
                break;
            default:
                diaAmountLabel11.text = 500.ToString();
                diaAmountLabel33.text = 1500.ToString();
                break;
        }
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

        float delayBetweenSlots = 0.05f; // 각 슬롯 시작 간격 (중첩 정도를 조정)
        Vector3 initialScale = Vector3.one * 12f; // 12배 크기
        Vector3 finalScale = Vector3.one; // 1배 크기
        Color transparentColor = new Color(1f, 1f, 1f, 0f); // 투명
        Color opaqueColor = new Color(1f, 1f, 1f, 1f); // 불투명

        for (int i = 0; i < summonSlotObjs.Count; i++)
        {
            if (i < dataSOs.Count && dataSOs[i] != null)
            {
                // 슬롯 데이터 설정
                summonSlotObjs[i].GetComponent<SummonSlot>().SetSlot(dataSOs[i]);

                // 슬롯별 코루틴을 시작하여 애니메이션 실행
                StartCoroutine(AnimateSlot(summonSlotObjs[i], initialScale, finalScale, transparentColor, opaqueColor));

                // 다음 슬롯 시작까지 대기 시간 (슬롯 간 중첩 정도 조정)
                yield return new WaitForSeconds(delayBetweenSlots);
            }
        }
        
        float summonAnimationDuration = 0.1f;
        // 모든 슬롯 애니메이션 완료 후 버튼 활성화
        yield return new WaitForSeconds(summonAnimationDuration); // 마지막 슬롯 애니메이션 대기
        SetBtnInteractable(true);
    }

    // 슬롯 하나에 대한 애니메이션 처리
    private IEnumerator AnimateSlot(GameObject slotObj, Vector3 initialScale, Vector3 finalScale, Color transparentColor, Color opaqueColor)
    {
        Image image = slotObj.GetComponent<Image>();
        Image whiteImage = slotObj.GetComponent<SummonSlot>().whiteImage;
        TextMeshProUGUI gradeTxt = slotObj.GetComponent<SummonSlot>().gradeTxt;
        TextMeshProUGUI rankTxt = slotObj.GetComponent<SummonSlot>().rankTxt;
        TextMeshProUGUI rankLabel = slotObj.GetComponent<SummonSlot>().rankLabel;

        // 1. 초기 상태 설정 (투명한 흰색, 12배 크기, image는 꺼져있음)
        whiteImage.rectTransform.localScale = initialScale;
        whiteImage.color = transparentColor;
        image.enabled = false;
        gradeTxt.enabled = false;
        rankTxt.enabled = false;
        rankLabel.enabled = false;

        float summonAnimationDuration = 0.2f;

        // 2. 애니메이션: 투명한 흰색(12배 크기) -> 불투명한 흰색(1배 크기)
        float elapsedTime = 0f;
        while (elapsedTime < summonAnimationDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / summonAnimationDuration;
            whiteImage.rectTransform.localScale = Vector3.Lerp(initialScale, finalScale, progress);
            whiteImage.color = Color.Lerp(transparentColor, opaqueColor, progress);

            yield return null;
        }

        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);

        // 최종 상태 보정 (불투명한 흰색, 1배 크기)
        whiteImage.rectTransform.localScale = finalScale;
        whiteImage.color = transparentColor;

        // 아이콘 이미지 켬
        image.enabled = true;
        gradeTxt.enabled = true;
        rankTxt.enabled = true;
        rankLabel.enabled = true;

        // 3. 애니메이션: 불투명한 흰색(1배 크기) -> 투명한 흰색(1배 크기)
        elapsedTime = 0f;
        while (elapsedTime < summonAnimationDuration)
        {
            yield return new WaitForSeconds(0.03f);

            elapsedTime += Time.deltaTime;

            // 진행률 (0~1)
            float progress = elapsedTime / summonAnimationDuration;

            // Ease 효과 (SmoothStep)
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            // 색상을 보간 (크기는 유지)
            whiteImage.color = Color.Lerp(opaqueColor, transparentColor, easedProgress);

            yield return null;
        }
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
        float price = 0;

        switch (summonCount)
        {
            case 1:
                price = 50f;
                break;
            case 11:
                price = 500f;
                break;
            default:
                price = 1500f;
                break;
        }

        if (CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond) >= price)
        {
            CurrencyManager.Instance.SubtractCurrency(CurrencyType.Diamond, price);

            // 무기 소환 예제
            var weaponDataSOs = summon.SummonWeaponDataSOList(summonCount);
            SetDiaAmountLabel();
            SetSlotAsCount(summonCount);
            ClearSlotData();
            StartSetDataSOs(weaponDataSOs); // 제너릭 메서드 호출
            SummonDataManager.Instance.AddExperience(ItemType.Weapon, summonCount);
        }
    }

    public void OnAccSummon(int summonCount)
    {
        float price = 0;

        switch (summonCount)
        {
            case 1:
                price = 50f;
                break;
            case 11:
                price = 500f;
                break;
            default:
                price = 1500f;
                break;
        }

        if (CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond) >= price)
        {
            CurrencyManager.Instance.SubtractCurrency(CurrencyType.Diamond, price);

            // 악세서리 소환 예제
            var accessoryDataSOs = summon.SummonAccessoryDataSOList(summonCount);
            SetDiaAmountLabel();
            SetSlotAsCount(summonCount);
            ClearSlotData();
            StartSetDataSOs(accessoryDataSOs); // 제너릭 메서드 호출
            SummonDataManager.Instance.AddExperience(ItemType.Accessory, summonCount);
        }
    }

    public void OnSkillCardSummon(int summonCount)
    {
        float price = 0;

        switch (summonCount)
        {
            case 1:
                price = 300f;
                break;
            case 11:
                price = 3000f;
                break;
            default:
                price = 9000f;
                break;
        }

        if (CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond) >= price)
        {
            CurrencyManager.Instance.SubtractCurrency(CurrencyType.Diamond, price);

            // 스킬 소환 예제
            var skillDataSOs = summon.SummonSkillDataSOList(summonCount);
            SetDiaAmountLabel();
            SetSlotAsCount(summonCount);
            ClearSlotData();
            StartSetDataSOs(skillDataSOs); // 제너릭 메서드 호출
            SummonDataManager.Instance.AddExperience(ItemType.Skill, summonCount);
        }
    }
}