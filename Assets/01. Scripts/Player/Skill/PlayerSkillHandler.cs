using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private List<BaseSkill> equippedSkills; // 장착된 스킬 목록
    private EquipManager equipManager;
    private PlayerStat playerStat;

    private void Start()
    {
        // EquipManager와 PlayerStat 가져오기
        equipManager = PlayerObjManager.Instance?.Player?.GetComponent<EquipManager>();
        playerStat = PlayerObjManager.Instance?.Player?.GetComponent<PlayerStat>();

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

    /// <summary>
    /// EquipManager와 동기화하여 장착된 스킬 목록 초기화
    /// </summary>
    public void SyncWithEquipManager()
    {
        equippedSkills = equipManager.GetAllEquippedSkills();

        // 각 스킬 초기화
        foreach (var skill in equippedSkills)
        {
            if (skill != null)
                skill.Initialize(skill.SkillData, playerStat);
        }
    }

    /// <summary>
    /// 모든 장착된 스킬의 Update 실행
    /// </summary>
    private void UpdateSkills()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill != null)
                skill.Update();
        }
    }

    /// <summary>
    /// 스킬 사용 (장착된 스킬만 발동 가능)
    /// </summary>
    public void UseSkill(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null || !IsSkillEquipped(skill))
        {
            Debug.LogWarning($"{skill?.SkillData.itemName} 스킬은 장착되지 않았거나 유효하지 않습니다.");
            return;
        }

        if (!skill.IsReadyToActivate())
        {
            Debug.LogWarning($"{skill.SkillData.itemName} 스킬은 아직 준비되지 않았습니다.");
            return;
        }

        // 스킬 발동
        skill.Activate(targetPosition);
    }

    /// <summary>
    /// 특정 스킬의 히트 등록
    /// </summary>
    public void RegisterHit(BaseSkill skill)
    {
        if (skill == null || !IsSkillEquipped(skill))
        {
            Debug.LogWarning("장착되지 않은 스킬에는 히트를 등록할 수 없습니다.");
            return;
        }

        skill.RegisterHit();
    }

    /// <summary>
    /// 해당 스킬이 장착되었는지 확인
    /// </summary>
    private bool IsSkillEquipped(BaseSkill skill)
    {
        return equippedSkills.Contains(skill);
    }
}