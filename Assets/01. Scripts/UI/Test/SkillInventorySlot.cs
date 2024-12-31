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

    [SerializeField] private GameObject normalImage;
    [SerializeField] private GameObject uncommonImage;
    [SerializeField] private GameObject rareImage;
    [SerializeField] private GameObject heroImage;
    [SerializeField] private GameObject legendaryImage;
    [SerializeField] private GameObject mythicImage;
    [SerializeField] private GameObject ultimateImage;

    private BaseSkill skillData;
    private SkillDataSO skillDataSO;

    private EquipManager equipManager;

    private Color defaultColor = Color.white;
    private Color equippedColor = new Color32(50, 50, 50, 255);
    private Color buttonTransparentColor = new Color(1f, 1f, 1f, 0.3f);

    public BaseSkill SkillData => skillData;
    public SkillDataSO GetSkillDataSO() => skillDataSO;

    private void Start()
    {
        equipManager = PlayerObjManager.Instance.Player.EquipManager;
    }

    private void OnEnable()
    {
        if (equipManager != null)
        {
            equipManager.OnSkillEquippedChanged += HandleSkillEquipChanged;
        }
    }

    private void OnDisable()
    {
        if (equipManager != null)
        {
            equipManager.OnSkillEquippedChanged -= HandleSkillEquipChanged;
        }
    }

    public void InitializeSlot(BaseSkill skill, SkillDataSO skillDataRef, int currentAmount, int requiredAmount, bool isEquipped)
    {
        skillData = skill;
        skillDataSO = skillDataRef;

        if (skillData != null)
        {
            skillIcon.sprite = skillData.SkillData.icon;
            nameTxt.text = skillData.SkillData.itemName;
            curAmountTxt.text = $"{currentAmount} / {requiredAmount}";
            amountSlider.value = (float)currentAmount / requiredAmount;

            SetSlotOwned(true);
            SetEquippedState(isEquipped);
            emptyIcon.SetActive(false);

            slotButton.image.color = Color.white;
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OnClickSlot());

            UpdateGradeImage(skillData.SkillData.grade);
        }
        else if (skillDataSO != null)
        {
            skillIcon.sprite = skillDataSO.icon;
            nameTxt.text = skillDataSO.itemName;
            curAmountTxt.text = "0 / 1";
            amountSlider.value = 0;

            SetSlotOwned(false);
            SetEquippedState(false);
            emptyIcon.SetActive(true);

            Color color = slotButton.image.color;
            color.a = 0f;
            slotButton.image.color = color;

            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OnClickSlot(skillDataSO));

            UpdateGradeImage(skillDataSO.grade);
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

        DisableAllGradeImages();
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
                skillDetailUIInstance.DisplaySkillDetails(skillData, currentMaterialCount, skillData.GetRuntimeRequiredSkillCards());
            }
            else if (skillDataSO != null)
            {
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

    private void HandleSkillEquipChanged(BaseSkill changedSkill, int slotIndex, bool isEquipped)
    {
        if (skillData != null && skillData == changedSkill)
        {
            SetEquippedState(isEquipped);
        }
    }

    private void UpdateGradeImage(Grade grade)
    {
        DisableAllGradeImages();

        switch (grade)
        {
            case Grade.Normal:
                normalImage.SetActive(true);
                break;
            case Grade.Uncommon:
                uncommonImage.SetActive(true);
                break;
            case Grade.Rare:
                rareImage.SetActive(true);
                break;
            case Grade.Hero:
                heroImage.SetActive(true);
                break;
            case Grade.Legendary:
                legendaryImage.SetActive(true);
                break;
            case Grade.Mythic:
                mythicImage.SetActive(true);
                break;
            case Grade.Ultimate:
                ultimateImage.SetActive(true);
                break;
        }
    }

    private void DisableAllGradeImages()
    {
        normalImage.SetActive(false);
        uncommonImage.SetActive(false);
        rareImage.SetActive(false);
        heroImage.SetActive(false);
        legendaryImage.SetActive(false);
        mythicImage.SetActive(false);
        ultimateImage.SetActive(false);
    }
}