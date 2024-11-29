using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public void TriggerSkill(SkillDataSO skill, PlayerSkill playerSkill, Vector3 position)
    {
        if (skill == null || playerSkill == null) return;

        if (skill.skillType == SkillType.Active)
        {
            playerSkill.ExecuteActiveSkill(skill, position);
        }
        else if (skill.skillType == SkillType.Buff)
        {
            playerSkill.ApplyBuff(skill);
        }
        else if (skill.skillType == SkillType.Passive)
        {
            playerSkill.TriggerPassiveSkill(skill);
        }
    }
}