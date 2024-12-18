using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, IEnhanceable
{
    protected SkillDataSO skillData;
    public SkillDataSO SkillData => skillData;
    public BaseItemDataSO BaseData => skillData;
    public int EnhancementLevel { get; protected set; } = 1;
    public int RequiredCurrencyForUpgrade => skillData.requiredCurrencyForUpgrade;
    public int StackCount { get; private set; } = 1;

    protected float cooldownTimer = 0f;
    protected int currentHits = 0;
    protected PlayerStat playerStat;
    protected bool isActivated = false;
    private int requireSkillCardsForUpgrade;

    public virtual void Initialize(SkillDataSO data, PlayerStat stat)
    {
        skillData = data;
        playerStat = stat;

        requireSkillCardsForUpgrade = skillData.requireSkillCardsForUpgrade;
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
            skillData.damagePercent, // 강화 시 피해 비율 고정
            skillData.buffDuration, // 강화 시 버프 지속 시간 고정
            skillData.effectRange,
            skillData.effectType,
            targetPosition
        );
    }

    public abstract void Activate(Vector3 targetPosition);

    public bool CanEnhance()
    {
        int requiredSkillCount = requireSkillCardsForUpgrade;
        int availableSkillCount = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(this);

        return CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald) >= RequiredCurrencyForUpgrade
               && availableSkillCount >= requiredSkillCount + 1;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("강화 조건이 충족되지 않았습니다.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, RequiredCurrencyForUpgrade);
        PlayerObjManager.Instance.Player.inventory.RemoveItemFromInventory(skillData, requireSkillCardsForUpgrade);

        EnhancementLevel++;
        requireSkillCardsForUpgrade++;

        Debug.Log($"강화 완료! 현재 레벨: {EnhancementLevel}, 다음 강화 필요 카드: {requireSkillCardsForUpgrade}");

        EnhanceSkill();

        PlayerObjManager.Instance.Player.inventory.NotifySkillsChanged();

        return true;
    }

    protected abstract void EnhanceSkill();

    public void AddStack(int count)
    {
        StackCount += count;
    }

    public void RemoveStack(int count)
    {
        StackCount = Mathf.Max(0, StackCount - count);
    }

    public int GetRuntimeRequiredSkillCards()
    {
        return requireSkillCardsForUpgrade;
    }
}