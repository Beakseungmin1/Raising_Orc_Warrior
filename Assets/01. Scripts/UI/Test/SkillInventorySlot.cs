using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillInventorySlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI curAmountTxt;
    [SerializeField] private TextMeshProUGUI maxAmountTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Slider amountSlider;
    [SerializeField] private TextMeshProUGUI equippedTxt;
    [SerializeField] private Button slotButton;

    private Skill skillData;
    private SkillEquipSlotManager equipSlotManager;

    private Color defaultColor = Color.white; // 흰색 (스킬 추가 시)
    private Color emptyColor = new Color32(80, 80, 80, 255); // 슬롯 비어있을 때 회색

    public void InitializeSlot(Skill skill, int currentAmount, int requiredAmount, bool isEquipped, SkillEquipSlotManager equipManager)
    {
        skillData = skill;
        equipSlotManager = equipManager;

        if (skillData != null)
        {
            skillIcon.sprite = skillData.BaseData.icon;
            nameTxt.text = skillData.BaseData.itemName;
            curAmountTxt.text = currentAmount.ToString();
            maxAmountTxt.text = requiredAmount.ToString();
            amountSlider.value = (float)currentAmount / requiredAmount;

            UpdateSlotColor(true);

            equippedTxt.gameObject.SetActive(isEquipped);
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(OnClickSlot);
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        skillData = null;
        skillIcon.sprite = null;
        nameTxt.text = "";
        curAmountTxt.text = "0";
        maxAmountTxt.text = "0";
        amountSlider.value = 0;

        UpdateSlotColor(false);
        equippedTxt.gameObject.SetActive(false);
        slotButton.onClick.RemoveAllListeners();
    }

    private void OnClickSlot()
    {
        var dimmedUI = UIManager.Instance.Show<DimmedUI>();
        dimmedUI.canvas.sortingOrder = 4;

        if (skillData != null)
        {
            var skillDetailUIInstance = UIManager.Instance.Show<SkillInfoPopupUI>();
            skillDetailUIInstance.canvas.sortingOrder = 5;

            if (skillDetailUIInstance != null)
            {
                int currentMaterialCount = PlayerobjManager.Instance.Player.inventory.SkillInventory.GetItemStackCount(skillData);

                skillDetailUIInstance.Initialize(equipSlotManager);
                skillDetailUIInstance.DisplaySkillDetails(skillData, currentMaterialCount);
            }
        }
    }

    public void SetEquippedState(bool isEquipped)
    {
        equippedTxt.gameObject.SetActive(isEquipped);

        UpdateSlotColor(!isEquipped);
    }

    private void UpdateSlotColor(bool hasSkill)
    {
        skillIcon.color = hasSkill ? defaultColor : emptyColor;
    }

    public bool MatchesSkill(SkillDataSO skill)
    {
        return skillData?.BaseData == skill;
    }
}