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

        // 버프 스킬은 바로 발동
        if (skill is BuffSkill)
        {
            ActivateBuffSkill(skill);
            return;
        }

        // 액티브 스킬
        if (skill is ActiveSkill)
        {
            switch (skill.SkillData.effectType)
            {
                case EffectType.OnPlayer:
                case EffectType.OnMapCenter:
                    // 범위 탐지 로직
                    targetPosition = transform.position;
                    Collider2D[] targets = Physics2D.OverlapCircleAll(targetPosition, skill.SkillData.effectRange);

                    bool hasTargetInRange = false;
                    foreach (var target in targets)
                    {
                        if (target.CompareTag("Monster"))
                        {
                            float distance = Vector2.Distance(target.transform.position, targetPosition);
                            if (distance <= skill.SkillData.effectRange)
                            {
                                hasTargetInRange = true;
                            }
                        }
                    }

                    if (!hasTargetInRange)
                    {
                        Debug.Log("PlayerSkillHandler: 범위 내 적이 없습니다. 스킬 발동 취소.");
                        return;
                    }

                    ActivateActiveSkill(skill, targetPosition, targets);
                    break;

                case EffectType.Projectile:
                    // 프로젝타일 스킬은 바로 발사
                    ActivateProjectileSkill(skill, targetPosition);
                    break;

                default:
                    Debug.LogWarning($"PlayerSkillHandler: 알 수 없는 EffectType: {skill.SkillData.effectType}");
                    break;
            }
        }
    }

    private void ActivateBuffSkill(BaseSkill skill)
    {
        Debug.Log($"[PlayerSkillHandler] 버프 스킬 발동: {skill.SkillData.itemName}");
        skill.Activate(Vector3.zero);
    }

    private void ActivateActiveSkill(BaseSkill skill, Vector3 targetPosition, Collider2D[] targets)
    {
        Debug.Log($"[PlayerSkillHandler] 액티브 스킬 발동: {skill.SkillData.itemName}, 쿨타임: {skill.RemainingCooldown}");

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                IEnemy enemy = target.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    skill.Activate(target.transform.position);
                }
            }
        }

        skill.ResetCondition();
    }

    private void ActivateProjectileSkill(BaseSkill skill, Vector3 targetPosition)
    {
        Debug.Log($"[PlayerSkillHandler] 프로젝타일 스킬 발사: {skill.SkillData.itemName}");
        skill.Activate(targetPosition);
        skill.ResetCondition();
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
