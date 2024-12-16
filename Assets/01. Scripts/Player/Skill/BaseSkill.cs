using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillDataSO skillData; // ��ų ������ ����
    protected float cooldownTimer;
    protected int currentHits;

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

    protected bool ConsumeMana()
    {
        // PlayerStat���� ���� ���� üũ
        PlayerStat playerStat = PlayerObjManager.Instance.Player.GetComponent<PlayerStat>();
        if (playerStat == null)
        {
            Debug.LogError("PlayerStat ������Ʈ�� ã�� �� �����ϴ�!");
            return false;
        }

        if (playerStat.mana < skillData.manaCost)
        {
            Debug.Log("������ �����մϴ�.");
            return false;
        }

        playerStat.reduceMana(skillData.manaCost); // ���� �Ҹ�
        return true;
    }

    // ��ų ȿ���� �����ϴ� �Լ�
    public abstract SkillEffect GetSkillEffect();

    public abstract void Activate(Vector3 targetPosition);
}