using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlot : UIBase
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Button slotButton;

    private BaseSkill equippedSkill; // Skill → BaseSkill
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

    public void EquipSkill(BaseSkill skill) // Skill → BaseSkill
    {
        equippedSkill = skill;
        skillIcon.sprite = skill?.SkillData.icon;

        UpdateSlotColor(equipped: skill != null);

        Debug.Log($"[SkillEquipSlot] 슬롯 {slotIndex}에 {equippedSkill?.SkillData.itemName ?? "스킬 없음"}이 장착되었습니다.");
    }

    public BaseSkill GetEquippedSkill() // 반환 타입 수정
    {
        return equippedSkill;
    }

    private void OnClickSlot()
    {
        if (slotManager.HasSkillToEquip())
        {
            Debug.Log($"[SkillEquipSlot] 장착 대기 상태에서 슬롯 {slotIndex} 클릭으로 장착이 우선 처리됩니다.");
            slotManager.TryEquipSkillToSlot(slotIndex);
        }
        else if (equippedSkill != null)
        {
            Debug.Log($"[SkillEquipSlot] {equippedSkill.SkillData.itemName} 스킬 발동!");
            ActivateSkill();
        }
        else
        {
            Debug.Log($"[SkillEquipSlot] 슬롯 {slotIndex}에 스킬이 없습니다.");
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