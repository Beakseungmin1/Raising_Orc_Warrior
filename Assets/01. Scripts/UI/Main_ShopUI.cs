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

    private Dictionary<ItemType, int> summonTypeMapping;

    private void Awake()
    {
        summon = GetComponent<Summon>();
        RefreshUI();
        SummonDataManager.Instance.OnExpChanged += RefreshUI;
        SummonDataManager.Instance.OnLevelChanged += RefreshUI;
    }

    public void ShowMainUI(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.Instance.Show<Main_PlayerUpgradeUI>();
                break;
            case 1:
                UIManager.Instance.Show<Main_SkillUI>();
                break;
            case 2:
                UIManager.Instance.Show<Main_EquipmentUI>();
                break;
            case 3:
                Debug.Log("����ĵ��������");
                UIManager.Instance.Show<Main_ShopUI>();
                break;
            case 4:
                UIManager.Instance.Show<Main_DungeonUI>();
                break;
            case 5:
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }
        Hide();
    }

    public void OnWeaponSummon(int summonCount)
    {
        List<WeaponDataSO> weaponDataSOs = new List<WeaponDataSO>();
        weaponDataSOs = summon.SummonWeaponDataSOList(summonCount); //���������� ����Ʈ�� ���õȴ�. //OK

        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.curSummoningItemType = ItemType.Weapon;
        summonPopupUI.SetSlotAsCount(summonCount);
        summonPopupUI.ClearSlotData();
        summonPopupUI.StartSetDataSOs(weaponDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.

        if (SummonDataManager.Instance.GetLevel(ItemType.Weapon) < 50)
        {
            SummonDataManager.Instance.AddExperience(ItemType.Weapon, summonCount);
        }
    }

    public void OnAccSummon(int summonCount)
    {
        List<AccessoryDataSO> accessoryDataSOs = new List<AccessoryDataSO>();
        accessoryDataSOs = summon.SummonAccessoryDataSOList(summonCount);

        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.curSummoningItemType = ItemType.Accessory;
        summonPopupUI.SetSlotAsCount(summonCount);
        summonPopupUI.ClearSlotData();
        summonPopupUI.StartSetDataSOs(accessoryDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.

        if (SummonDataManager.Instance.GetLevel(ItemType.Accessory) < 50)
        {
            SummonDataManager.Instance.AddExperience(ItemType.Accessory, summonCount);
        }
    }

    public void OnSkillCardSummon(int summonCount)
    {
        List<SkillDataSO> skillDataSOs = new List<SkillDataSO>();
        skillDataSOs = summon.SummonSkillDataSOList(summonCount);

        SummonPopupUI summonPopupUI = UIManager.Instance.Show<SummonPopupUI>();
        summonPopupUI.curSummoningItemType = ItemType.Skill;
        summonPopupUI.SetSlotAsCount(summonCount);
        summonPopupUI.ClearSlotData();
        summonPopupUI.StartSetDataSOs(skillDataSOs); //�� ������ ���������͸� �������� ���������͸� �������ش�.

        if (SummonDataManager.Instance.GetLevel(ItemType.Skill) < 50)
        {
            SummonDataManager.Instance.AddExperience(ItemType.Skill, summonCount);
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
}
