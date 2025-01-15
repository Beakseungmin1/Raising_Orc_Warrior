using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private List<BaseSkill> equippedSkills;
    private EquipManager equipManager;
    private PlayerStat playerStat;

    public event Action<BaseSkill> OnSkillUsed;

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
                Debug.Log($"[PlayerSkillHandler] �нú� ��ų �ڵ� �ߵ�: {skill.SkillData.itemName}");
                skill.Activate(Vector3.zero);
                OnSkillUsed.Invoke(skill);
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

        // ���� ��ų�� �ٷ� �ߵ�
        if (skill is BuffSkill)
        {
            ActivateBuffSkill(skill);
            OnSkillUsed?.Invoke(skill);
            return;
        }

        // ��Ƽ�� ��ų
        if (skill is ActiveSkill)
        {
            switch (skill.SkillData.effectType)
            {
                case EffectType.OnPlayer:
                case EffectType.OnMapCenter:
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
                        return;
                    }

                    OnSkillUsed?.Invoke(skill);
                    ActivateActiveSkill(skill, targetPosition, targets);
                    break;

                case EffectType.Projectile:
                    ActivateProjectileSkill(skill, targetPosition);
                    OnSkillUsed?.Invoke(skill);
                    break;

                default:
                    break;
            }
        }
    }

    private void ActivateBuffSkill(BaseSkill skill)
    {
        skill.Activate(Vector3.zero);
    }

    private void ActivateActiveSkill(BaseSkill skill, Vector3 targetPosition, Collider2D[] targets)
    {
        // OnPlayer: ���� �� ù ��° ���� ����
        if (skill.SkillData.effectType == EffectType.OnPlayer)
        {
            foreach (var target in targets)
            {
                if (target.CompareTag("Monster"))
                {
                    IEnemy enemy = target.GetComponent<IEnemy>();
                    if (enemy != null)
                    {
                        skill.Activate(target.transform.position); // ��ų �ߵ�
                        break; // ù ��° ���� ����
                    }
                }
            }
        }
        // OnMapCenter: ���� �� ��� ���� ����
        else if (skill.SkillData.effectType == EffectType.OnMapCenter)
        {
            foreach (var target in targets)
            {
                if (target.CompareTag("Monster"))
                {
                    IEnemy enemy = target.GetComponent<IEnemy>();
                    if (enemy != null)
                    {
                        skill.Activate(target.transform.position); // ��ų �ߵ�
                        break;
                    }
                }
            }
        }

        skill.ResetCondition(); // ��ų ���� �ʱ�ȭ
    }

    private void ActivateProjectileSkill(BaseSkill skill, Vector3 targetPosition)
    {
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