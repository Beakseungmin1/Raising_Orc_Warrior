using System.Collections.Generic;
using UnityEngine;

public class SkillSlotManager : MonoBehaviour
{
    [SerializeField] private List<SkillSlot> skillSlots;
    private SkillDataSO skillToEquip;

    public void PrepareSkillForEquip(SkillDataSO skill)
    {
        skillToEquip = skill;
    }

    public void EquipSkillToSlot(int slotIndex)
    {
        if (skillToEquip != null && slotIndex >= 0 && slotIndex < skillSlots.Count)
        {
            skillSlots[slotIndex].EquipSkill(skillToEquip);
            skillToEquip = null;
        }
    }
}