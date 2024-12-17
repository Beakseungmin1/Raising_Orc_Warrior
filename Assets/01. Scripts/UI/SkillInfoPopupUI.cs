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
    [SerializeField] private TextMeshProUGUI materialCountTxt; // 현재/필요한 스킬카드 수
    [SerializeField] private Image skillImage;
    [SerializeField] private Slider materialSlider; // 스킬카드 진행도 슬라이더
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeCostTxt;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Image currencyIcon;

    private BaseSkill currentSkill; // Skill → BaseSkill
    private SkillEquipSlotManager equipSlotManager;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitBtn);
    }

    public void Initialize(SkillEquipSlotManager manager)
    {
        equipSlotManager = manager;
    }

    public void DisplaySkillDetails(BaseSkill skill, int currentMaterialCount) // Skill → BaseSkill
    {
        currentSkill = skill;

        skillNameTxt.text = skill.SkillData.itemName;
        descriptionTxt.text = skill.SkillData.description;
        currentLevelTxt.text = skill.SkillData.currentLevel.ToString();

        gradeTxt.text = $"[{TranslateGrade(skill.SkillData.grade)}]";
        gradeTxt.color = skill.SkillData.gradeColor;

        skillImage.sprite = skill.SkillData.icon;
        currencyIcon.sprite = skill.SkillData.currencyIcon;

        // 효과 설명 텍스트 설정
        effectDescriptionTxt.text = GenerateEffectDescription(skill);

        // 필요 공격수 또는 대기 시간 표시
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

        // 마나 코스트 표시
        requiredMPTxt.text = skill.SkillData.manaCost > 0 ? skill.SkillData.manaCost.ToString() : "-";

        // 현재/필요한 재료 수 표시 및 슬라이더 설정
        int requiredMaterials = skill.SkillData.requireSkillCardsForUpgrade;
        materialCountTxt.text = $"{currentMaterialCount} / {requiredMaterials}";
        materialSlider.value = (float)currentMaterialCount / requiredMaterials;

        // 강화 비용 텍스트
        upgradeCostTxt.text = skill.SkillData.requiredCurrencyForUpgrade.ToString();

        // 버튼 리스너 설정
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeSkill);

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(PrepareSkillForEquip);
    }

    private string GenerateEffectDescription(BaseSkill skill) // Skill → BaseSkill
    {
        switch (skill.SkillData.skillType)
        {
            case SkillType.Active:
                return $"범위 {skill.SkillData.effectRange} 이내의 적 모두에게 공격력의 {skill.SkillData.damagePercent}%로 {skill.SkillData.requiredHits}회 공격";
            case SkillType.Buff:
                return $"{skill.SkillData.buffDuration}초간 전체 공격력 +{skill.SkillData.attackIncreasePercent}%";
            case SkillType.Passive:
                return $"전투 돌입 후, {skill.SkillData.periodicInterval}초마다 전체 공격력 +{skill.SkillData.attackIncreasePercent}%";
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
            DisplaySkillDetails(currentSkill, currentSkill.StackCount);

            Debug.Log($"스킬 {currentSkill.SkillData.itemName} 강화 완료!");
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