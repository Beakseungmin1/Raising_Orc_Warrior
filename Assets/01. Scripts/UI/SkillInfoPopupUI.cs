using System.Collections.Generic;
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
        currentLevelTxt.text = 1.ToString();

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
        if (skill.SkillData == null || string.IsNullOrEmpty(skill.SkillData.effectDescription))
            return "설명 없음";

        string template = skill.SkillData.effectDescription;

        var values = new Dictionary<string, object>
    {
        { "range", skill.SkillData.effectRange },
        { "damagePercent", skill.SkillData.damagePercent },
        { "requiredHits", skill.SkillData.requiredHits },
        { "buffDuration", skill.SkillData.buffDuration },
        { "attackIncreasePercent", skill.SkillData.attackIncreasePercent },
        { "cooldown", skill.SkillData.cooldown },
        { "manaRecoveryAmount", skill.SkillData.manaRecoveryAmount },
        { "moveSpeedIncrease", skill.SkillData.moveSpeedIncrease },
        { "attackSpeedIncrease", skill.SkillData.attackSpeedIncrease }
    };

        foreach (var pair in values)
        {
            template = template.Replace($"{{{pair.Key}}}", pair.Value.ToString());
        }

        return template.Replace("\\n", "\n");
    }

    private string GenerateEffectDescription(SkillDataSO skillDataSO)
    {
        if (skillDataSO == null || string.IsNullOrEmpty(skillDataSO.effectDescription))
            return "설명 없음";

        string template = skillDataSO.effectDescription;

        var values = new Dictionary<string, object>
    {
        { "range", skillDataSO.effectRange },
        { "damagePercent", skillDataSO.damagePercent },
        { "requiredHits", skillDataSO.requiredHits },
        { "buffDuration", skillDataSO.buffDuration },
        { "attackIncreasePercent", skillDataSO.attackIncreasePercent },
        { "cooldown", skillDataSO.cooldown },
        { "manaRecoveryAmount", skillDataSO.manaRecoveryAmount },
        { "moveSpeedIncrease", skillDataSO.moveSpeedIncrease },
        { "attackSpeedIncrease", skillDataSO.attackSpeedIncrease }
    };

        foreach (var pair in values)
        {
            template = template.Replace($"{{{pair.Key}}}", pair.Value.ToString());
        }

        return template.Replace("\\n", "\n");
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

        var equipManager = PlayerObjManager.Instance.Player.EquipManager;
        equipManager.SetWaitingSkillForEquip(currentSkill);

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