using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    public SkillDataSO skillData; // 스킬 데이터 참조
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

    protected bool ConsumeMana()
    {
        // PlayerStat에서 현재 마나 체크
        PlayerStat playerStat = PlayerObjManager.Instance.Player.GetComponent<PlayerStat>();
        if (playerStat == null)
        {
            Debug.LogError("PlayerStat 컴포넌트를 찾을 수 없습니다!");
            return false;
        }

        if (playerStat.mana < skillData.manaCost)
        {
            Debug.Log("마나가 부족합니다.");
            return false;
        }

        playerStat.reduceMana(skillData.manaCost); // 마나 소모
        return true;
    }

    // 스킬 효과를 전달하는 함수
    public abstract SkillEffect GetSkillEffect();

    public abstract void Activate(Vector3 targetPosition);
}