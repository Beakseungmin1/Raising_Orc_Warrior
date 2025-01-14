using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class Datas
{
    public int level = 0;
    public BigInteger exp = 0;
    public BigInteger needExp = 100;
    public float attackPower = 20;
    public BigInteger maxHealth = 200;
    public BigInteger health = 200;
    public BigInteger healthRegeneration = 0;
    public float criticalProbability = 0;
    public float criticalIncreaseDamage = 100;
    public float bluecriticalIncreaseDamage = 0;
    public float bluecriticalProbability = 0;
    public float maxMana = 200;
    public float mana = 200;
    public float manaRegeneration = 5;
    public float hitLate = 0;
    public float avoid = 0;
    public BigInteger extraGoldGainRate = 0;
    public BigInteger extraExpRate = 0;
    public float attackSpeed = 0;
    public float normalMonsterIncreaseDamage = 0;
    public float bossMonsterIncreaseDamage = 0;
    public int attackLevel = 1;
    public int healthLevel = 1;
    public int healthRegenerationLevel = 1;
    public int criticalIncreaseDamageLevel = 1;
    public int criticalProbabilityLevel = 1;
    public int bluecriticalIncreaseDamageLevel = 1;
    public int bluecriticalProbabilityLevel = 1;
    public BigInteger needAttackUpgradeMoney = 1000;
    public BigInteger needHealthUpgradeMoney = 1000;
    public BigInteger needHealthRegenerationUpgradeMoney = 1000;
    public BigInteger needCriticalIncreaseDamageUpgradeMoney = 1000;
    public BigInteger needCriticalProbabilityUpgradeMoney = 1000;
    public BigInteger needBlueCriticalIncreaseDamageUpgradeMoney = 1000;
    public BigInteger needBlueCriticalProbabilityUpgradeMoney = 1000;
    public BigInteger gold = 0;
    public float Emerald = 0;
    public float Cube = 0;
    public float Diamond = 0;
    public float DungeonTicket = 0;
    public int curChapter = 0;
    public int curStageIndex = 0;
}

public class SaveManager : Singleton<SaveManager>
{

    public Datas datas;

    private string KeyName = "Datas";
    private string fileName = "SaveFile.es3";

    PlayerStat stat;
    CurrencyManager currency;

    private void Awake()
    {
        stat = PlayerObjManager.Instance.Player.stat;
    }

    public void DataSave()
    {
        datas.level = stat.level;
        datas.exp = stat.exp;
        datas.needExp = stat.needExp;
        datas.attackPower = stat.attackPower;
        datas.health = stat.health;
        datas.maxHealth = stat.maxHealth;
        datas.healthRegeneration = stat.healthRegeneration;
        datas.criticalProbability = stat.criticalProbability;
        datas.criticalIncreaseDamage = stat.criticalIncreaseDamage;
        datas.bluecriticalIncreaseDamage = stat.bluecriticalIncreaseDamage;
        datas.bluecriticalProbability = stat.bluecriticalProbability;
        datas.mana = stat.mana;
        datas.maxMana = stat.maxMana;
        datas.manaRegeneration = stat.manaRegeneration;
        datas.hitLate = stat.hitLate;
        datas.avoid = stat.avoid;
        datas.attackLevel = stat.attackLevel;
        datas.healthLevel = stat.healthLevel;
        datas.healthRegenerationLevel = stat.healthRegenerationLevel;
        datas.criticalIncreaseDamageLevel = stat.criticalIncreaseDamageLevel;
        datas.criticalProbabilityLevel = stat.criticalProbabilityLevel;
        datas.bluecriticalIncreaseDamageLevel = stat.bluecriticalIncreaseDamageLevel;
        datas.bluecriticalProbabilityLevel = stat.bluecriticalProbabilityLevel;
        datas.needAttackUpgradeMoney = stat.needAttackUpgradeMoney;
        datas.needHealthUpgradeMoney = stat.needHealthUpgradeMoney;
        datas.needHealthRegenerationUpgradeMoney = stat.needHealthRegenerationUpgradeMoney;
        datas.needCriticalIncreaseDamageUpgradeMoney = stat.needCriticalIncreaseDamageUpgradeMoney;
        datas.needCriticalProbabilityUpgradeMoney = stat.needCriticalProbabilityUpgradeMoney;
        datas.needBlueCriticalIncreaseDamageUpgradeMoney = stat.needBlueCriticalIncreaseDamageUpgradeMoney;
        datas.needBlueCriticalProbabilityUpgradeMoney= stat.needBlueCriticalProbabilityUpgradeMoney;
        datas.gold = CurrencyManager.Instance.GetGold();
        datas.Emerald = CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald);
        datas.Cube = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube);
        datas.Diamond = CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond);
        datas.DungeonTicket = CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket);
        datas.curChapter = StageManager.Instance.curChapterIndex;
        datas.curStageIndex = StageManager.Instance.curStageIndex;


        ES3.Save("Datas", datas);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName))
        {
            ES3.LoadInto(KeyName, datas);
            stat.StatDataLoad();
            stat.UpdateNeedMoney();
            CurrencyManager.Instance.SetGold(datas.gold);
            CurrencyManager.Instance.SetCurrency(CurrencyType.Emerald, datas.Emerald);
            CurrencyManager.Instance.SetCurrency(CurrencyType.Cube, datas.Cube);
            CurrencyManager.Instance.SetCurrency(CurrencyType.Diamond, datas.Diamond);
            CurrencyManager.Instance.SetCurrency(CurrencyType.DungeonTicket, datas.DungeonTicket);
            StageManager.Instance.curChapterIndex = datas.curChapter;
            StageManager.Instance.curStageIndex = datas.curStageIndex;

        }
        else
        {
            DataSave();
        }

    }

    public void DataDelete()
    {
        ES3.DeleteKey(KeyName);
    }


    //테스트용 임시 메서드들

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            DataSave();
        }
    }

    private void OnApplicationQuit()
    {
        DataSave();
    }
}
