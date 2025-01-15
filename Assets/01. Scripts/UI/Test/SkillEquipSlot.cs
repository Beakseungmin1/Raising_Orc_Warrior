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
        // 버프 스킬 또는 패시브 스킬일 경우 지속 시간 처리
        if (equippedSkill != null && usedSkill == equippedSkill)
        {
            ActivateSkillDuration(usedSkill);
        }
    }

    private void ActivateSkillDuration(BaseSkill skill)
    {
        if (skill == null) return;

        // 스킬의 지속 시간 설정
        buffDuration = skill.SkillData.buffDuration; // SkillData에서 지속 시간 가져오기
        currentBuffTime = buffDuration;
        isBuffActive = true;

        // 지속 시간 UI 활성화
        if (buffDurationImage != null)
        {
            buffDurationImage.fillAmount = 1f;
            buffDurationImage.gameObject.SetActive(true);
        }

        // 기존 코루틴 중단 후 새 코루틴 시작
        StopAllCoroutines();
        StartCoroutine(HandleSkillDuration());
    }

    private IEnumerator HandleSkillDuration()
    {
        while (isBuffActive && currentBuffTime > 0)
        {
            yield return null;

            // 지속 시간 감소
            currentBuffTime -= Time.deltaTime;

            // 지속 시간 UI 업데이트
            if (buffDurationImage != null)
            {
                buffDurationImage.fillAmount = Mathf.Clamp01(currentBuffTime / buffDuration);
            }
        }

        // 스킬 효과 종료 처리
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

        Debug.Log($"[SkillEquipSlot] 슬롯 {slotIndex}에 스킬 {skill?.SkillData.itemName ?? "없음"} 업데이트됨. 장착 상태: {isEquipped}");
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
                conditionText.text = $"{equippedSkill.RemainingCooldown:F1} 초";
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
