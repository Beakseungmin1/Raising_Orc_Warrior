using System.Collections.Generic;
using System.Numerics;

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
    public List<WeaponSaveData> WeaponInventory;
    public List<AccessorySaveData> AccessoryInventory;


    private string KeyName = "Datas";
    private string fileName = "SaveFile.es3";

    PlayerStat stat;
    CurrencyManager currency;
    PlayerInventory inventory;


    public void Init()
    {
        datas = new Datas();
        SkillInventory = new List<SkillSaveData>();
        WeaponInventory = new List<WeaponSaveData>();
        AccessoryInventory = new List<AccessorySaveData>();

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


        SkillInventory.Clear();
        List<BaseSkill> skills = inventory.GetSkillInventory().GetAllItems();
        foreach (BaseSkill skill in skills)
        {
            SkillSaveData newSkillData = new SkillSaveData();


            newSkillData.Skillid = skill.SkillData.SkillId;
            newSkillData.EnhancementLevel = skill.EnhancementLevel;
            newSkillData.StackCount = skill.StackCount;


            SkillInventory.Add(newSkillData);
        }

        WeaponInventory.Clear();
        List<Weapon> weapons = inventory.GetWeaponInventory().GetAllItems();
        foreach (Weapon weapon in weapons)
        {
            WeaponSaveData newWeaponData = new WeaponSaveData();


            newWeaponData.WeaponId = weapon.BaseData.weaponId;
            newWeaponData.EnhancementLevel = weapon.EnhancementLevel;
            newWeaponData.StackCount = weapon.StackCount;


            WeaponInventory.Add(newWeaponData);
        }

        AccessoryInventory.Clear();
        List<Accessory> accessorys = inventory.GetAccessoryInventory().GetAllItems();
        foreach (Accessory access in accessorys)
        {
            AccessorySaveData newAccessoryData = new AccessorySaveData();


            newAccessoryData.AccessoryId = access.BaseData.AccessoryId;
            newAccessoryData.EnhancementLevel = access.EnhancementLevel;
            newAccessoryData.StackCount = access.StackCount;


            AccessoryInventory.Add(newAccessoryData);
        }

        //Unity에서 UnityEngine.Object 타입의 필드(예: Component, ScriptableObject, Texture2D 등)는 참조로 저장되서 따로 저장하고 불러야함
        ES3.Save("SkillInventory", SkillInventory);
        ES3.Save("WeaponInventory", WeaponInventory);
        ES3.Save("AccessoryInventory", AccessoryInventory);

        ES3.Save("Datas", datas);
    }

    public void DataLoad()
    {
        if (ES3.FileExists(fileName))
        {
            // 리스트 로드 할려면 무조건 이렇게 해야한다 조금이라도 다른명령어 쓰면 절대안됌
            List<SkillSaveData> skillSaveDatas = ES3.Load<List<SkillSaveData>>("SkillInventory");
            List<WeaponSaveData> weaponSaveDatas = ES3.Load<List<WeaponSaveData>>("WeaponInventory");
            List<AccessorySaveData> accessorySavedatas = ES3.Load<List<AccessorySaveData>>("AccessoryInventory");

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

            // 모든 스킬의 So데이터를 가져옴
            List<SkillDataSO> allSkills = DataManager.Instance.GetAllSkills();

            //저장해놓은 스킬데이터들을 모두 살펴봄
            foreach (SkillSaveData skill in skillSaveDatas)
            {
                //찾은 저장스킬데이터를 모든 스킬데이터들이랑 비교후 같은걸 집어넣음
                foreach (SkillDataSO skillData in allSkills)
                {
                    if (skill.Skillid == skillData.SkillId)
                    {
                        skill.SkillDataSO = skillData;
                    }
                }
                // 넣은 SO 데이터를 가지고있던 갯수만큼 추가해줌
                inventory.SetSkillSaveDataInventory(skill);
            }

            List<WeaponDataSO> allWeapons = DataManager.Instance.GetAllWeapons();

            foreach (WeaponSaveData weapon in weaponSaveDatas)
            {
                foreach (WeaponDataSO weaponData in allWeapons)
                {
                    if (weapon.WeaponId == weaponData.weaponId)
                    {
                        weapon.WeaponDataSO = weaponData;
                    }
                }
                inventory.SetWeaponSaveDataInventory(weapon);
            }

            List<AccessoryDataSO> allAccessorys = DataManager.Instance.GetAllAccessories();

            foreach (AccessorySaveData accessory in accessorySavedatas)
            {
                foreach (AccessoryDataSO accessoryData in allAccessorys)
                {
                    if (accessory.AccessoryId == accessoryData.AccessoryId)
                    {
                        accessory.AccessoryDataSO = accessoryData;
                    }
                }
                inventory.SetAccessorySaveDataInventory(accessory);
            }
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
