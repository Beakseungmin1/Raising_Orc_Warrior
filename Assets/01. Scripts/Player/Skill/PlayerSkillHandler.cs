using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private List<BaseSkill> equippedSkills;
    private EquipManager equipManager;
    private PlayerStat playerStat;

    private void Start()
    {
        equipManager = PlayerObjManager.Instance?.Player?.EquipManager;
        playerStat = PlayerObjManager.Instance?.Player?.stat;

        if (equipManager == null || playerStat == null) return;

        SyncWithEquipManager();
    }

    private void Update()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill == null) continue;

            skill.DecreaseCooldown(Time.deltaTime);

            if (skill is PassiveSkill && skill.IsReadyToActivate())
            {
                Debug.Log($"[PlayerSkillHandler] 패시브 스킬 자동 발동: {skill.SkillData.itemName}");
                skill.Activate(Vector3.zero);
            }
        }
    }

    public void SyncWithEquipManager()
    {
        equippedSkills = equipManager.GetAllEquippedSkills();

        foreach (var skill in equippedSkills)
        {
            if (skill != null)
                skill.Initialize(skill.SkillData, playerStat);
        }
    }

    public void UseSkill(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null || !IsSkillEquipped(skill)) return;

        if (!skill.IsReadyToActivate()) return;

        if (skill is BuffSkill)
        {
            ActivateBuffSkill(skill);
        }
        else if (skill is ActiveSkill)
        {
            ActivateActiveSkill(skill, targetPosition);
        }
        else
        {
            Debug.LogWarning($"[PlayerSkillHandler] 알 수 없는 스킬 타입: {skill.SkillData.itemName}");
        }
    }

    private void ActivateBuffSkill(BaseSkill skill)
    {
        Debug.Log($"[PlayerSkillHandler] 버프 스킬 발동: {skill.SkillData.itemName}");
        skill.Activate(Vector3.zero);
    }

    private void ActivateActiveSkill(BaseSkill skill, Vector3 targetPosition)
    {
        Debug.Log($"[PlayerSkillHandler] 액티브 스킬 발동: {skill.SkillData.itemName}, 쿨타임: {skill.RemainingCooldown}");
        skill.Activate(targetPosition);
        Debug.Log($"[PlayerSkillHandler] 스킬 발동 후 쿨타임: {skill.RemainingCooldown}");
    }

    public void RegisterHit(BaseSkill skill)
    {
        if (skill == null || !IsSkillEquipped(skill)) return;

        skill.RegisterHit();
    }

    private bool IsSkillEquipped(BaseSkill skill)
    {
        return equippedSkills.Exists(s => s.SkillData == skill.SkillData);
    }
}