using System;
using UnityEngine;

[System.Serializable]
public class Accessory : IFusable
{
    public AccessoryDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;
    public int Rank => BaseData.rank;
    public Grade Grade => BaseData.grade;
    public int EnhancementLevel { get; set; }
    public int StackCount { get; internal set; }
    public int RequiredCurrencyForUpgrade { get; private set; }
    public double EquipHpAndHpRecoveryIncreaseRate { get; private set; }
    public int PassiveHpAndHpRecoveryIncreaseRate { get; set; }
    public int PassiveMpAndMpRecoveryIncreaseRate { get;  set; }
    public int PassiveAddEXPRate { get;  set; }

    public Accessory(AccessoryDataSO baseData, int initialStackCount = 1)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = initialStackCount;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipHpAndHpRecoveryIncreaseRate = baseData.equipHpAndHpRecoveryIncreaseRate;
        PassiveHpAndHpRecoveryIncreaseRate = Mathf.RoundToInt((float)baseData.equipHpAndHpRecoveryIncreaseRate / 3f);
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
        RequiredCurrencyForUpgrade = Mathf.RoundToInt(RequiredCurrencyForUpgrade * 1.1f);
        UpdateAccessoryEffects();
        PassiveManager.Instance.UpdateAccessoryEffects();
        PlayerObjManager.Instance.Player.inventory.NotifyInventoryChanged(false);
        return true;
    }

    private void UpdateAccessoryEffects()
    {
        switch (Grade)
        {
            case Grade.Normal:
                EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 1;
                PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 1;
                break;

            case Grade.Uncommon:
                EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 10;
                PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 2;
                break;

            case Grade.Rare:
                EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 30;
                PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 10;
                break;

            case Grade.Hero:
                EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 200;
                PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 50;
                break;

            case Grade.Legendary:
                EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 2000;
                PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 700;
                break;

            case Grade.Mythic:
                if (Rank == 4)
                {
                    EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 30000;
                    PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 3000;
                }
                else if (Rank == 3)
                {
                    EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 70000;
                    PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 7000;
                }
                else if (Rank == 2)
                {
                    EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 100000;
                    PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 30000;
                }
                else if (Rank == 1)
                {
                    EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 300000;
                    PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 100000;
                }
                break;

            case Grade.Ultimate:
                EquipHpAndHpRecoveryIncreaseRate += EnhancementLevel * 3000000;
                PassiveHpAndHpRecoveryIncreaseRate += EnhancementLevel * 300000;
                break;
        }

        if (PassiveMpAndMpRecoveryIncreaseRate >= 0)
        {
            PassiveMpAndMpRecoveryIncreaseRate += 1;
        }

        if (PassiveAddEXPRate >= 0)
        {
            PassiveAddEXPRate += 1;
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