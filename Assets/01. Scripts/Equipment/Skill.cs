//using UnityEngine;

//[System.Serializable]
//public class Skill : IFusable
//{
//    public SkillDataSO BaseData { get; private set; }
//    BaseItemDataSO IEnhanceable.BaseData => BaseData;
//    public int EnhancementLevel { get; private set; }
//    public int StackCount { get; internal set; }
//    public float Cooldown { get; private set; }
//    public float DamagePercent { get; private set; }
//    public float BuffDuration { get; private set; }
//    public int RequiredCurrencyForUpgrade { get; private set; }

//    public Skill(SkillDataSO baseData)
//    {
//        BaseData = baseData;
//        EnhancementLevel = 0;
//        StackCount = 1;
//        Cooldown = baseData.cooldown;
//        DamagePercent = baseData.damagePercent;
//        BuffDuration = baseData.buffDuration;
//        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
//    }

//    public bool CanEnhance()
//    {
//        return CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald) >= RequiredCurrencyForUpgrade
//               && StackCount >= BaseData.requireSkillCardsForUpgrade
//               && EnhancementLevel < 100;
//    }

//    public bool Enhance()
//    {
//        if (!CanEnhance())
//        {
//            return false;
//        }

//        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, RequiredCurrencyForUpgrade);
//        StackCount -= BaseData.requireSkillCardsForUpgrade;
//        EnhancementLevel++;
//        UpdateSkillEffects();

//        return true;
//    }

//    private void UpdateSkillEffects()
//    {
//        Cooldown = Mathf.Max(0.5f, BaseData.cooldown - EnhancementLevel * 0.1f);
//        DamagePercent = BaseData.damagePercent + EnhancementLevel * 0.05f;
//        BuffDuration = BaseData.buffDuration + EnhancementLevel * 0.2f;
//    }

//    public bool Fuse(int materialCount)
//    {
//        int totalRequiredMaterials = materialCount;

//        if (StackCount < totalRequiredMaterials)
//        {
//            return false;
//        }

//        StackCount -= totalRequiredMaterials;

//        SkillDataSO nextSkill = DataManager.Instance.GetNextSkill(BaseData.grade);

//        if (nextSkill != null)
//        {
//            Skill newSkill = new Skill(nextSkill);
//            PlayerObjManager.Instance.Player.inventory.SkillInventory.AddItem(newSkill);

//            return true;
//        }

//        return false;
//    }

//    public void AddStack(int count)
//    {
//        StackCount += count;
//    }

//    public void RemoveStack(int count)
//    {
//        StackCount = Mathf.Max(StackCount - count, 0);
//    }
//}