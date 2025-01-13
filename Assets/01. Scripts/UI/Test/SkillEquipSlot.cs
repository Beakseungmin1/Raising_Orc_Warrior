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
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private GameObject highlightEffect2;

    private Image skillIconImage;
    private BaseSkill equippedSkill;
    private int slotIndex;
    private EquipManager equipManager;

    private Color defaultColor = new Color32(50, 50, 50, 255);
    private Color equippedColor = Color.white;

    private void Start()
    {
        if (skillIcon != null)
        {
            skillIconImage = skillIcon.GetComponent<Image>();
        }
    }

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

        Debug.Log($"[SkillEquipSlot] ���� {slotIndex}�� ��ų {skill?.SkillData.itemName ?? "����"} ������Ʈ��. ���� ����: {isEquipped}");
    }

    public void UpdateHighlight(bool isWaitingForEquip)
    {
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(isWaitingForEquip && equippedSkill == null);
        }
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
                conditionText.text = $"{equippedSkill.RemainingCooldown:F1} ��";
            }

            skillIcon.color = equippedSkill != null ? equippedColor : defaultColor;
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

            skillIcon.color = equippedSkill != null ? equippedColor : defaultColor;
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
            playerSkillHandler.UseSkill(equippedSkill, transform.position);

            StartCoroutine(ReactivateTextAfterDelay());

            StartCoroutine(ActivateHighlightEffect());
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

        if (skillIcon != null)
        {
            if (skillIcon.sprite == null)
            {
                skillIcon.sprite = defaultSprite;
            }

            skillIcon.color = defaultColor;

            if (skillIconImage != null)
            {
                skillIconImage.type = Image.Type.Filled;
            }
        }

        if (highlightEffect != null)
        {
            highlightEffect.SetActive(false);
        }
    }

    
    private IEnumerator ActivateHighlightEffect()
    {
        if (highlightEffect2 != null)
        {
            highlightEffect2.SetActive(true);
        }

        yield return new WaitForSeconds(equippedSkill.SkillData.buffDuration);

        Debug.Log(equippedSkill.SkillData.buffDuration);

        if (highlightEffect2 != null)
        {
            highlightEffect2.SetActive(false);
        }
    }
}