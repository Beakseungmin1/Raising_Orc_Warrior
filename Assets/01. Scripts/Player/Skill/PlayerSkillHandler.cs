using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public List<Skill> equippedSkills; // �÷��̾ ������ ��ų
    private float[] cooldownTimers;    // �� ��ų�� ��ٿ� ����
    private int[] hitCounters;         // ���� ��� ��ų�� ���� ī��Ʈ

    public SkillEffectManager skillEffectManager; // ����Ʈ ���� �Ŵ���

    private void Start()
    {
        //UpdateEquippedSkills(); // �ʱ�ȭ
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    //private void UpdateEquippedSkills()
    //{
    //    equippedSkills = new List<Skill>(); // �ʱ�ȭ

    //    var inventorySkills = PlayerManager.Instance.player.inventory.SkillInventory.GetEquippedSkills();

    //    foreach (var skillData in inventorySkills)
    //    {
    //        equippedSkills.Add(new Skill(skillData));
    //    }

    //    cooldownTimers = new float[equippedSkills.Count];
    //    hitCounters = new int[equippedSkills.Count];

    //    for (int i = 0; i < equippedSkills.Count; i++)
    //    {
    //        cooldownTimers[i] = 0f;
    //        hitCounters[i] = 0;
    //    }

    //    Debug.Log($"��ų ����Ʈ�� ������Ʈ�Ǿ����ϴ�. ���� ������ ��ų ��: {equippedSkills.Count}");
    //}

    private void UpdateCooldowns()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= Time.deltaTime;
        }
    }

    public void RegisterHit()
    {
        for (int i = 0; i < equippedSkills.Count; i++)
        {
            if (equippedSkills[i].BaseData.activationCondition == ActivationCondition.HitBased)
            {
                hitCounters[i]++;
                if (hitCounters[i] >= equippedSkills[i].BaseData.requiredHits)
                {
                    UseSkill(i, transform.position);
                    hitCounters[i] = 0;
                }
            }
        }
    }

    public void UseSkill(int skillIndex, Vector3 targetPosition)
    {
        if (skillIndex < 0 || skillIndex >= equippedSkills.Count) return;

        Skill skill = equippedSkills[skillIndex];
        SkillDataSO skillData = skill.BaseData;

        // ��ٿ� Ȯ��
        if (skillData.cooldown > 0 && cooldownTimers[skillIndex] > 0)
        {
            Debug.Log($"��ų {skillData.itemName} ��ٿ� ���Դϴ�.");
            return;
        }

        // ���� Ȯ��
        if (skillData.manaCost > 0 && !HasEnoughMana(skillData.manaCost))
        {
            Debug.Log($"��ų {skillData.itemName} ��� ����! ������ �����մϴ�.");
            return;
        }

        // ��ų �ߵ�
        skillEffectManager.TriggerEffect(skill, targetPosition);
        cooldownTimers[skillIndex] = skillData.cooldown;

        if (skillData.manaCost > 0)
            ConsumeMana(skillData.manaCost);
    }

    private bool HasEnoughMana(int manaCost)
    {
        // ���� Ȯ�� ����
        return true; // TODO: ���� ���� ����
    }

    private void ConsumeMana(int manaCost)
    {
        // ���� �Ҹ� ����
        Debug.Log($"���� {manaCost} �Ҹ�");
    }
}