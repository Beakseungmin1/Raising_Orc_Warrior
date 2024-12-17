using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public List<BaseSkill> equippedSkills; // 장착된 스킬 목록
    private EquipManager equipManager;
    private PlayerStat playerStat;

    private void Start()
    {
        equipManager = PlayerObjManager.Instance.Player.GetComponent<EquipManager>();
        playerStat = PlayerObjManager.Instance.Player.GetComponent<PlayerStat>();

        if (equipManager == null || playerStat == null)
        {
            Debug.LogError("PlayerSkillHandler: EquipManager나 PlayerStat을 찾을 수 없습니다.");
            return;
        }

        SyncWithEquipManager();
    }

    private void Update()
    {
        UpdateSkills();
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

    private void UpdateSkills()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill != null)
                skill.Update();
        }
    }

    public void UseSkill(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null || !skill.IsReadyToActivate())
        {
            Debug.Log($"{skill?.SkillData.itemName} 스킬은 아직 준비되지 않았습니다.");
            return;
        }

        skill.Activate(targetPosition);
    }

    public void RegisterHit(BaseSkill skill)
    {
        if (skill == null) return;

        skill.RegisterHit();
    }
}