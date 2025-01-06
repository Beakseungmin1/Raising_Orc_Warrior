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
    public float extraExpRate { get; private set; }
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

    public Action UpdateLevelStatUI;

    public Action UpdateUserInformationUI;

    public Action OnStatChange;

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

        Debug.Log("Health: " + health + ", MaxHealth: " + PlayerStatCalculator.GetAdjustedMaxHealth());

        if (mana < maxMana)
        {
            mana += manaRegeneration * Time.deltaTime;

            if (mana > maxMana)
            {
                mana = maxMana;
            }
        }
    }

    public void AddExpFromMonsters(IEnemy enemy)
    {
        exp += enemy.GiveExp();

        while (exp >= needExp)
        {
            LevelUp();
        }
        PlayerObjManager.Instance.Player.stat.UpdateLevelStatUI?.Invoke();
    }

    public void AddExp(BigInteger getExp)
    {
        exp += getExp;

        while (exp >= needExp)
        {
            LevelUp();
        }
        PlayerObjManager.Instance.Player.stat.UpdateLevelStatUI?.Invoke();
    }

    public void decreaseHp(BigInteger damage)
    {
        health -= damage;

        Debug.Log("Current Health: " + health + " / Max Health: " + PlayerStatCalculator.GetAdjustedMaxHealth());
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
        }
        else
        {
            Debug.Log("����ġ�� �����մϴ�");
        }
    }

    public void AttackLevelUp()
    {
        needAttackUpgradeMoney = attackLevel * 1000; //�ʿ䰡�� ��������

        if (CurrencyManager.Instance.GetGold() >= needAttackUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needAttackUpgradeMoney);
            attackLevel++;
            attackPower = 20 + (attackLevel * 4);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void HealthLevelUp()
    {
        //���� ������ �ϵ�Ӵ� ���� ��� �߰�
        needHealthUpgradeMoney = healthLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needHealthUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needHealthUpgradeMoney);
            healthLevel++;
            maxHealth = 200 + (healthLevel * 40);

            OnStatChange?.Invoke();
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void HealthRegenerationLevelUp()
    {
        needHealthRegenerationUpgradeMoney = healthRegenerationLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needHealthRegenerationUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needHealthRegenerationUpgradeMoney);
            healthRegenerationLevel++;
            healthRegeneration = healthRegenerationLevel * 4;

            OnStatChange?.Invoke();
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void CriticalIncreaseDamageLevelUp()
    {
        needCriticalIncreaseDamageUpgradeMoney = criticalIncreaseDamageLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needCriticalIncreaseDamageUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needCriticalIncreaseDamageUpgradeMoney);
            criticalIncreaseDamageLevel++;
            criticalIncreaseDamage = 100 + criticalIncreaseDamageLevel;

            OnStatChange?.Invoke();
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void CriticalProbabilityLevelUp()
    {
        needCriticalProbabilityUpgradeMoney = criticalProbabilityLevel * 1000;

        if (CurrencyManager.Instance.GetGold() >= needCriticalProbabilityUpgradeMoney)
        {
            CurrencyManager.Instance.SubtractGold(needCriticalProbabilityUpgradeMoney);
            criticalProbability = criticalProbabilityLevel * 0.1f;
            criticalProbabilityLevel++;

            OnStatChange?.Invoke();
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
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
            Debug.Log("��尡 �����մϴ�.");
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
            Debug.Log("��尡 �����մϴ�.");
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
        maxMana = 10000;
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
        criticalProbabilityLevel = 1;
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

    public void RefillHP()
    {
        health = maxHealth;
    }

    public void ResetHealth()
    {
        health = PlayerStatCalculator.GetAdjustedMaxHealth();
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