using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillEquipSlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button slotButton;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private Image IconImage;
    [SerializeField] private TextMeshProUGUI conditionText;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private Image buffDurationImage;

    private Image skillIconImage;
    private BaseSkill equippedSkill;
    private int slotIndex;
    private EquipManager equipManager;
    private PlayerSkillHandler playerSkillHandler;

    private Color defaultColor = new Color32(50, 50, 50, 255);
    private Color equippedColor = Color.white;

    private float buffDuration = 10f;
    private float currentBuffTime = 0f;
    private bool isBuffActive = false;

    private void Start()
    {
        if (skillIcon != null)
        {
            skillIconImage = skillIcon.GetComponent<Image>();

            if (skillIconImage != null)
            {
                skillIconImage.type = Image.Type.Sliced;
            }
        }

        EnsureSkillHandler();
    }

    private void EnsureSkillHandler()
    {
        if (playerSkillHandler == null)
        {
            var player = PlayerObjManager.Instance?.Player;
            if (player != null)
            {
                playerSkillHandler = player.SkillHandler;
            }

            if (playerSkillHandler == null)
            {
                Debug.LogWarning("[SkillEquipSlot] PlayerSkillHandler is not initialized yet.");
            }
        }
    }

    private void OnEnable()
    {
        EnsureSkillHandler();
        if (playerSkillHandler != null)
        {
            playerSkillHandler.OnSkillUsed += HandleSkillUsed;
        }
    }

    private void OnDisable()
    {
        if (playerSkillHandler != null)
        {
            playerSkillHandler.OnSkillUsed -= HandleSkillUsed;
        }
    }

    private void HandleSkillUsed(BaseSkill usedSkill)
    {
        // ���� ��ų �Ǵ� �нú� ��ų�� ��� ���� �ð� ó��
        if (equippedSkill != null && usedSkill == equippedSkill)
        {
            ActivateSkillDuration(usedSkill);
        }
    }

    private void ActivateSkillDuration(BaseSkill skill)
    {
        if (skill == null) return;

        // ��ų�� ���� �ð� ����
        buffDuration = skill.SkillData.buffDuration; // SkillData���� ���� �ð� ��������
        currentBuffTime = buffDuration;
        isBuffActive = true;

        // ���� �ð� UI Ȱ��ȭ
        if (buffDurationImage != null)
        {
            buffDurationImage.fillAmount = 1f;
            buffDurationImage.gameObject.SetActive(true);
        }

        // ���� �ڷ�ƾ �ߴ� �� �� �ڷ�ƾ ����
        StopAllCoroutines();
        StartCoroutine(HandleSkillDuration());
    }

    private IEnumerator HandleSkillDuration()
    {
        while (isBuffActive && currentBuffTime > 0)
        {
            yield return null;

            // ���� �ð� ����
            currentBuffTime -= Time.deltaTime;

            // ���� �ð� UI ������Ʈ
            if (buffDurationImage != null)
            {
                buffDurationImage.fillAmount = Mathf.Clamp01(currentBuffTime / buffDuration);
            }
        }

        // ��ų ȿ�� ���� ó��
        if (currentBuffTime <= 0)
        {
            isBuffActive = false;

            if (buffDurationImage != null)
            {
                buffDurationImage.fillAmount = 0;
                buffDurationImage.gameObject.SetActive(false);
            }
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

            if (IconImage != null)
            {
                IconImage.gameObject.SetActive(false);
            }

            if (skillIconImage != null)
            {
                skillIconImage.type = Image.Type.Sliced;
            }

            return;
        }

        conditionText.gameObject.SetActive(true);

        if (equippedSkill.SkillData.activationCondition == ActivationCondition.Cooldown)
        {
            float cooldownRatio = Mathf.Clamp01(equippedSkill.RemainingCooldown / equippedSkill.SkillData.cooldown);
            cooldownImage.fillAmount = 1 - cooldownRatio;

            if (IconImage != null)
            {
                IconImage.gameObject.SetActive(true);
                IconImage.sprite = equippedSkill.SkillData.icon;
            }
            if (skillIconImage != null)
            {
                skillIconImage.type = Image.Type.Filled;
            }
            if (equippedSkill.IsReadyToActivate())
            {
                if (IconImage != null)
                {
                    IconImage.gameObject.SetActive(false);
                }

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

            if (IconImage != null)
            {
                IconImage.gameObject.SetActive(true);
                IconImage.sprite = equippedSkill.SkillData.icon;
            }
            if (skillIconImage != null)
            {
                skillIconImage.type = Image.Type.Filled;
            }
            if (equippedSkill.IsReadyToActivate())
            {
                if (IconImage != null)
                {
                    IconImage.gameObject.SetActive(false);
                }

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

        if (buffDurationImage != null)
        {
            buffDurationImage.fillAmount = 0;
        }
    }
}
