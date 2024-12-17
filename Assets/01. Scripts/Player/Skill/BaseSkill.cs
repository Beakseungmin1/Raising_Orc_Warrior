using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, IFusable
{
    protected SkillDataSO skillData;
    public SkillDataSO SkillData => skillData;

    public BaseItemDataSO BaseData => skillData;
    public int EnhancementLevel { get; protected set; } = 0;
    public int RequiredCurrencyForUpgrade => skillData.requiredCurrencyForUpgrade;
    public int StackCount { get; private set; } = 1;

    protected float cooldownTimer = 0f;
    protected int currentHits = 0;
    protected PlayerStat playerStat;

    public virtual void Initialize(SkillDataSO data, PlayerStat stat)
    {
        skillData = data;
        playerStat = stat;
    }

    public virtual void Update()
    {
        if (skillData.activationCondition == ActivationCondition.Cooldown && cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public bool IsReadyToActivate()
    {
        return skillData.activationCondition switch
        {
            ActivationCondition.Cooldown => cooldownTimer <= 0,
            ActivationCondition.HitBased => currentHits >= skillData.requiredHits,
            _ => false,
        };
    }

    public bool ConsumeMana()
    {
        if (playerStat.mana >= skillData.manaCost)
        {
            playerStat.reduceMana(skillData.manaCost);
            return true;
        }
        Debug.Log($"{skillData.itemName} 발동 실패: 마나 부족!");
        return false;
    }

    public void RegisterHit()
    {
        if (skillData.activationCondition == ActivationCondition.HitBased)
            currentHits++;
    }

    protected void ResetCondition()
    {
        if (skillData.activationCondition == ActivationCondition.Cooldown)
            cooldownTimer = skillData.cooldown;
        else if (skillData.activationCondition == ActivationCondition.HitBased)
            currentHits = 0;
    }

    public virtual SkillEffect GetSkillEffect(Vector3 targetPosition)
    {
        return new SkillEffect(
            skillData.effectPrefab,
            skillData.damagePercent + EnhancementLevel * 0.05f,
            skillData.buffDuration + EnhancementLevel * 0.2f,
            skillData.effectRange,
            skillData.effectType,
            targetPosition
        );
    }

    public abstract void Activate(Vector3 targetPosition);

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald) >= RequiredCurrencyForUpgrade && StackCount > 0;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("강화 조건이 충족되지 않았습니다.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, RequiredCurrencyForUpgrade);
        EnhancementLevel++;
        Debug.Log($"스킬 {skillData.itemName}이(가) {EnhancementLevel}레벨로 강화되었습니다.");
        return true;
    }

    public void AddStack(int count)
    {
        StackCount += count;
    }

    public void RemoveStack(int count)
    {
        StackCount = Mathf.Max(0, StackCount - count);
    }

    public bool Fuse(int materialCount)
    {
        if (StackCount < materialCount)
        {
            Debug.LogWarning("스택이 부족하여 합성할 수 없습니다.");
            return false;
        }

        StackCount -= materialCount;
        Debug.Log($"스킬 {skillData.itemName}이(가) 합성되었습니다. 남은 스택: {StackCount}");
        return true;
    }
}