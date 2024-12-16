using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillDataSO skillData; // 스킬 데이터 참조
    protected float cooldownTimer; // 쿨다운 타이머
    protected int currentHits; // 현재 공격 횟수 트래킹

    public virtual void Initialize(SkillDataSO data)
    {
        skillData = data;
        cooldownTimer = 0f;
        currentHits = 0;
    }

    public virtual void UpdateSkill()
    {
        if (skillData.activationCondition == ActivationCondition.Cooldown && cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool IsReadyToActivate()
    {
        switch (skillData.activationCondition)
        {
            case ActivationCondition.Cooldown:
                return cooldownTimer <= 0;
            case ActivationCondition.HitBased:
                return currentHits >= skillData.requiredHits;
            case ActivationCondition.Periodic:
                return true; // 주기적 발동은 Update에서 처리
            default:
                return false;
        }
    }

    public void RegisterHit()
    {
        if (skillData.activationCondition == ActivationCondition.HitBased)
        {
            currentHits++;
        }
    }

    public void ResetCondition()
    {
        cooldownTimer = skillData.cooldown;
        currentHits = 0;
    }

    public abstract void Activate(Vector3 targetPosition);
}
