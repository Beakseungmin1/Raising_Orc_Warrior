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

    private BaseSkill skillData; // Skill → BaseSkill
    private SkillEquipSlotManager equipSlotManager;

    private Color defaultColor = Color.white;
    private Color emptyColor = new Color32(80, 80, 80, 255);

    public void InitializeSlot(BaseSkill skill, int currentAmount, int requiredAmount, bool isEquipped, SkillEquipSlotManager equipManager)
    {
        skillData = skill;
        equipSlotManager = equipManager;

        if (skillData != null)
        {
            skillIcon.sprite = skillData.SkillData.icon;
            nameTxt.text = skillData.SkillData.itemName;
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

        if (skillData != null)
        {
            var skillDetailUIInstance = UIManager.Instance.Show<SkillInfoPopupUI>();

            if (skillDetailUIInstance != null)
            {
                int currentMaterialCount = PlayerObjManager.Instance.Player.inventory.SkillInventory.GetItemStackCount(skillData);

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
        return skillData?.SkillData == skill; // BaseSkill에 맞게 수정
    }
}