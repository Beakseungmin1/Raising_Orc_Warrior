using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillDataSO skillData; // ��ų ������ ����
    protected float cooldownTimer; // ��ٿ� Ÿ�̸�
    protected int currentHits; // ���� ���� Ƚ�� Ʈ��ŷ

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
                return true; // �ֱ��� �ߵ��� Update���� ó��
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
