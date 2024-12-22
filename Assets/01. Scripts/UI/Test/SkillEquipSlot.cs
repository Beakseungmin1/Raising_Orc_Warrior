using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillEquipSlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button slotButton;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI conditionText;

    private BaseSkill equippedSkill;
    private int slotIndex;
    private EquipManager equipManager;

    private Color defaultColor = new Color32(50, 50, 50, 255);
    private Color equippedColor = Color.white;

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
        UpdateSkillUI();

        Debug.Log($"[SkillEquipSlot] 슬롯 {slotIndex}에 스킬 {skill?.SkillData.itemName ?? "없음"} 업데이트됨. 장착 상태: {isEquipped}");
    }

    private void UpdateSkillUI()
    {
        if (equippedSkill == null)
        {
            skillIcon.color = defaultColor;
            cooldownImage.fillAmount = 1;
            conditionText.text = "";
            conditionText.gameObject.SetActive(false);
            return;
        }

        conditionText.gameObject.SetActive(true);

        if (equippedSkill.SkillData.activationCondition == ActivationCondition.Cooldown)
        {
            float cooldownRatio = Mathf.Clamp01(equippedSkill.RemainingCooldown / equippedSkill.SkillData.cooldown);
            cooldownImage.fillAmount = 1 - cooldownRatio;

            if (equippedSkill.IsReadyToActivate())
            {
                conditionText.gameObject.SetActive(false);
            }
            else
            {
                conditionText.text = $"{equippedSkill.RemainingCooldown:F1} 초";
            }

            skillIcon.color = equippedSkill.IsReadyToActivate() ? equippedColor : defaultColor;
        }
        else if (equippedSkill.SkillData.activationCondition == ActivationCondition.HitBased)
        {
            float hitRatio = Mathf.Clamp01((float)equippedSkill.CurrentHits / equippedSkill.SkillData.requiredHits);
            cooldownImage.fillAmount = hitRatio;

            if (equippedSkill.IsReadyToActivate())
            {
                conditionText.text = $"{equippedSkill.SkillData.requiredHits} / {equippedSkill.SkillData.requiredHits}";
                conditionText.gameObject.SetActive(false);
            }
            else
            {
                conditionText.text = $"{equippedSkill.CurrentHits} / {equippedSkill.SkillData.requiredHits}";
            }

            skillIcon.color = equippedSkill.IsReadyToActivate() ? equippedColor : defaultColor;
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

            StartCoroutine(ReactivateTextAfterDelay());
        }
    }

    private IEnumerator ReactivateTextAfterDelay()
    {
        conditionText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        conditionText.gameObject.SetActive(true);
    }

    public BaseSkill GetEquippedSkill()
    {
        return equippedSkill;
    }

    private void ResetUI()
    {
        cooldownImage.fillAmount = 1;
        conditionText.text = "";

        skillIcon.color = equippedSkill == null ? defaultColor : equippedColor;
    }
}