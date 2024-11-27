using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public List<SkillSO> skills;
    private float[] cooldownTimers;

    void Start()
    {
        InitializeCooldowns();
    }

    void Update()
    {
        UpdateCooldowns();
    }

    private void InitializeCooldowns()
    {
        if (skills == null || skills.Count == 0) return;

        cooldownTimers = new float[skills.Count];
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            cooldownTimers[i] = 0f;
        }
    }

    private void UpdateCooldowns()
    {
        if (cooldownTimers == null) return;

        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
            }
        }
    }

    public bool CanUseSkill(int skillIndex)
    {
        return skillIndex >= 0 && skillIndex < skills.Count && cooldownTimers[skillIndex] <= 0;
    }

    public void UseSkill(int skillIndex, Vector3 position)
    {
        if (!CanUseSkill(skillIndex)) return;

        SkillSO skill = skills[skillIndex];
        SkillManager.Instance.ExecuteSkill(skill, position);

        cooldownTimers[skillIndex] = skill.cooldown;
    }
}