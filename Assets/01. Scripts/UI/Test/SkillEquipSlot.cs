using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button slotButton;

    private BaseSkill equippedSkill; // Skill �� BaseSkill
    private int slotIndex;
    private SkillEquipSlotManager slotManager;

    private Color defaultColor = new Color32(80, 80, 80, 255);
    private Color equippedColor = Color.white;

    public void InitializeSlot(int index, SkillEquipSlotManager manager)
    {
        slotIndex = index;
        slotManager = manager;

        UpdateSlotColor(equipped: false);

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnClickSlot);
    }

    public void EquipSkill(BaseSkill skill) // Skill �� BaseSkill
    {
        equippedSkill = skill;
        skillIcon.sprite = skill?.SkillData.icon;

        UpdateSlotColor(equipped: skill != null);

        Debug.Log($"[SkillEquipSlot] ���� {slotIndex}�� {equippedSkill?.SkillData.itemName ?? "��ų ����"}�� �����Ǿ����ϴ�.");
    }

    public BaseSkill GetEquippedSkill() // ��ȯ Ÿ�� ����
    {
        return equippedSkill;
    }

    private void OnClickSlot()
    {
        if (slotManager.HasSkillToEquip())
        {
            Debug.Log($"[SkillEquipSlot] ���� ��� ���¿��� ���� {slotIndex} Ŭ������ ������ �켱 ó���˴ϴ�.");
            slotManager.TryEquipSkillToSlot(slotIndex);
        }
        else if (equippedSkill != null)
        {
            Debug.Log($"[SkillEquipSlot] {equippedSkill.SkillData.itemName} ��ų �ߵ�!");
            ActivateSkill();
        }
        else
        {
            Debug.Log($"[SkillEquipSlot] ���� {slotIndex}�� ��ų�� �����ϴ�.");
        }
    }

    private void ActivateSkill()
    {
        if (equippedSkill == null) return;

        var playerSkillHandler = PlayerObjManager.Instance.Player?.GetComponent<PlayerSkillHandler>();
        if (playerSkillHandler != null)
        {
            playerSkillHandler.UseSkill(equippedSkill, transform.position);
        }
    }

    private void UpdateSlotColor(bool equipped)
    {
        skillIcon.color = equipped ? equippedColor : defaultColor;
    }
}