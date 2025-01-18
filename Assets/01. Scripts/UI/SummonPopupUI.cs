using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonPopupUI : UIBase
{
    public ItemType curSummoningItemType; //���� ��ȯ���� ������ Ÿ��

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

    // ������ Ÿ�Ժ� ����Ʈ ��ųʸ�
    private Dictionary<System.Type, IList> summonDataMapping;

    private void Awake()
    {
        summon = GetComponent<Summon>();

        // Dictionary �ʱ�ȭ
        summonSlotMapping = new Dictionary<int, GameObject>
        {
            { 1, summonSlotListArea1obj },
            { 11, summonSlotListArea11obj },
            { 33, summonSlotListArea33obj }
        };

        // ������ ���� ��ųʸ� �ʱ�ȭ
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

        summonDataMapping[typeof(T)] = dataSOs; // ������ ����

        float delayBetweenSlots = 0.05f; // �� ���� ���� ���� (��ø ������ ����)
        Vector3 initialScale = Vector3.one * 12f; // 12�� ũ��
        Vector3 finalScale = Vector3.one; // 1�� ũ��
        Color transparentColor = new Color(1f, 1f, 1f, 0f); // ����
        Color opaqueColor = new Color(1f, 1f, 1f, 1f); // ������

        for (int i = 0; i < summonSlotObjs.Count; i++)
        {
            if (i < dataSOs.Count && dataSOs[i] != null)
            {
                // ���� ������ ����
                summonSlotObjs[i].GetComponent<SummonSlot>().SetSlot(dataSOs[i]);

                // ���Ժ� �ڷ�ƾ�� �����Ͽ� �ִϸ��̼� ����
                StartCoroutine(AnimateSlot(summonSlotObjs[i], initialScale, finalScale, transparentColor, opaqueColor));

                // ���� ���� ���۱��� ��� �ð� (���� �� ��ø ���� ����)
                yield return new WaitForSeconds(delayBetweenSlots);
            }
        }
        
        float summonAnimationDuration = 0.1f;
        // ��� ���� �ִϸ��̼� �Ϸ� �� ��ư Ȱ��ȭ
        yield return new WaitForSeconds(summonAnimationDuration); // ������ ���� �ִϸ��̼� ���
        SetBtnInteractable(true);
    }

    // ���� �ϳ��� ���� �ִϸ��̼� ó��
    private IEnumerator AnimateSlot(GameObject slotObj, Vector3 initialScale, Vector3 finalScale, Color transparentColor, Color opaqueColor)
    {
        Image image = slotObj.GetComponent<Image>();
        Image whiteImage = slotObj.GetComponent<SummonSlot>().whiteImage;
        TextMeshProUGUI gradeTxt = slotObj.GetComponent<SummonSlot>().gradeTxt;
        TextMeshProUGUI rankTxt = slotObj.GetComponent<SummonSlot>().rankTxt;
        TextMeshProUGUI rankLabel = slotObj.GetComponent<SummonSlot>().rankLabel;

        // 1. �ʱ� ���� ���� (������ ���, 12�� ũ��, image�� ��������)
        whiteImage.rectTransform.localScale = initialScale;
        whiteImage.color = transparentColor;
        image.enabled = false;
        gradeTxt.enabled = false;
        rankTxt.enabled = false;
        rankLabel.enabled = false;

        float summonAnimationDuration = 0.2f;

        // 2. �ִϸ��̼�: ������ ���(12�� ũ��) -> �������� ���(1�� ũ��)
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

        // ���� ���� ���� (�������� ���, 1�� ũ��)
        whiteImage.rectTransform.localScale = finalScale;
        whiteImage.color = transparentColor;

        // ������ �̹��� ��
        image.enabled = true;
        gradeTxt.enabled = true;
        rankTxt.enabled = true;
        rankLabel.enabled = true;

        // 3. �ִϸ��̼�: �������� ���(1�� ũ��) -> ������ ���(1�� ũ��)
        elapsedTime = 0f;
        while (elapsedTime < summonAnimationDuration)
        {
            yield return new WaitForSeconds(0.03f);

            elapsedTime += Time.deltaTime;

            // ����� (0~1)
            float progress = elapsedTime / summonAnimationDuration;

            // Ease ȿ�� (SmoothStep)
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            // ������ ���� (ũ��� ����)
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

            // ���� ��ȯ ����
            var weaponDataSOs = summon.SummonWeaponDataSOList(summonCount);
            SetDiaAmountLabel();
            SetSlotAsCount(summonCount);
            ClearSlotData();
            StartSetDataSOs(weaponDataSOs); // ���ʸ� �޼��� ȣ��
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

            // �Ǽ����� ��ȯ ����
            var accessoryDataSOs = summon.SummonAccessoryDataSOList(summonCount);
            SetDiaAmountLabel();
            SetSlotAsCount(summonCount);
            ClearSlotData();
            StartSetDataSOs(accessoryDataSOs); // ���ʸ� �޼��� ȣ��
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

            // ��ų ��ȯ ����
            var skillDataSOs = summon.SummonSkillDataSOList(summonCount);
            SetDiaAmountLabel();
            SetSlotAsCount(summonCount);
            ClearSlotData();
            StartSetDataSOs(skillDataSOs); // ���ʸ� �޼��� ȣ��
            SummonDataManager.Instance.AddExperience(ItemType.Skill, summonCount);
        }
    }
}