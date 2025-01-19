using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_ShopUI : UIBase
{
    private Summon summon;

    [Header("Weapon")]
    [SerializeField] private TextMeshProUGUI weaponSummonLevelTxt;
    [SerializeField] private TextMeshProUGUI weaponSummonExpTxt;
    public Slider weaponExpSlider;

    [Header("Accessory")]
    [SerializeField] private TextMeshProUGUI accSummonLevelTxt;
    [SerializeField] private TextMeshProUGUI accSummonExpTxt;
    public Slider accExpSlider;

    [Header("Skill")]
    [SerializeField] private TextMeshProUGUI skillSummonLevelTxt;
    [SerializeField] private TextMeshProUGUI skillSummonExpTxt;
    public Slider skillExpSlider;

    private Dictionary<GameObject, int> summonRedDotMapping;

    public GameObject weaponOneTimesRedDot; //ItemType.Weapon; 50;
    public GameObject weaponElevenTimesRedDot; //ItemType.Weapon; 500;
    public GameObject accessoryOneTimeRedDot; //ItemType.Accessory; 50;
    public GameObject accessoryElevenTimesRedDot; //ItemType.Accessory; 500;
    public GameObject skillOneTimeRedDot; //ItemType.Skill; 300;
    public GameObject skillElevenTimesRedDot; //ItemType.Skill; 3000;

    private void Awake()
    {
        // Initialize summonRedDotMapping (GameObject�� Key��, ���(int)�� Value�� ����)
        summonRedDotMapping = new Dictionary<GameObject, int>
        {
            { weaponOneTimesRedDot, 50 },
            { weaponElevenTimesRedDot, 500 },
            { accessoryOneTimeRedDot, 50 },
            { accessoryElevenTimesRedDot, 500 },
            { skillOneTimeRedDot, 300 },
            { skillElevenTimesRedDot, 3000 }
        };

        summon = GetComponent<Summon>();
        SummonDataManager.Instance.OnExpChanged += RefreshUI;
        SummonDataManager.Instance.OnLevelChanged += RefreshUI;
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.currencyEvents.onDiamondChanged += ShowOrHideRedDot;
        ShowOrHideRedDot();
        RefreshUI();
    }
    private void OnDisable()
    {
        GameEventsManager.Instance.currencyEvents.onDiamondChanged -= ShowOrHideRedDot;
    }

    public void OnWeaponSummon(int summonCount)
    {
        float price = summonCount == 1 ? 50f : summonCount == 11 ? 500f : 1500f;

        // Weapon ��ȯ ������ ����Ʈ ����
        List<WeaponDataSO> weaponDataSOs = summon.SummonWeaponDataSOList(summonCount);

        // ���� �۾� ����
        PerformSummon(ItemType.Weapon, summonCount, price, weaponDataSOs);
    }

    public void OnAccSummon(int summonCount)
    {
        float price = summonCount == 1 ? 50f : summonCount == 11 ? 500f : 1500f;

        // Accessory ��ȯ ������ ����Ʈ ����
        List<AccessoryDataSO> accessoryDataSOs = summon.SummonAccessoryDataSOList(summonCount);

        // ���� �۾� ����
        PerformSummon(ItemType.Accessory, summonCount, price, accessoryDataSOs);
    }

    public void OnSkillCardSummon(int summonCount)
    {
        float price = summonCount == 1 ? 300f : summonCount == 11 ? 3000f : 9000f;

        // Skill ��ȯ ������ ����Ʈ ����
        List<SkillDataSO> skillDataSOs = summon.SummonSkillDataSOList(summonCount);

        // ���� �۾� ����
        PerformSummon(ItemType.Skill, summonCount, price, skillDataSOs);
    }

    private void PerformSummon<T>(ItemType itemType, int summonCount, float price, List<T> dataSOList) where T : BaseItemDataSO
    {
        // ���̾Ƹ�尡 ������� Ȯ��
        if (CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond) >= price)
        {
            // ���̾Ƹ�� ����
            CurrencyManager.Instance.SubtractCurrency(CurrencyType.Diamond, price);

            SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
            summonPopupUI.curSummoningItemType = itemType;
            summonPopupUI.SetSlotAsCount(summonCount);
            summonPopupUI.ClearSlotData();
            summonPopupUI.StartSetDataSOs(dataSOList); // ��ȯ ������ ����

            // ����ġ �߰�
            if (SummonDataManager.Instance.GetLevel(itemType) < 50)
            {
                SummonDataManager.Instance.AddExperience(itemType, summonCount);
            }
        }
    }


    public void RefreshUI()
    {
        weaponSummonLevelTxt.text = SummonDataManager.Instance.GetLevel(ItemType.Weapon).ToString();
        weaponSummonExpTxt.text = $"{SummonDataManager.Instance.GetExp(ItemType.Weapon).ToString("F0")} / {SummonDataManager.Instance.GetExpToNextLevel(ItemType.Weapon).ToString("F0")}";
        weaponExpSlider.value = SummonDataManager.Instance.GetExp(ItemType.Weapon) / SummonDataManager.Instance.GetExpToNextLevel(ItemType.Weapon);
        if(SummonDataManager.Instance.GetLevel(ItemType.Weapon) >= 50)
        {
            weaponSummonExpTxt.text = "Max Level";
            weaponExpSlider.value = 1;
        }

        accSummonLevelTxt.text = SummonDataManager.Instance.GetLevel(ItemType.Accessory).ToString();
        accSummonExpTxt.text = $"{SummonDataManager.Instance.GetExp(ItemType.Accessory).ToString("F0")} / {SummonDataManager.Instance.GetExpToNextLevel(ItemType.Accessory).ToString("F0")}";
        accExpSlider.value = SummonDataManager.Instance.GetExp(ItemType.Accessory) / SummonDataManager.Instance.GetExpToNextLevel(ItemType.Accessory);
        if (SummonDataManager.Instance.GetLevel(ItemType.Accessory) >= 50)
        {
            accSummonExpTxt.text = "Max Level";
            accExpSlider.value = 1;
        }

        skillSummonLevelTxt.text = SummonDataManager.Instance.GetLevel(ItemType.Skill).ToString();
        skillSummonExpTxt.text = $"{SummonDataManager.Instance.GetExp(ItemType.Skill).ToString("F0")} / {SummonDataManager.Instance.GetExpToNextLevel(ItemType.Skill).ToString("F0")}";
        skillExpSlider.value = SummonDataManager.Instance.GetExp(ItemType.Skill) / SummonDataManager.Instance.GetExpToNextLevel(ItemType.Skill);
        if (SummonDataManager.Instance.GetLevel(ItemType.Skill) >= 50)
        {
            skillSummonExpTxt.text = "Max Level";
            skillExpSlider.value = 1;
        }
    }

    public void ShowOrHideRedDot()
    {
        // ���� ������ DungeonTicket ������ ������
        float currentDiamond = CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond);

        // ��ųʸ��� ��ȸ�ϸ鼭 Key(����)�� Value(GameObject)�� Ȯ��
        foreach (var pair in summonRedDotMapping)
        {
            int requiredDiamond = pair.Value;           // �ʿ��� DungeonTicket (Key)
            GameObject redDot = pair.Key;           // RedDot GameObject (Value)

            // SetActive ó��: ������ Ƽ���� Key �� �̻����� Ȯ��
            if (currentDiamond >= requiredDiamond)
            {
                redDot.SetActive(true);  // Ƽ���� ����ϸ� Ȱ��ȭ
            }
            else
            {
                redDot.SetActive(false); // �����ϸ� ��Ȱ��ȭ
            }
        }
    }
}
