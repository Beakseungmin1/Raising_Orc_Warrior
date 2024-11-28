using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public List<SkillSO> skills;
    private float[] cooldownTimers;
    private bool[] isBuffActive;

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
        isBuffActive = new bool[skills.Count];

        for (int i = 0; i < skills.Count; i++)
        {
            cooldownTimers[i] = 0f;
            isBuffActive[i] = false;
        }
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            SkillSO skill = skills[i];

            if (skill.activationCondition == ActivationCondition.Cooldown && cooldownTimers[i] > 0)
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

            if (skill.skillType == SkillType.Passive && skill.activationCondition == ActivationCondition.Cooldown)
            {
                if (cooldownTimers[i] <= 0 && !isBuffActive[i])
                {
                    StartCoroutine(ApplyPassiveSkill(skill, i));
                }
            }
        }
    }

    public bool CanUseSkill(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skills.Count) return false;

        SkillSO skill = skills[skillIndex];

        if (skill.activationCondition == ActivationCondition.Cooldown)
        {
            return cooldownTimers[skillIndex] <= 0 && HasEnoughMana(skill.manaCost);
        }

        return false;
    }

    public void ExecuteActiveSkill(SkillSO skill, Vector3 position)
    {
        if (skill == null) return;

        Collider[] hitEnemies = Physics.OverlapSphere(position, skill.effectRange);
        foreach (var enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = skill.damagePercent;
                //damageable.TakeDamage(damage);
            }
        }

        ConsumeMana(skill.manaCost);
    }

    public void ApplyBuff(SkillSO skill)
    {
        StartCoroutine(ApplyBuffCoroutine(skill));
    }

    private IEnumerator ApplyBuffCoroutine(SkillSO skill)
    {
        // ���� ȿ�� ����
        IncreasePlayerAttack(skill.attackIncreasePercent);

        yield return new WaitForSeconds(skill.buffDuration);

        // ���� ȿ�� ����
        ResetPlayerAttack();
    }

    public void TriggerPassiveSkill(SkillSO skill)
    {
        if (skill == null) return;

        // �нú� ��ų ��� �ߵ� ����
        StartCoroutine(ApplyPassiveSkill(skill, skills.IndexOf(skill)));
    }

    private IEnumerator ApplyPassiveSkill(SkillSO skill, int skillIndex)
    {
        isBuffActive[skillIndex] = true;
        cooldownTimers[skillIndex] = skill.cooldown;

        // �нú� ��ų ȿ�� ����
        IncreasePlayerAttack(skill.attackIncreasePercent);

        yield return new WaitForSeconds(skill.buffDuration);

        isBuffActive[skillIndex] = false;

        // �нú� ȿ�� ����
        ResetPlayerAttack();
    }

    private bool HasEnoughMana(int manaCost)
    {
        // ���� Ȯ�� ����
        return true;
    }

    private void ConsumeMana(int manaCost)
    {
        // ���� �Ҹ� ����
    }

    private void IncreasePlayerAttack(float percent)
    {
        // �÷��̾��� ���ݷ� ����
        // ����: playerStats.attackPower *= (1 + percent / 100);
    }

    private void ResetPlayerAttack()
    {
        // �÷��̾��� ���ݷ� ������� ����
        // ����: playerStats.ResetAttackPower();
    }
}