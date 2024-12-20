using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button slotButton;
    [SerializeField] private Image cooldownImage; // ä������ �̹���
    [SerializeField] private TextMeshProUGUI conditionText; // ���� ǥ�ÿ� �ؽ�Ʈ

    private BaseSkill equippedSkill;
    private int slotIndex;
    private EquipManager equipManager;

    private Color defaultColor = new Color32(50, 50, 50, 255); // ���� ����� �� �⺻ ����
    private Color equippedColor = Color.white; // ��ų ���� �� �⺻ ����
    private Color cooldownColor = new Color32(50, 50, 50, 255); // ��Ÿ�� �� ����

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
        UpdateSkillUI(); // ���� ������Ʈ �� UI�� ����

        Debug.Log($"[SkillEquipSlot] ���� {slotIndex}�� ��ų {skill?.SkillData.itemName ?? "����"} ������Ʈ��. ���� ����: {isEquipped}");
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
            conditionText.text = $"{equippedSkill.RemainingCooldown:F1} ��";

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
        // �⺻ �ʱ�ȭ
        cooldownImage.fillAmount = 1; // �⺻ ���´� �� �� ����
        conditionText.text = "";

        // ������ ��� ������ �⺻ �������� �ʱ�ȭ
        skillIcon.color = equippedSkill == null ? defaultColor : equippedColor;
    }
}
