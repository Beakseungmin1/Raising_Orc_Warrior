using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public void TriggerSkill(SkillSO skill, PlayerSkill playerSkill, Vector3 position)
    {
        if (skill == null || playerSkill == null) return;

        if (skill.skillType == SkillSO.SkillType.Active)
        {
            playerSkill.ExecuteActiveSkill(skill, position);
        }
        else if (skill.skillType == SkillSO.SkillType.Buff)
        {
            playerSkill.ApplyBuff(skill);
        }
        else if (skill.skillType == SkillSO.SkillType.Passive)
        {
            playerSkill.TriggerPassiveSkill(skill);
        }
    }
}