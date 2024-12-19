using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInventorySlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI curAmountTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Slider amountSlider;
    [SerializeField] private TextMeshProUGUI equippedTxt;
    [SerializeField] private GameObject emptyIcon;
    [SerializeField] private Button slotButton;

    private BaseSkill skillData;
    private SkillDataSO skillDataSO;

    private SkillEquipSlotManager equipSlotManager;

    public BaseSkill SkillData => skillData;
    public SkillDataSO GetSkillDataSO() => skillDataSO;

    private Color defaultColor = Color.white;
    private Color equippedColor = new Color32(50, 50, 50, 255);
    private Color buttonTransparentColor = new Color(1f, 1f, 1f, 0.3f);

    public void InitializeSlot(BaseSkill skill, SkillDataSO skillDataRef, int currentAmount, int requiredAmount, bool isEquipped, SkillEquipSlotManager equipManager)
    {
        skillData = skill;
        skillDataSO = skillDataRef;
        equipSlotManager = equipManager;

        if (skillData != null)
        {
            skillIcon.sprite = skillData.SkillData.icon;
            nameTxt.text = skillData.SkillData.itemName;
            curAmountTxt.text = $"{currentAmount} / {requiredAmount}";
            amountSlider.value = (float)currentAmount / requiredAmount;

            SetEquippedState(isEquipped);
            SetSlotOwned(true);
            emptyIcon.SetActive(false);

            slotButton.image.color = Color.white;
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OnClickSlot());
        }
        else if (skillDataSO != null)
        {
            skillIcon.sprite = skillDataSO.icon;
            nameTxt.text = skillDataSO.itemName;
            curAmountTxt.text = "0 / 1";
            amountSlider.value = 0;

            SetEquippedState(false);
            SetSlotOwned(false);
            emptyIcon.SetActive(true);

            Color color = slotButton.image.color;
            color.a = 0f;
            slotButton.image.color = color;

            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OnClickSlot(skillDataSO));
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        skillData = null;
        skillDataSO = null;

        skillIcon.sprite = null;
        nameTxt.text = "";
        curAmountTxt.text = "0 / 0";
        amountSlider.value = 0;

        SetSlotOwned(false);
        ClearEquippedState();
        slotButton.onClick.RemoveAllListeners();
    }

    private void OnClickSlot(SkillDataSO skillDataSO = null)
    {
        var dimmedUI = UIManager.Instance.Show<DimmedUI>();
        var skillDetailUIInstance = UIManager.Instance.Show<SkillInfoPopupUI>();

        if (skillDetailUIInstance != null)
        {
            if (skillData != null)
            {
                int currentMaterialCount = PlayerObjManager.Instance.Player.inventory.SkillInventory.GetItemStackCount(skillData);
                skillDetailUIInstance.Initialize(equipSlotManager);
                skillDetailUIInstance.DisplaySkillDetails(skillData, currentMaterialCount, skillData.GetRuntimeRequiredSkillCards());
            }
            else if (skillDataSO != null)
            {
                skillDetailUIInstance.Initialize(equipSlotManager);
                skillDetailUIInstance.DisplaySkillDetails(skillDataSO);
            }
        }
    }

    public void SetEquippedState(bool isEquipped)
    {
        equippedTxt.gameObject.SetActive(isEquipped);
        skillIcon.color = isEquipped ? equippedColor : defaultColor;
    }

    private void ClearEquippedState()
    {
        equippedTxt.gameObject.SetActive(false);
    }

    private void SetSlotOwned(bool isOwned)
    {
        nameTxt.gameObject.SetActive(isOwned);
        curAmountTxt.gameObject.SetActive(isOwned);
        amountSlider.gameObject.SetActive(isOwned);

        emptyIcon.SetActive(!isOwned);
        slotButton.image.color = isOwned ? Color.white : buttonTransparentColor;
    }
}
