using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    public int curStageIndexInThisChapter = 0;
    public int MaxStageIndexInThisChapter = 0;
    public int WeaponSummonLevel =1;
    public float WeaponSummonExp = 0f;
    public float WeaponSummonNextExp = 100f;
    public int AccessorySummonLevel = 1;
    public float AccessorySummonExp = 0f;
    public float AccessorySummonNextExp = 100f;
    public int SkillSummonLevel = 1;
    public float SkillSummonExp = 0f;
    public float SkillSummonNextExp = 100f;
}


public class SaveManager : Singleton<SaveManager>
{

    public Datas datas;
    public List<SkillSaveData> SkillInventory;
    public List<Weapon> WeaponInventory;
    public List<Accessory> AccessoryInventory;


    private string KeyName = "Datas";
    private string fileName = "SaveFile.es3";

    PlayerStat stat;
    CurrencyManager currency;
    PlayerInventory inventory;


    public void Init()
    {
        datas = new Datas();
        SkillInventory = new List<SkillSaveData>();

        stat = PlayerObjManager.Instance.Player.stat;
        inventory = PlayerObjManager.Instance.Player.inventory;
        currency = CurrencyManager.Instance;


        if (ES3.FileExists(fileName))
        {
            DataLoad();
        }
        else
        {
            stat.SetDefaultStat();
        }
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
        datas.gold = currency.GetGold();
        datas.Emerald = currency.GetCurrency(CurrencyType.Emerald);
        datas.Cube = currency.GetCurrency(CurrencyType.Cube);
        datas.Diamond = currency.GetCurrency(CurrencyType.Diamond);
        datas.DungeonTicket = currency.GetCurrency(CurrencyType.DungeonTicket);
        datas.curChapter = StageManager.Instance.curChapterIndex;
        datas.curStageIndex = StageManager.Instance.curStageIndex;
        datas.curStageIndexInThisChapter = StageManager.Instance.curStageIndexInThisChapter;
        datas.MaxStageIndexInThisChapter = StageManager.Instance.MaxStageIndexInThisChapter;
        datas.WeaponSummonLevel = SummonDataManager.Instance.GetLevel(ItemType.Weapon);
        datas.WeaponSummonExp = SummonDataManager.Instance.GetExp(ItemType.Weapon);
        datas.WeaponSummonNextExp = SummonDataManager.Instance.GetExpToNextLevel(ItemType.Weapon);
        datas.AccessorySummonLevel = SummonDataManager.Instance.GetLevel(ItemType.Accessory);
        datas.AccessorySummonExp = SummonDataManager.Instance.GetExp(ItemType.Accessory);
        datas.AccessorySummonNextExp = SummonDataManager.Instance.GetExpToNextLevel(ItemType.Accessory);
        datas.SkillSummonLevel = SummonDataManager.Instance.GetLevel(ItemType.Skill);
        datas.SkillSummonExp = SummonDataManager.Instance.GetExp(ItemType.Skill);
        datas.SkillSummonNextExp = SummonDataManager.Instance.GetExpToNextLevel(ItemType.Skill);


        SkillInventory = new List<SkillSaveData>();
        List<BaseSkill> skills = inventory.GetSkillInventory().GetAllItems();
        foreach (BaseSkill skill in skills)
        {
            SkillSaveData newSkillData = new SkillSaveData();


            newSkillData.Skillid = skill.SkillData.SkillId;
            newSkillData.EnhancementLevel = skill.EnhancementLevel;
            newSkillData.StackCount = skill.StackCount;


            SkillInventory.Add(newSkillData);
        }
        //Debug.Log(SkillInventory[0].skillData);

        //datas.WeaponInventory = inventory.GetWeaponInventory().GetAllItems();
        //datas.AccessoryInventory = inventory.GetAccessoryInventory().GetAllItems();
        // �ʿ��� �ʵ常 ������ �ִ� Ŭ������ �ϳ� ����� ��� ����ȭ �����ϰ�

        //Unity���� UnityEngine.Object Ÿ���� �ʵ�(��: Component, ScriptableObject, Texture2D ��)�� ������ ����Ǽ� ���� �����ϰ� �ҷ�����
        ES3.Save("SkillInventory", SkillInventory);

        ES3.Save("Datas", datas);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName))
        {
            // ����Ʈ �ε� �ҷ��� ������ �̷��� �ؾ��Ѵ� �ù� �����̶� �ٸ���ɾ� ���� ����ȉ�
            List<SkillSaveData> skillSaveDatas = ES3.Load<List<SkillSaveData>>("SkillInventory");

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
            StageManager.Instance.curStageIndexInThisChapter = datas.curStageIndexInThisChapter;
            StageManager.Instance.MaxStageIndexInThisChapter = datas.MaxStageIndexInThisChapter;
            SummonDataManager.Instance.SetLevel(ItemType.Weapon, datas.WeaponSummonLevel);
            SummonDataManager.Instance.SetExp(ItemType.Weapon, datas.WeaponSummonExp);
            SummonDataManager.Instance.SetExpToNextLevel(ItemType.Weapon, datas.WeaponSummonNextExp);
            SummonDataManager.Instance.SetLevel(ItemType.Accessory, datas.AccessorySummonLevel);
            SummonDataManager.Instance.SetExp(ItemType.Accessory, datas.AccessorySummonExp);
            SummonDataManager.Instance.SetExpToNextLevel(ItemType.Accessory, datas.AccessorySummonNextExp);
            SummonDataManager.Instance.SetLevel(ItemType.Skill, datas.SkillSummonLevel);
            SummonDataManager.Instance.SetExp(ItemType.Skill, datas.SkillSummonExp);
            SummonDataManager.Instance.SetExpToNextLevel(ItemType.Skill, datas.SkillSummonNextExp);


            List<SkillDataSO> allSkills = DataManager.Instance.GetAllSkills();

            //�����س��� ��ų�����͵��� ��� ���캽
            foreach (SkillSaveData skill in skillSaveDatas)
            {
                //ã�� ���彺ų�����͸� ��� ��ų�����͵��̶� ���� ������ �������
                foreach (SkillDataSO data in allSkills)
                {
                    if (skill.Skillid == data.SkillId)
                    {
                        skill.SkillDataSO = data;
                    }
                }
                inventory.SetTestSkillInventory(skill);
            }

            //inventory.SetSkillInventory(datas.SkillInventory);
            //inventory.SetWeaponInventory(datas.WeaponInventory);
            //inventory.SetAccessoryInventory(datas.AccessoryInventory);
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


    //�׽�Ʈ�� �ӽ� �޼����

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
