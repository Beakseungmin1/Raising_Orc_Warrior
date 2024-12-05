using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public List<Skill> equippedSkills;
    private float[] cooldownTimers;
    private int[] hitCounters; // ������ hitCounters �߰�

    public SkillEffectManager skillEffectManager;

    private EquipManager equipManager;
    private PlayerStat playerStat;

    private void Start()
    {
        equipManager = PlayerobjManager.Instance.Player.GetComponent<EquipManager>();
        playerStat = PlayerobjManager.Instance.Player.GetComponent<PlayerStat>();

        if (equipManager == null || playerStat == null)
        {
            Debug.LogError("PlayerSkillHandler: EquipManager �Ǵ� PlayerStat�� ã�� �� �����ϴ�.");
            return;
        }

        InitializeSkillHandler();
        SyncWithEquipManager();
    }

    private void InitializeSkillHandler()
    {
        equippedSkills = new List<Skill>();
        cooldownTimers = new float[0];
        hitCounters = new int[0]; // hitCounters �ʱ�ȭ
    }

    public void SyncWithEquipManager()
    {
        equippedSkills = equipManager.GetAllEquippedSkills();

        cooldownTimers = new float[equippedSkills.Count];
        hitCounters = new int[equippedSkills.Count]; // hitCounters �迭 ����ȭ

        for (int i = 0; i < equippedSkills.Count; i++)
        {
            cooldownTimers[i] = 0f;
            hitCounters[i] = 0; // �� ��ų�� hitCounters �ʱ�ȭ

            if (equippedSkills[i] != null)
            {
                Debug.Log($"PlayerSkillHandler: ��ų {equippedSkills[i].BaseData.itemName}��(��) �����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning($"PlayerSkillHandler: EquippedSkills[{i}]�� null�Դϴ�.");
            }
        }

        Debug.Log($"PlayerSkillHandler: EquipManager�� ����ȭ �Ϸ�. ������ ��ų ��: {equippedSkills.Count}");
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    private void UpdateCooldowns()
    {
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= Time.deltaTime;
        }
    }

    public void UseSkill(Skill skill, Vector3 targetPosition)
    {
        if (skill == null)
        {
            Debug.LogWarning("PlayerSkillHandler: �ߵ��� ��ų�� �����ϴ�.");
            return;
        }

        int skillIndex = equippedSkills.IndexOf(skill);
        if (skillIndex < 0 || cooldownTimers[skillIndex] > 0)
        {
            Debug.Log($"PlayerSkillHandler: ��ų {skill.BaseData.itemName}�� ��ٿ� ���Դϴ�.");
            return;
        }

        if (skill.BaseData.manaCost > playerStat.GetMana())
        {
            Debug.Log($"PlayerSkillHandler: ��ų {skill.BaseData.itemName} ��� ����! ���� ����.");
            return;
        }

        skillEffectManager.TriggerEffect(skill, targetPosition);

        // ��ų ��� �� hitCounters ����
        hitCounters[skillIndex]++;
        Debug.Log($"��ų {skill.BaseData.itemName} ��� �Ϸ�! ���� hitCounters: {hitCounters[skillIndex]}");

        cooldownTimers[skillIndex] = skill.BaseData.cooldown;
        playerStat.reduceMana(skill.BaseData.manaCost);
    }

    public int GetHitCount(Skill skill)
    {
        int skillIndex = equippedSkills.IndexOf(skill);
        if (skillIndex < 0)
        {
            Debug.LogError($"PlayerSkillHandler: ��ų {skill.BaseData.itemName}�� �������� �ʾҽ��ϴ�.");
            return 0;
        }
        return hitCounters[skillIndex];
    }

    public void ResetHitCounter(Skill skill)
    {
        int skillIndex = equippedSkills.IndexOf(skill);
        if (skillIndex >= 0)
        {
            hitCounters[skillIndex] = 0;
            Debug.Log($"��ų {skill.BaseData.itemName}�� hitCounters �ʱ�ȭ �Ϸ�.");
        }
    }
}