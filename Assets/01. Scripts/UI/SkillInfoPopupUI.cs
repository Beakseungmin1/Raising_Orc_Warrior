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

        // [일반], [희귀] 등의 텍스트 설정
        gradeTxt.text = $"[{TranslateGrade(skill.BaseData.grade)}]";
        gradeTxt.color = skill.BaseData.gradeColor;

        skillImage.sprite = skill.BaseData.icon;
        currencyIcon.sprite = skill.BaseData.currencyIcon;

        // 효과 설명 텍스트 동적 설정
        effectDescriptionTxt.text = GenerateEffectDescription(skill);

        // 필요 공격수 또는 대기 시간 표시
        if (skill.BaseData.activationCondition == ActivationCondition.HitBased)
        {
            requiredAttackCountLabel.text = "필요 공격수";
            requiredAttackCountTxt.text = skill.BaseData.requiredHits.ToString();
        }
        else
        {
            requiredAttackCountLabel.text = "대기 시간";
            requiredAttackCountTxt.text = $"{skill.BaseData.cooldown:F1} 초";
        }

        // 마나 코스트 표시
        requiredMPTxt.text = skill.BaseData.manaCost > 0 ? skill.BaseData.manaCost.ToString() : "-";

        // 현재/필요한 재료 수 표시 및 슬라이더 설정
        int requiredMaterials = skill.BaseData.requireSkillCardsForUpgrade;
        materialCountTxt.text = $"{currentMaterialCount} / {requiredMaterials}";
        materialSlider.value = (float)currentMaterialCount / requiredMaterials;

        // 강화 비용 텍스트
        upgradeCostTxt.text = skill.BaseData.requiredCurrencyForUpgrade.ToString();

        // 버튼 리스너 설정
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
                return $"범위 {skill.BaseData.effectRange} 이내의 적 모두에게 공격력의 {skill.BaseData.damagePercent}%로 {skill.BaseData.requiredHits}회 공격";
            case SkillType.Buff:
                return $"{skill.BaseData.buffDuration}초간 전체 공격력 +{skill.BaseData.attackIncreasePercent}%";
            case SkillType.Passive:
                return $"전투 돌입 후, {skill.BaseData.periodicInterval}초마다 전체 공격력 +{skill.BaseData.attackIncreasePercent}%";
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

        // 강화 시도
        bool success = currentSkill.Enhance();

        if (success)
        {
            // 강화 성공 시 UI 업데이트
            DisplaySkillDetails(currentSkill, currentSkill.StackCount);

            // 성공 메시지 출력
            Debug.Log($"스킬 {currentSkill.BaseData.itemName} 강화 완료!");
        }
        else
        {
            // 강화 실패 시 메시지 출력
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