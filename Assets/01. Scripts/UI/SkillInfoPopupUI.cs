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

    private Skill currentSkill;
    private SkillEquipSlotManager equipSlotManager;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitBtn);
    }

    public void Initialize(SkillEquipSlotManager manager)
    {
        equipSlotManager = manager;
    }

    public void DisplaySkillDetails(Skill skill, int currentMaterialCount)
    {
        currentSkill = skill;

        skillNameTxt.text = skill.BaseData.itemName;
        descriptionTxt.text = skill.BaseData.description;
        currentLevelTxt.text = skill.BaseData.currentLevel.ToString();

        // [�Ϲ�], [���] ���� �ؽ�Ʈ ����
        gradeTxt.text = $"[{TranslateGrade(skill.BaseData.grade)}]";
        gradeTxt.color = skill.BaseData.gradeColor;

        skillImage.sprite = skill.BaseData.icon;
        currencyIcon.sprite = skill.BaseData.currencyIcon;

        // ȿ�� ���� �ؽ�Ʈ ���� ����
        effectDescriptionTxt.text = GenerateEffectDescription(skill);

        // �ʿ� ���ݼ� �Ǵ� ��� �ð� ǥ��
        if (skill.BaseData.activationCondition == ActivationCondition.HitBased)
        {
            requiredAttackCountLabel.text = "�ʿ� ���ݼ�";
            requiredAttackCountTxt.text = skill.BaseData.requiredHits.ToString();
        }
        else
        {
            requiredAttackCountLabel.text = "��� �ð�";
            requiredAttackCountTxt.text = $"{skill.BaseData.cooldown:F1} ��";
        }

        // ���� �ڽ�Ʈ ǥ��
        requiredMPTxt.text = skill.BaseData.manaCost > 0 ? skill.BaseData.manaCost.ToString() : "-";

        // ����/�ʿ��� ��� �� ǥ�� �� �����̴� ����
        int requiredMaterials = skill.BaseData.requireSkillCardsForUpgrade;
        materialCountTxt.text = $"{currentMaterialCount} / {requiredMaterials}";
        materialSlider.value = (float)currentMaterialCount / requiredMaterials;

        // ��ȭ ��� �ؽ�Ʈ
        upgradeCostTxt.text = skill.BaseData.requiredCurrencyForUpgrade.ToString();

        // ��ư ������ ����
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeSkill);

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(PrepareSkillForEquip);
    }

    private string GenerateEffectDescription(Skill skill)
    {
        switch (skill.BaseData.skillType)
        {
            case SkillType.Active:
                return $"���� {skill.BaseData.effectRange} �̳��� �� ��ο��� ���ݷ��� {skill.BaseData.damagePercent}%�� {skill.BaseData.requiredHits}ȸ ����";
            case SkillType.Buff:
                return $"{skill.BaseData.buffDuration}�ʰ� ��ü ���ݷ� +{skill.BaseData.attackIncreasePercent}%";
            case SkillType.Passive:
                return $"���� ���� ��, {skill.BaseData.periodicInterval}�ʸ��� ��ü ���ݷ� +{skill.BaseData.attackIncreasePercent}%";
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

        // ��ȭ �õ�
        bool success = currentSkill.Enhance();

        if (success)
        {
            // ��ȭ ���� �� UI ������Ʈ
            DisplaySkillDetails(currentSkill, currentSkill.StackCount);

            // ���� �޽��� ���
            Debug.Log($"��ų {currentSkill.BaseData.itemName} ��ȭ �Ϸ�!");
        }
        else
        {
            // ��ȭ ���� �� �޽��� ���
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