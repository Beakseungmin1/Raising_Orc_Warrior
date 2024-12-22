using System.Collections.Generic;
using UnityEngine;

public class SkillEquipSlotManager : UIBase
{
    [SerializeField] private GameObject skillSlotPrefab;
    [SerializeField] private Transform contentParent;

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
}