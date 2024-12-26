using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;

    [Header("Auto Activation")]
    [SerializeField] private Button autoActivateButton; // 자동 발동 버튼
    [SerializeField] private Image autoActivateIndicator; // 버튼 상태 표시용 이미지
    [SerializeField] private Animator autoActivateAnimator; // 자동 발동 버튼 애니메이터
    [SerializeField] private Color inactiveColor = Color.gray; // 자동 발동 비활성화 색상
    private bool autoActivate = false; // 자동 발동 상태

    private List<SkillEquipSlot> equipSlots = new List<SkillEquipSlot>();
    private EquipManager equipManager;

    private void Start()
    {
        equipManager = PlayerObjManager.Instance.Player.EquipManager;

        if (equipManager == null)
        {
            return;
        }

        equipManager.InitializeSkillSlots();

        CreateSlots();

        equipManager.OnSkillEquippedChanged += UpdateSlot;

        // 버튼 이벤트 연결
        if (autoActivateButton != null)
        {
            autoActivateButton.onClick.AddListener(ToggleAutoActivate);
            UpdateAutoActivateButtonUI();
        }
    }

    private void Update()
    {
        HighlightWaitingSlot(); // 장착 대기 슬롯 반짝임 업데이트

        if (autoActivate)
        {
            HandleAutoActivation(); // 자동 발동 처리
        }
    }

    private void CreateSlots()
    {
        int slotCount = equipManager.EquippedSkills.Count;

        foreach (var slot in equipSlots)
        {
            Destroy(slot.gameObject);
        }
        equipSlots.Clear();

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(skillSlotPrefab, contentParent);
            SkillEquipSlot slot = slotObj.GetComponent<SkillEquipSlot>();
            if (slot != null)
            {
                slot.InitializeSlot(i, equipManager);
                equipSlots.Add(slot);
            }
        }
    }

    private void UpdateSlot(BaseSkill skill, int slotIndex, bool isEquipped)
    {
        if (slotIndex >= 0 && slotIndex < equipSlots.Count)
        {
            equipSlots[slotIndex].UpdateSlot(skill, isEquipped);
            Debug.Log($"[SkillEquipSlotManager] 슬롯 {slotIndex}가 업데이트되었습니다. 장착 상태: {isEquipped}");
        }

        if (!isEquipped)
        {
            foreach (var slot in equipSlots)
            {
                if (slot.GetEquippedSkill() == skill)
                {
                    slot.UpdateSlot(null, false);
                    Debug.Log($"[SkillEquipSlotManager] 스킬 {skill.SkillData.itemName}이 제거되었습니다.");
                    break;
                }
            }
        }
    }

    private void HighlightWaitingSlot()
    {
        foreach (var slot in equipSlots)
        {
            slot.UpdateHighlight(equipManager.WaitingSkillForEquip != null);
        }
    }

    private void HandleAutoActivation()
    {
        foreach (var slot in equipSlots)
        {
            var skill = slot.GetEquippedSkill();
            if (skill != null && skill.IsReadyToActivate())
            {
                PlayerObjManager.Instance.Player.SkillHandler.UseSkill(skill, slot.transform.position);
                Debug.Log($"[SkillEquipSlotManager] 자동 발동: {skill.SkillData.itemName}");
            }
        }
    }

    public void ToggleAutoActivate()
    {
        autoActivate = !autoActivate;
        UpdateAutoActivateButtonUI();
        Debug.Log($"[SkillEquipSlotManager] 자동 발동 상태: {autoActivate}");
    }

    private void UpdateAutoActivateButtonUI()
    {
        if (autoActivateAnimator != null)
        {
            // 애니메이터 트리거 또는 상태 변경
            autoActivateAnimator.SetBool("IsActive", autoActivate);
        }

        if (autoActivateIndicator != null)
        {
            // 비활성화 상태에서만 색상 변경
            autoActivateIndicator.color = autoActivate ? Color.white : inactiveColor;
        }
    }
}
