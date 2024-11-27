using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public void ExecuteSkill(SkillSO skill, Vector3 position)
    {
        if (skill == null)
        {
            return;
        }

        if (skill.skillEffectPrefab != null)
        {
            Instantiate(skill.skillEffectPrefab, position, Quaternion.identity);
        }
    }
}