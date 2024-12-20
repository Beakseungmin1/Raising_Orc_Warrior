using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button slotButton;
    [SerializeField] private Image cooldownImage; // 채워지는 이미지
    [SerializeField] private TextMeshProUGUI conditionText; // 숫자 표시용 텍스트

    private BaseSkill equippedSkill;
    private int slotIndex;
    private EquipManager equipManager;

    private Color defaultColor = new Color32(50, 50, 50, 255); // 슬롯 비었을 때 기본 색상
    private Color equippedColor = Color.white; // 스킬 장착 시 기본 색상
    private Color cooldownColor = new Color32(50, 50, 50, 255); // 쿨타임 중 색상

    private void Update()
    {
        UpdateSkillUI();
    }

    public void InitializeSlot(int index, EquipManager manager)
    {
        slotIndex = index;
        equipManager = manager;

        ResetUI();

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnClickSlot);
    }

    public void UpdateSlot(BaseSkill skill, bool isEquipped)
    {
        equippedSkill = skill;
        skillIcon.sprite = skill?.SkillData.icon;

        ResetUI();
        UpdateSkillUI(); // 슬롯 업데이트 시 UI도 갱신

        Debug.Log($"[SkillEquipSlot] 슬롯 {slotIndex}에 스킬 {skill?.SkillData.itemName ?? "없음"} 업데이트됨. 장착 상태: {isEquipped}");
    }

    private void UpdateSkillUI()
    {
        if (equippedSkill == null)
        {
            skillIcon.color = defaultColor;
            cooldownImage.fillAmount = 1;
            conditionText.text = "";
            return;
        }

        if (equippedSkill.SkillData.activationCondition == ActivationCondition.Cooldown)
        {
            float cooldownRatio = Mathf.Clamp01(equippedSkill.RemainingCooldown / equippedSkill.SkillData.cooldown);
            cooldownImage.fillAmount = 1 - cooldownRatio;
            conditionText.text = $"{equippedSkill.RemainingCooldown:F1} 초";

            skillIcon.color = equippedSkill.IsReadyToActivate() ? equippedColor : cooldownColor;
        }
        else if (equippedSkill.SkillData.activationCondition == ActivationCondition.HitBased)
        {
            float hitRatio = Mathf.Clamp01((float)equippedSkill.CurrentHits / equippedSkill.SkillData.requiredHits);
            cooldownImage.fillAmount = hitRatio;
            conditionText.text = $"{equippedSkill.CurrentHits} / {equippedSkill.SkillData.requiredHits}";

            skillIcon.color = equippedSkill.IsReadyToActivate() ? equippedColor : cooldownColor;
        }
    }

    private void OnClickSlot()
    {
        if (equipManager.WaitingSkillForEquip != null)
        {
            equipManager.EquipSkill(equipManager.WaitingSkillForEquip, slotIndex);
        }
        else if (equippedSkill != null)
        {
            RequestSkillActivation();
        }        
    }

    private void RequestSkillActivation()
    {
        var playerSkillHandler = PlayerObjManager.Instance.Player?.SkillHandler;
        if (playerSkillHandler != null && equippedSkill != null)
        {
            Debug.Log($"[SkillEquipSlot] RequestSkillActivation: {equippedSkill.SkillData.itemName}");
            playerSkillHandler.UseSkill(equippedSkill, transform.position);
        }
    }

    public BaseSkill GetEquippedSkill()
    {
        return equippedSkill;
    }

    private void ResetUI()
    {
        // 기본 초기화
        cooldownImage.fillAmount = 1; // 기본 상태는 꽉 찬 상태
        conditionText.text = "";

        // 슬롯이 비어 있으면 기본 색상으로 초기화
        skillIcon.color = equippedSkill == null ? defaultColor : equippedColor;
    }
}
