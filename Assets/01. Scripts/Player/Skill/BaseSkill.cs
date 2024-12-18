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
        Debug.Log($"{skillData.itemName} �ߵ� ����: ���� ����!");
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
            skillData.damagePercent, // ��ȭ �� ���� ���� ����
            skillData.buffDuration, // ��ȭ �� ���� ���� �ð� ����
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
            Debug.LogWarning("��ȭ ������ �������� �ʾҽ��ϴ�.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, RequiredCurrencyForUpgrade);
        PlayerObjManager.Instance.Player.inventory.RemoveItemFromInventory(skillData, requireSkillCardsForUpgrade);

        EnhancementLevel++;
        requireSkillCardsForUpgrade++;

        Debug.Log($"��ȭ �Ϸ�! ���� ����: {EnhancementLevel}, ���� ��ȭ �ʿ� ī��: {requireSkillCardsForUpgrade}");

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