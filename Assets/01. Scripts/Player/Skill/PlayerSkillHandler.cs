using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public List<Skill> equippedSkills; // 플레이어가 장착한 스킬
    private float[] cooldownTimers;    // 각 스킬의 쿨다운 상태
    private int[] hitCounters;         // 공격 기반 스킬의 현재 카운트

    public SkillEffectManager skillEffectManager; // 이펙트 실행 매니저

    private void Start()
    {
        //UpdateEquippedSkills(); // 초기화
    }

    private void Update()
    {
        UpdateCooldowns();
    }

    //private void UpdateEquippedSkills()
    //{
    //    equippedSkills = new List<Skill>(); // 초기화

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

    //    Debug.Log($"스킬 리스트가 업데이트되었습니다. 현재 장착된 스킬 수: {equippedSkills.Count}");
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

        // 쿨다운 확인
        if (skillData.cooldown > 0 && cooldownTimers[skillIndex] > 0)
        {
            Debug.Log($"스킬 {skillData.itemName} 쿨다운 중입니다.");
            return;
        }

        // 마나 확인
        if (skillData.manaCost > 0 && !HasEnoughMana(skillData.manaCost))
        {
            Debug.Log($"스킬 {skillData.itemName} 사용 실패! 마나가 부족합니다.");
            return;
        }

        // 스킬 발동
        skillEffectManager.TriggerEffect(skill, targetPosition);
        cooldownTimers[skillIndex] = skillData.cooldown;

        if (skillData.manaCost > 0)
            ConsumeMana(skillData.manaCost);
    }

    private bool HasEnoughMana(int manaCost)
    {
        // 마나 확인 로직
        return true; // TODO: 실제 로직 구현
    }

    private void ConsumeMana(int manaCost)
    {
        // 마나 소모 로직
        Debug.Log($"마나 {manaCost} 소모");
    }
}