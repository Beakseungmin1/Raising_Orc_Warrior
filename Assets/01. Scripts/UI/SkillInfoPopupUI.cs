using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPopupUI : UIBase
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;
    [SerializeField] private TextMeshProUGUI effectDescriptionTxt;
    [SerializeField] private TextMeshProUGUI requiredAttackCountLabel;
    [SerializeField] private TextMeshProUGUI requiredAttackCountTxt;
    [SerializeField] private TextMeshProUGUI requiredMPTxt;
    [SerializeField] private TextMeshProUGUI materialCountTxt;
    [SerializeField] private Image skillImage;
    [SerializeField] private Slider materialSlider;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostTxt;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image currencyIcon;

    [SerializeField] private GameObject noInventoryTxt;

    public BaseSkill currentSkill;
    public SkillEquipSlotManager equipSlotManager;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitBtn);
    }

    public void Initialize(SkillEquipSlotManager manager)
    {
        equipSlotManager = manager;
    }

    public void DisplaySkillDetails(BaseSkill skill, int currentMaterialCount, int requiredMaterials)
    {
        Debug.Log($"Displaying details for skill: {skill.SkillData.itemName}");

        currentSkill = skill;

        skillNameTxt.text = skill.SkillData.itemName;
        descriptionTxt.text = skill.SkillData.description;
        currentLevelTxt.text = skill.EnhancementLevel.ToString();

        gradeTxt.text = $"[{TranslateGrade(skill.SkillData.grade)}]";
        gradeTxt.color = skill.SkillData.gradeColor;

        skillImage.sprite = skill.SkillData.icon;
        currencyIcon.sprite = skill.SkillData.currencyIcon;

        effectDescriptionTxt.text = GenerateEffectDescription(skill);

        if (skill.SkillData.activationCondition == ActivationCondition.HitBased)
        {
            requiredAttackCountLabel.text = "필요 공격수";
            requiredAttackCountTxt.text = skill.SkillData.requiredHits.ToString();
        }
        else
        {
            requiredAttackCountLabel.text = "대기 시간";
            requiredAttackCountTxt.text = $"{skill.SkillData.cooldown:F1} 초";
        }

        requiredMPTxt.text = skill.SkillData.manaCost > 0 ? skill.SkillData.manaCost.ToString() : "-";

        int adjustedCurrentMaterialCount = Mathf.Max(0, currentMaterialCount - 1);
        materialCountTxt.text = $"{adjustedCurrentMaterialCount} / {requiredMaterials}";
        materialSlider.value = (float)adjustedCurrentMaterialCount / requiredMaterials;

        upgradeCostTxt.text = skill.SkillData.requiredCurrencyForUpgrade.ToString();

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeSkill);

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(PrepareSkillForEquip);

        if (skill.SkillData == null)
        {
            skillImage.color = new Color(0.2f, 0.2f, 0.2f);
            noInventoryTxt.SetActive(true);
        }
        else
        {
            skillImage.color = Color.white;
            noInventoryTxt.SetActive(false);
        }
    }

    public void DisplaySkillDetails(SkillDataSO skillDataSO)
    {
        Debug.Log($"Displaying details for skill: {skillDataSO.itemName}");

        currentSkill = null;

        skillNameTxt.text = skillDataSO.itemName;
        descriptionTxt.text = skillDataSO.description;
        currentLevelTxt.text = "N/A";

        gradeTxt.text = $"[{TranslateGrade(skillDataSO.grade)}]";
        gradeTxt.color = skillDataSO.gradeColor;

        skillImage.sprite = skillDataSO.icon;
        currencyIcon.sprite = skillDataSO.currencyIcon;

        effectDescriptionTxt.text = GenerateEffectDescription(skillDataSO);

        if (skillDataSO.activationCondition == ActivationCondition.HitBased)
        {
            requiredAttackCountLabel.text = "필요 공격수";
            requiredAttackCountTxt.text = skillDataSO.requiredHits.ToString();
        }
        else
        {
            requiredAttackCountLabel.text = "대기 시간";
            requiredAttackCountTxt.text = $"{skillDataSO.cooldown:F1} 초";
        }

        requiredMPTxt.text = skillDataSO.manaCost > 0 ? skillDataSO.manaCost.ToString() : "-";

        materialCountTxt.text = $"0 / 1";
        materialSlider.value = 0f;

        upgradeButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);

        skillImage.color = new Color(0.2f, 0.2f, 0.2f);
        noInventoryTxt.SetActive(true);
    }

    private string GenerateEffectDescription(BaseSkill skill)
    {
        switch (skill.SkillData.skillType)
        {
            case SkillType.Active:
                return $"범위 {skill.SkillData.effectRange} 이내의 적 모두에게 공격력의 {skill.SkillData.damagePercent}%로 {skill.SkillData.requiredHits}회 공격";
            case SkillType.Buff:
                return $"{skill.SkillData.buffDuration}초간 전체 공격력 +{skill.SkillData.attackIncreasePercent}%";
            case SkillType.Passive:
                return $"전투 돌입 후, {skill.SkillData.buffDuration}초마다 전체 공격력 +{skill.SkillData.attackIncreasePercent}%";
            default:
                return "알 수 없는 스킬 타입";
        }
    }

    private string GenerateEffectDescription(SkillDataSO skillDataSO)
    {
        switch (skillDataSO.skillType)
        {
            case SkillType.Active:
                return $"범위 {skillDataSO.effectRange} 이내의 적 모두에게 공격력의 {skillDataSO.damagePercent}%로 {skillDataSO.requiredHits}회 공격";
            case SkillType.Buff:
                return $"{skillDataSO.buffDuration}초간 전체 공격력 +{skillDataSO.attackIncreasePercent}%";
            case SkillType.Passive:
                return $"전투 돌입 후, {skillDataSO.buffDuration}초마다 전체 공격력 +{skillDataSO.attackIncreasePercent}%";
            default:
                return "알 수 없는 스킬 타입";
        }
    }

    private void UpgradeSkill()
    {
        if (currentSkill == null)
        {
            Debug.LogWarning("강화할 스킬이 없습니다.");
            return;
        }

        bool success = currentSkill.Enhance();

        if (success)
        {
            DisplaySkillDetails(currentSkill, currentSkill.StackCount, currentSkill.GetRuntimeRequiredSkillCards());
        }
        else
        {
            Debug.LogWarning("강화에 실패했습니다. 재료 또는 재화가 부족합니다.");
        }
    }

    private void PrepareSkillForEquip()
    {
        if (currentSkill == null) return;

        equipSlotManager.PrepareSkillForEquip(currentSkill);

        UIManager.Instance.Hide<SkillInfoPopupUI>();
        UIManager.Instance.Hide<DimmedUI>();
    }

    public void ExitBtn()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }

    private string TranslateGrade(Grade grade)
    {
        return grade switch
        {
            Grade.Normal => "일반",
            Grade.Uncommon => "희귀",
            Grade.Rare => "레어",
            Grade.Hero => "영웅",
            Grade.Legendary => "전설",
            Grade.Mythic => "신화",
            Grade.Ultimate => "불멸",
            _ => "알 수 없음"
        };
    }
}