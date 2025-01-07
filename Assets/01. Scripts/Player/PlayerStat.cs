using System;
using System.Numerics;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{

    public int level { get; private set; }
    public BigInteger exp { get; private set; }
    public BigInteger needExp { get; private set; }
    public float attackPower { get; private set; }
    public BigInteger health { get; private set; }
    public BigInteger maxHealth { get; private set; }
    public BigInteger healthRegeneration { get; private set; }
    public float criticalProbability { get; private set; }
    public float criticalIncreaseDamage { get; private set; }
    public float bluecriticalIncreaseDamage { get; private set; }
    public float bluecriticalProbability { get; private set; }
    public float mana { get; private set; }
    public float maxMana { get; private set; }
    public float manaRegeneration { get; private set; }
    public float hitLate { get; private set; }
    public float avoid { get; private set; }
    public BigInteger extraGoldGainRate { get; private set; }
    public BigInteger extraExpRate { get; private set; }
    public float attackSpeed { get; private set; }
    public float normalMonsterIncreaseDamage { get; private set; }
    public float bossMonsterIncreaseDamage { get; private set; }
    public int attackLevel { get; private set; }
    public int healthLevel { get; private set; }
    public int healthRegenerationLevel { get; private set; }
    public int criticalIncreaseDamageLevel { get; private set; }
    public int criticalProbabilityLevel { get; private set; }
    public int bluecriticalIncreaseDamageLevel { get; private set; }
    public int bluecriticalProbabilityLevel { get; private set; }
    public BigInteger needAttackUpgradeMoney { get; private set; }
    public BigInteger needHealthUpgradeMoney { get; private set; }
    public BigInteger needHealthRegenerationUpgradeMoney { get; private set; }
    public BigInteger needCriticalIncreaseDamageUpgradeMoney { get; private set; }
    public BigInteger needCriticalProbabilityUpgradeMoney { get; private set; }
    public BigInteger needBlueCriticalIncreaseDamageUpgradeMoney { get; private set; }
    public BigInteger needBlueCriticalProbabilityUpgradeMoney { get; private set; }

    public int statUpgradeMultiplier = 0; // 스탯 업그레이드 배율 , 0 = 1배, 1 = 10배, 2 = 100배

    public Action UpdateLevelStatUI;

    public Action UpdateUserInformationUI;

    public Action OnStatChange;

    public Action UpdateAllStatUI;

    [Header("Multiplier")]
    public BigInteger healthMultiplier = 1;
    public BigInteger maxHealthMultiplier = 1;

    private PlayerStatCalculator PlayerStatCalculator;
    private float timer = 0f;

    private void Start()
    {
        PlayerStatCalculator = GetComponent<PlayerStatCalculator>();
    }

    private void Update()
    {
        if (health < PlayerStatCalculator.GetAdjustedMaxHealth())
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                health += PlayerStatCalculator.GetAdjustedHealthRegeneration();

                if (health > PlayerStatCalculator.GetAdjustedMaxHealth())
                {
                    health = PlayerStatCalculator.GetAdjustedMaxHealth();
                }

                timer = 0f;
            }
        }

        if (mana < maxMana)
        {
            mana += manaRegeneration * Time.deltaTime;

            if (mana > maxMana)
            {
                mana = maxMana;
            }
        }

    }

    public void ChangeUpgradeMultiplier(int number)
    {
        if (number >= 0 && number <= 2)
        {
            statUpgradeMultiplier = number;

        }
    }

    private void Awake()
    {
        GameEventsManager.Instance.stageEvents.onStageChange += RefillHpAndMp;
    }

    public void AddExpFromMonsters(IEnemy enemy)
    {
        BigInteger expGained = enemy.GiveExp();
        BigInteger adjustedExp = expGained + (expGained * (extraExpRate / 100));

        exp += adjustedExp;

        while (exp >= needExp)
        {
            LevelUp();
        }

        UpdateLevelStatUI?.Invoke();
    }

    public void AddExp(BigInteger getExp)
    {
        exp += getExp;

        while (exp >= needExp)
        {
            LevelUp();
        }

        UpdateLevelStatUI?.Invoke();
    }

    public void decreaseHp(BigInteger damage)
    {
        health -= damage;
        UpdateUserInformationUI?.Invoke();
    }


    public void LevelUp()
    {
        if (exp >= needExp)
        {
            level++;
            BigInteger curNeedExp = needExp;
            exp -= curNeedExp;
            needExp = needExp * 2;
            UpdateLevelStatUI.Invoke();
            SoundManager.Instance.PlaySFX(SFXType.LevelUp);
            UpdateUserInformationUI?.Invoke();
        }
        else
        {
            Debug.Log("경험치가 부족합니다");
        }
    }


    //스탯업그레이드시 호출되는 메서드 (스탯레벨, 스탯값, 총 업그레이드 비용, 시작스탯, 1레벨당 업그레이드 비용, 1레벨당 업그레이드값)
    public void UpgradeStat(ref int level, ref BigInteger Stat, ref BigInteger totalUpgradeCost, int startStat, int baseCost, int increment)
    {
        int multiplier = statUpgradeMultiplier == 0 ? 1 : (statUpgradeMultiplier == 1 ? 10 : 100);
        BigInteger needUpgradeMoney = baseCost + ((level+1) * baseCost * multiplier); // 레벨당 드는 업그레이드 비용
        totalUpgradeCost = needUpgradeMoney; // 합산한 비용을 스탯당 업그레이드 비용으로 전달

        if (CurrencyManager.Instance.GetGold() >= needUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needUpgradeMoney);
            level += multiplier; // 레벨 증가
            Stat = startStat + (level * increment); // 스탯 업데이트

           OnStatChange?.Invoke(); // 건강 스탯 변경 이벤트 호출
        }
        else
        {
            Debug.Log("골드가 부족합니다");
        }
    }



    public void AttackLevelUp()
    {
        int LevelValue = attackLevel;
        BigInteger PowerValue = (BigInteger)attackPower;
        BigInteger upgradecost = needAttackUpgradeMoney;

        UpgradeStat(ref LevelValue, ref PowerValue, ref upgradecost, 20,1000, 4);

        attackLevel = LevelValue;
        attackPower = (float)PowerValue;
        needAttackUpgradeMoney = upgradecost;
    }

    //스탯업그레이드시 호출되는 메서드 (스탯레벨, 스탯값, 총 업그레이드 비용, 시작스탯, 1레벨당 업그레이드 비용, 1레벨당 업그레이드값)
    public void HealthLevelUp()
    {
        int LevelValue = healthLevel;
        BigInteger PowerValue = maxHealth;
        BigInteger upgradecost = needHealthUpgradeMoney;

        UpgradeStat(ref LevelValue, ref PowerValue, ref upgradecost, 200, 1000, 40);

        healthLevel = LevelValue;
        maxHealth = PowerValue;
        needHealthUpgradeMoney = upgradecost;
    }

    public void HealthRegenerationLevelUp()
    {
        int LevelValue = healthRegenerationLevel;
        BigInteger PowerValue = healthRegeneration;
        BigInteger upgradecost = needHealthRegenerationUpgradeMoney;

        UpgradeStat(ref LevelValue, ref PowerValue, ref upgradecost, 0, 1000, 4);

        healthRegenerationLevel = LevelValue;
        healthRegeneration = PowerValue;
        needHealthRegenerationUpgradeMoney = upgradecost;
    }

    public void CriticalIncreaseDamageLevelUp()
    {
        int LevelValue = criticalIncreaseDamageLevel;
        BigInteger PowerValue = (BigInteger)criticalIncreaseDamage;
        BigInteger upgradecost = needCriticalIncreaseDamageUpgradeMoney;

        UpgradeStat(ref LevelValue, ref PowerValue, ref upgradecost, 100, 1000, 1);

        criticalIncreaseDamageLevel = LevelValue;
        criticalIncreaseDamage = (float)PowerValue;
        needCriticalIncreaseDamageUpgradeMoney = upgradecost;
    }

    public void CriticalProbabilityLevelUp()
    {
        int multiplier = statUpgradeMultiplier == 0 ? 1 : (statUpgradeMultiplier == 1 ? 10 : 100);
        BigInteger needUpgradeMoney = 1000 + ((criticalProbabilityLevel + 1) * 1000 * multiplier); // 레벨당 드는 업그레이드 비용
        needCriticalProbabilityUpgradeMoney = needUpgradeMoney;

        if (CurrencyManager.Instance.GetGold() >= needUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needUpgradeMoney);
            criticalProbabilityLevel += multiplier; // 레벨 증가
            criticalProbability = criticalProbabilityLevel * 0.1f; // 스탯 업데이트

            OnStatChange?.Invoke(); // 건강 스탯 변경 이벤트 호출
        }
        else
        {
            Debug.Log("골드가 부족합니다");
        }
    }

    public void BlueCriticalIncreaseDamageLevelUp()
    {
        needBlueCriticalIncreaseDamageUpgradeMoney = bluecriticalIncreaseDamageLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needBlueCriticalIncreaseDamageUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needBlueCriticalIncreaseDamageUpgradeMoney);
            bluecriticalIncreaseDamage = bluecriticalIncreaseDamageLevel;
            bluecriticalIncreaseDamageLevel++;

            OnStatChange?.Invoke();
        }
        else
        {
            Debug.Log("골드가 부족합니다");
        }
    }

    public void BlueCriticalProbabilityLevelUp()
    {
        needBlueCriticalProbabilityUpgradeMoney = bluecriticalProbabilityLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needBlueCriticalProbabilityUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needBlueCriticalProbabilityUpgradeMoney);
            bluecriticalProbability = bluecriticalProbabilityLevel * 0.1f;
            bluecriticalProbabilityLevel++;

            OnStatChange?.Invoke();
        }
        else
        {
            Debug.Log("골드가 부족합니다");
        }
    }

    public void SetDefaultStat()
    {
        level = 0;
        exp = 0;
        needExp = 100;
        attackPower = 20;
        maxHealth = 200;
        health = maxHealth;
        healthRegeneration = 1;
        criticalProbability = 0;
        criticalIncreaseDamage = 100;
        maxMana = 200;
        mana = maxMana;
        manaRegeneration = 10;
        hitLate = 0;
        avoid = 0;
        extraGoldGainRate = 0;
        extraExpRate = 0;
        attackSpeed = 0;
        normalMonsterIncreaseDamage = 0;
        bossMonsterIncreaseDamage = 0;
        attackLevel = 0;
        healthLevel = 0;
        healthRegenerationLevel = 0;
        criticalIncreaseDamageLevel = 0;
        criticalProbabilityLevel = 0;
        bluecriticalIncreaseDamageLevel = 1;
        bluecriticalProbabilityLevel = 1;
        needAttackUpgradeMoney = 1000;
        needHealthUpgradeMoney = 1000;
        needHealthRegenerationUpgradeMoney = 1000;
        needCriticalIncreaseDamageUpgradeMoney = 1000;
        needCriticalProbabilityUpgradeMoney = 1000;
        needBlueCriticalIncreaseDamageUpgradeMoney = 1000;
        needBlueCriticalProbabilityUpgradeMoney = 1000;
    }

    public float GetMana()
    {
        return mana;
    }

    public void reduceMana(float value)
    {
        mana -= value;

        UpdateUserInformationUI?.Invoke();
    }

    public void setMana(float value)
    {
        mana += value;

        UpdateUserInformationUI?.Invoke();
    }

    public BigInteger GetGoldGainRate()
    {
        return extraGoldGainRate;
    }


    public void UseHealSkill(BaseSkill skill)
    {
        BigInteger skillValue = (BigInteger)skill.SkillData.hpRecoveryAmount;

        BigInteger Healhealth = maxHealth * (skillValue / 100);

        if (health + Healhealth <= maxHealth)
        {
            health += Healhealth;
        }
        else
        {
            health = maxHealth;
        }

    }

    public void RefillHpAndMp()
    {
        health = maxHealth;
        mana = maxMana;
    }

    public void ResetHealth()
    {
        health = PlayerStatCalculator.GetAdjustedMaxHealth();
    }

    public void ResetMana()
    {
        mana = PlayerStatCalculator.GetAdjustedMaxMana();
    }

    public void ApplyPassiveStats()
    {
        //BigInteger increaseRate = new BigInteger(PassiveStatManager.Instance.PassiveHpAndHpRecoveryIncreaseRate);

        //maxHealth += maxHealth * increaseRate / 100;
        //healthRegeneration += healthRegeneration * increaseRate / 100;

        //maxMana += maxMana * (PassiveStatManager.Instance.PassiveMpAndMpRecoveryIncreaseRate / 100);
        //manaRegeneration += manaRegeneration * (PassiveStatManager.Instance.PassiveMpAndMpRecoveryIncreaseRate / 100);

        //extraExpRate += PassiveStatManager.Instance.PassiveAddEXPRate;
    }
}