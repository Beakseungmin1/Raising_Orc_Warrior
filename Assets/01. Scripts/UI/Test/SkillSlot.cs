using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    private SkillDataSO equippedSkill;

    public void EquipSkill(SkillDataSO skill)
    {
        equippedSkill = skill;
        skillIcon.sprite = skill.icon;
    }

    public void OnClickSlot()
    {
        if (equippedSkill != null)
        {
            Debug.Log($"{equippedSkill.itemName} 스킬 발동!");
        }
    }
}