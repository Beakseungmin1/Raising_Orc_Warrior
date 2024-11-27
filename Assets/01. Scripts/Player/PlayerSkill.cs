using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public List<SkillSO> skills;
    private float[] cooldownTimers;
    private int[] hitCounters;

    void Start()
    {
        InitializeSkills();
    }

    void Update()
    {
        UpdateCooldowns();
        UpdatePassiveSkills();
    }

    private void InitializeSkills()
    {
        if (skills == null || skills.Count == 0) return;

        cooldownTimers = new float[skills.Count];
        hitCounters = new int[skills.Count];

        for (int i = 0; i < skills.Count; i++)
        {
            cooldownTimers[i] = 0f;
            hitCounters[i] = 0;
        }
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            SkillSO skill = skills[i];

            if (skill.activationCondition == SkillSO.ActivationCondition.Cooldown && cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
            }
        }
    }

    private void UpdatePassiveSkills()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            SkillSO skill = skills[i];

            if (skill.skillType == SkillSO.SkillType.Passive &&
                skill.activationCondition == SkillSO.ActivationCondition.Cooldown &&
                cooldownTimers[i] <= 0)
            {
                ExecutePassiveSkill(skill);
                cooldownTimers[i] = skill.cooldown;
            }
        }
    }

    public bool CanUseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Count) return false;

        SkillSO skill = skills[skillIndex];

        if (skill.activationCondition == SkillSO.ActivationCondition.Cooldown)
        {
            return cooldownTimers[skillIndex] <= 0 && HasEnoughMana(skill.manaCost);
        }
        else if (skill.activationCondition == SkillSO.ActivationCondition.HitBased)
        {
            return hitCounters[skillIndex] >= skill.requiredHits && HasEnoughMana(skill.manaCost);
        }

        return false;
    }

    public void ExecuteActiveSkill(SkillSO skill, Vector3 position)
    {
        if (skill == null) return;

        Collider[] hitEnemies = Physics.OverlapSphere(position, skill.effectRange);
        foreach (var enemy in hitEnemies)
        {
            // 적에게 데미지 처리
        }

        ConsumeMana(skill.manaCost);
    }

    public void ApplyBuff(SkillSO skill)
    {
        StartCoroutine(ApplyBuffCoroutine(skill));
    }

    private IEnumerator ApplyBuffCoroutine(SkillSO skill)
    {
        // 공격력 증가 구현
        yield return new WaitForSeconds(skill.buffDuration);
    }

    public void ExecutePassiveSkill(SkillSO skill)
    {
        // 패시브 스킬 효과 구현
    }

    private bool HasEnoughMana(int manaCost)
    {
        // 마나 확인 로직
        return true;
    }

    private void ConsumeMana(int manaCost)
    {
        // 마나 소모 로직
    }

    public void IncrementHitCounter(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= hitCounters.Length) return;

        SkillSO skill = skills[skillIndex];
        if (skill.activationCondition == SkillSO.ActivationCondition.HitBased)
        {
            hitCounters[skillIndex]++;
        }
    }
}