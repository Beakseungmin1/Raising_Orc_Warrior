using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public List<Skill> equippedSkills;
    private float[] cooldownTimers;
    private int[] hitCounters; // 누락된 hitCounters 추가

    public SkillEffectManager skillEffectManager;

    private EquipManager equipManager;
    private PlayerStat playerStat;

    private void Start()
    {
        equipManager = PlayerobjManager.Instance.Player.GetComponent<EquipManager>();
        playerStat = PlayerobjManager.Instance.Player.GetComponent<PlayerStat>();

        if (equipManager == null || playerStat == null)
        {
            Debug.LogError("PlayerSkillHandler: EquipManager 또는 PlayerStat를 찾을 수 없습니다.");
            return;
        }

        InitializeSkillHandler();
        SyncWithEquipManager();
    }

    private void InitializeSkillHandler()
    {
        equippedSkills = new List<Skill>();
        cooldownTimers = new float[0];
        hitCounters = new int[0]; // hitCounters 초기화
    }

    public void SyncWithEquipManager()
    {
        equippedSkills = equipManager.GetAllEquippedSkills();

        cooldownTimers = new float[equippedSkills.Count];
        hitCounters = new int[equippedSkills.Count]; // hitCounters 배열 동기화

        for (int i = 0; i < equippedSkills.Count; i++)
        {
            cooldownTimers[i] = 0f;
            hitCounters[i] = 0; // 각 스킬의 hitCounters 초기화

            if (equippedSkills[i] != null)
            {
                Debug.Log($"PlayerSkillHandler: 스킬 {equippedSkills[i].BaseData.itemName}이(가) 장착되었습니다.");
            }
            else
            {
                Debug.LogWarning($"PlayerSkillHandler: EquippedSkills[{i}]가 null입니다.");
            }
        }

        Debug.Log($"PlayerSkillHandler: EquipManager와 동기화 완료. 장착된 스킬 수: {equippedSkills.Count}");
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
            Debug.LogWarning("PlayerSkillHandler: 발동할 스킬이 없습니다.");
            return;
        }

        int skillIndex = equippedSkills.IndexOf(skill);
        if (skillIndex < 0 || cooldownTimers[skillIndex] > 0)
        {
            Debug.Log($"PlayerSkillHandler: 스킬 {skill.BaseData.itemName}은 쿨다운 중입니다.");
            return;
        }

        if (skill.BaseData.manaCost > playerStat.GetMana())
        {
            Debug.Log($"PlayerSkillHandler: 스킬 {skill.BaseData.itemName} 사용 실패! 마나 부족.");
            return;
        }

        skillEffectManager.TriggerEffect(skill, targetPosition);

        // 스킬 사용 후 hitCounters 증가
        hitCounters[skillIndex]++;
        Debug.Log($"스킬 {skill.BaseData.itemName} 사용 완료! 현재 hitCounters: {hitCounters[skillIndex]}");

        cooldownTimers[skillIndex] = skill.BaseData.cooldown;
        playerStat.reduceMana(skill.BaseData.manaCost);
    }

    public int GetHitCount(Skill skill)
    {
        int skillIndex = equippedSkills.IndexOf(skill);
        if (skillIndex < 0)
        {
            Debug.LogError($"PlayerSkillHandler: 스킬 {skill.BaseData.itemName}은 장착되지 않았습니다.");
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
            Debug.Log($"스킬 {skill.BaseData.itemName}의 hitCounters 초기화 완료.");
        }
    }
}