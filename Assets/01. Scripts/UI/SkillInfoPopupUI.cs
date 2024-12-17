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
    [SerializeField] private TextMeshProUGUI materialCountTxt; // ����/�ʿ��� ��ųī�� ��
    [SerializeField] private Image skillImage;
    [SerializeField] private Slider materialSlider; // ��ųī�� ���൵ �����̴�
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostTxt;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image currencyIcon;

    private BaseSkill currentSkill; // Skill �� BaseSkill
    private SkillEquipSlotManager equipSlotManager;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitBtn);
    }

    public void Initialize(SkillEquipSlotManager manager)
    {
        equipSlotManager = manager;
    }

    public void DisplaySkillDetails(BaseSkill skill, int currentMaterialCount) // Skill �� BaseSkill
    {
        currentSkill = skill;

        skillNameTxt.text = skill.SkillData.itemName;
        descriptionTxt.text = skill.SkillData.description;
        currentLevelTxt.text = skill.SkillData.currentLevel.ToString();

        gradeTxt.text = $"[{TranslateGrade(skill.SkillData.grade)}]";
        gradeTxt.color = skill.SkillData.gradeColor;

        skillImage.sprite = skill.SkillData.icon;
        currencyIcon.sprite = skill.SkillData.currencyIcon;

        // ȿ�� ���� �ؽ�Ʈ ����
        effectDescriptionTxt.text = GenerateEffectDescription(skill);

        // �ʿ� ���ݼ� �Ǵ� ��� �ð� ǥ��
        if (skill.SkillData.activationCondition == ActivationCondition.HitBased)
        {
            requiredAttackCountLabel.text = "�ʿ� ���ݼ�";
            requiredAttackCountTxt.text = skill.SkillData.requiredHits.ToString();
        }
        else
        {
            requiredAttackCountLabel.text = "��� �ð�";
            requiredAttackCountTxt.text = $"{skill.SkillData.cooldown:F1} ��";
        }

        // ���� �ڽ�Ʈ ǥ��
        requiredMPTxt.text = skill.SkillData.manaCost > 0 ? skill.SkillData.manaCost.ToString() : "-";

        // ����/�ʿ��� ��� �� ǥ�� �� �����̴� ����
        int requiredMaterials = skill.SkillData.requireSkillCardsForUpgrade;
        materialCountTxt.text = $"{currentMaterialCount} / {requiredMaterials}";
        materialSlider.value = (float)currentMaterialCount / requiredMaterials;

        // ��ȭ ��� �ؽ�Ʈ
        upgradeCostTxt.text = skill.SkillData.requiredCurrencyForUpgrade.ToString();

        // ��ư ������ ����
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeSkill);

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(PrepareSkillForEquip);
    }

    private string GenerateEffectDescription(BaseSkill skill) // Skill �� BaseSkill
    {
        switch (skill.SkillData.skillType)
        {
            case SkillType.Active:
                return $"���� {skill.SkillData.effectRange} �̳��� �� ��ο��� ���ݷ��� {skill.SkillData.damagePercent}%�� {skill.SkillData.requiredHits}ȸ ����";
            case SkillType.Buff:
                return $"{skill.SkillData.buffDuration}�ʰ� ��ü ���ݷ� +{skill.SkillData.attackIncreasePercent}%";
            case SkillType.Passive:
                return $"���� ���� ��, {skill.SkillData.periodicInterval}�ʸ��� ��ü ���ݷ� +{skill.SkillData.attackIncreasePercent}%";
            default:
                return "�� �� ���� ��ų Ÿ��";
        }
    }

    private void UpgradeSkill()
    {
        if (currentSkill == null)
        {
            Debug.LogWarning("��ȭ�� ��ų�� �����ϴ�.");
            return;
        }

        bool success = currentSkill.Enhance();

        if (success)
        {
            DisplaySkillDetails(currentSkill, currentSkill.StackCount);

            Debug.Log($"��ų {currentSkill.SkillData.itemName} ��ȭ �Ϸ�!");
        }
        else
        {
            Debug.LogWarning("��ȭ�� �����߽��ϴ�. ��� �Ǵ� ��ȭ�� �����մϴ�.");
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
            Grade.Normal => "�Ϲ�",
            Grade.Uncommon => "���",
            Grade.Rare => "����",
            Grade.Hero => "����",
            Grade.Legendary => "����",
            Grade.Mythic => "��ȭ",
            Grade.Ultimate => "�Ҹ�",
            _ => "�� �� ����"
        };
    }
}