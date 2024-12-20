using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, IEnhanceable
{
    protected SkillDataSO skillData;
    public SkillDataSO SkillData => skillData;
    public BaseItemDataSO BaseData => skillData;
    public int EnhancementLevel { get; protected set; } = 1;
    public int RequiredCurrencyForUpgrade => skillData.requiredCurrencyForUpgrade;
    public int StackCount { get; private set; } = 1;
    public float RemainingCooldown
    {
        get
        {
            if (skillData.activationCondition == ActivationCondition.Cooldown)
            {
                return Mathf.Max(0, cooldownTimer);
            }
            else if (skillData.activationCondition == ActivationCondition.HitBased)
            {
                // 남은 히트 횟수 반환 (UI 표시용)
                return skillData.requiredHits - currentHits;
            }

            return 0;
        }
    }
    public int CurrentHits => currentHits;

    protected float cooldownTimer = 0f;
    protected float activeTimer = 0f;
    protected int currentHits = 0;
    protected PlayerStat playerStat;
    protected bool isActivated = false;
    private int requireSkillCardsForUpgrade;
    private bool isEquipped;
    private PlayerBattle playerBattle;

    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            if (isEquipped != value)
            {
                isEquipped = value;
            }
        }
    }

    public virtual void Initialize(SkillDataSO data, PlayerStat stat)
    {
        skillData = data;
        playerStat = stat;

        Debug.Log($"[BaseSkill] {skillData.itemName} 초기화, 쿨타임: {skillData.cooldown}");

        requireSkillCardsForUpgrade = skillData.requireSkillCardsForUpgrade;
        isEquipped = skillData.isEquipped;

        if (skillData.activationCondition == ActivationCondition.HitBased)
        {
            playerBattle = PlayerObjManager.Instance?.Player?.PlayerBattle;
            if (playerBattle != null)
                playerBattle.OnPlayerAttack += RegisterHit;
        }
    }
    private void OnDestroy()
    {
        if (playerBattle != null)
            playerBattle.OnPlayerAttack -= RegisterHit;
    }

    public virtual void Update(){}

    public bool IsReadyToActivate()
    {
        if (skillData.activationCondition == ActivationCondition.Cooldown)
        {
            return cooldownTimer <= 0;
        }
        else if (skillData.activationCondition == ActivationCondition.HitBased)
        {
            return currentHits >= skillData.requiredHits;
        }

        return false;
    }

    public bool ConsumeMana()
    {
        if (playerStat.mana >= skillData.manaCost)
        {
            playerStat.reduceMana(skillData.manaCost);
            return true;
        }
        return false;
    }

    public void RegisterHit()
    {
        if (skillData.activationCondition == ActivationCondition.HitBased)
            currentHits++;
    }

    protected void ResetCondition()
    {
        Debug.Log($"[BaseSkill] ResetCondition 호출 전, 쿨타임: {cooldownTimer}, 히트: {currentHits}");

        if (skillData.activationCondition == ActivationCondition.Cooldown)
        {
            cooldownTimer = skillData.cooldown; // 쿨타임 초기화
        }
        else if (skillData.activationCondition == ActivationCondition.HitBased)
        {
            currentHits = 0; // 히트 초기화
        }

        Debug.Log($"[BaseSkill] ResetCondition 호출 후, 쿨타임: {cooldownTimer}, 히트: {currentHits}");
    }

    public virtual SkillEffect GetSkillEffect(Vector3 targetPosition)
    {
        return new SkillEffect(
            skillData.effectPrefab,
            skillData.damagePercent,
            skillData.buffDuration,
            skillData.effectRange,
            skillData.effectType,
            targetPosition
        );
    }

    public abstract void Activate(Vector3 targetPosition);

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald) >= RequiredCurrencyForUpgrade &&
               PlayerObjManager.Instance.Player.inventory.GetItemStackCount(this) >= skillData.requireSkillCardsForUpgrade + 1;
    }

    public bool Enhance()
    {
        if (!CanEnhance()) return false;

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, RequiredCurrencyForUpgrade);
        PlayerObjManager.Instance.Player.inventory.RemoveItemFromInventory(skillData, skillData.requireSkillCardsForUpgrade);

        EnhancementLevel++;
        requireSkillCardsForUpgrade++;

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

    public void DecreaseCooldown(float deltaTime)
    {
        if (cooldownTimer > 0)
            cooldownTimer = Mathf.Max(0, cooldownTimer - deltaTime);

        if (isActivated && activeTimer > 0)
            activeTimer = Mathf.Max(0, activeTimer - deltaTime);

        if (isActivated && activeTimer <= 0 && !(this is ActiveSkill))
            EndEffect();
    }

    protected virtual void EndEffect()
    {
        isActivated = false;
    }
}