using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;

    [Header("Auto Activation")]
    [SerializeField] private Button autoActivateButton; // �ڵ� �ߵ� ��ư
    [SerializeField] private Image autoActivateIndicator; // ��ư ���� ǥ�ÿ� �̹���
    [SerializeField] private Animator autoActivateAnimator; // �ڵ� �ߵ� ��ư �ִϸ�����
    [SerializeField] private Color inactiveColor = Color.gray; // �ڵ� �ߵ� ��Ȱ��ȭ ����
    private bool autoActivate = false; // �ڵ� �ߵ� ����

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

        // ��ư �̺�Ʈ ����
        if (autoActivateButton != null)
        {
            autoActivateButton.onClick.AddListener(ToggleAutoActivate);
            UpdateAutoActivateButtonUI();
        }
    }

    private void Update()
    {
        HighlightWaitingSlot(); // ���� ��� ���� ��¦�� ������Ʈ

        if (autoActivate)
        {
            HandleAutoActivation(); // �ڵ� �ߵ� ó��
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
            Debug.Log($"[SkillEquipSlotManager] ���� {slotIndex}�� ������Ʈ�Ǿ����ϴ�. ���� ����: {isEquipped}");
        }

        if (!isEquipped)
        {
            foreach (var slot in equipSlots)
            {
                if (slot.GetEquippedSkill() == skill)
                {
                    slot.UpdateSlot(null, false);
                    Debug.Log($"[SkillEquipSlotManager] ��ų {skill.SkillData.itemName}�� ���ŵǾ����ϴ�.");
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
                Debug.Log($"[SkillEquipSlotManager] �ڵ� �ߵ�: {skill.SkillData.itemName}");
            }
        }
    }

    public void ToggleAutoActivate()
    {
        autoActivate = !autoActivate;
        UpdateAutoActivateButtonUI();
        Debug.Log($"[SkillEquipSlotManager] �ڵ� �ߵ� ����: {autoActivate}");
    }

    private void UpdateAutoActivateButtonUI()
    {
        if (autoActivateAnimator != null)
        {
            // �ִϸ����� Ʈ���� �Ǵ� ���� ����
            autoActivateAnimator.SetBool("IsActive", autoActivate);
        }

        if (autoActivateIndicator != null)
        {
            // ��Ȱ��ȭ ���¿����� ���� ����
            autoActivateIndicator.color = autoActivate ? Color.white : inactiveColor;
        }
    }
}
