using UnityEngine;

[System.Serializable]
public class Accessory : IFusable
{
    public AccessoryDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;
    public int Rank => BaseData.rank;
    public int EnhancementLevel { get; private set; }
    public int StackCount { get; internal set; }
    public int RequiredCurrencyForUpgrade { get; private set; }
    public float EquipHpAndHpRecoveryIncreaseRate { get; private set; }
    public float PassiveHpAndHpRecoveryIncreaseRate { get; private set; }
    public float PassiveMpAndMpRecoveryIncreaseRate { get; private set; }
    public float PassiveAddEXPRate { get; private set; }

    public Accessory(AccessoryDataSO baseData, int initialStackCount = 1)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = initialStackCount;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipHpAndHpRecoveryIncreaseRate = baseData.equipHpAndHpRecoveryIncreaseRate;
        PassiveHpAndHpRecoveryIncreaseRate = Mathf.RoundToInt(baseData.equipHpAndHpRecoveryIncreaseRate / 3f);
        PassiveMpAndMpRecoveryIncreaseRate = baseData.passiveMpAndMpRecoveryIncreaseRate;
        PassiveAddEXPRate = baseData.passiveAddEXPRate;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Cube) >= RequiredCurrencyForUpgrade
               && EnhancementLevel < 100;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, RequiredCurrencyForUpgrade);
        EnhancementLevel++;
        RequiredCurrencyForUpgrade = Mathf.RoundToInt(RequiredCurrencyForUpgrade * 1.5f);
        UpdateAccessoryEffects();

        PlayerObjManager.Instance.Player.inventory.NotifyInventoryChanged(false);
        return true;
    }

    private void UpdateAccessoryEffects()
    {
        EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 1.5f;
        PassiveHpAndHpRecoveryIncreaseRate = Mathf.RoundToInt(EquipHpAndHpRecoveryIncreaseRate / 3f);

        if (PassiveMpAndMpRecoveryIncreaseRate > 0)
        {
            PassiveMpAndMpRecoveryIncreaseRate += EnhancementLevel * 1.2f;
        }

        if (PassiveAddEXPRate > 0)
        {
            PassiveAddEXPRate += EnhancementLevel * 0.5f;
        }
    }

    public bool Fuse(int materialCount)
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        int requiredCount = BaseData.requireFuseItemCount;
        int totalRequiredMaterials = materialCount * requiredCount;

        var existingItem = inventory.AccessoryInventory.GetItem(BaseData.itemName);
        if (existingItem == null || existingItem.StackCount < totalRequiredMaterials)
        {
            return false;
        }

        existingItem.RemoveStack(totalRequiredMaterials);

        for (int i = 0; i < materialCount; i++)
        {
            var nextAccessoryData = DataManager.Instance.GetNextAccessory(BaseData.grade, BaseData.rank);

            if (nextAccessoryData is AccessoryDataSO accessoryDataSO)
            {
                inventory.AddItemToInventory(accessoryDataSO);
            }
        }

        return true;
    }

    public void AddStack(int count)
    {
        StackCount += count;
    }

    public void RemoveStack(int count)
    {
        StackCount = Mathf.Max(StackCount - count, 0);
    }
}