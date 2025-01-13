using UnityEngine;
using System;

[System.Serializable]
public class Weapon : IFusable
{
    public WeaponDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;
    public int Rank => BaseData.rank;
    public Grade Grade => BaseData.grade;
    public int EnhancementLevel { get; set; }
    public int StackCount { get; internal set; }
    public int RequiredCurrencyForUpgrade { get; private set; }
    public double EquipAtkIncreaseRate { get; private set; }
    public int PassiveEquipAtkIncreaseRate { get; private set; }
    public int PassiveCriticalDamageBonus { get; private set; }
    public int PassiveGoldGainRate { get; private set; }
    public Color GradeColor { get; private set; }

    public Weapon(WeaponDataSO baseData, int initialStackCount = 1)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = initialStackCount;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipAtkIncreaseRate = baseData.equipAtkIncreaseRate;
        PassiveEquipAtkIncreaseRate = Mathf.RoundToInt((float)baseData.equipAtkIncreaseRate / 10f);
        PassiveCriticalDamageBonus = baseData.passiveCriticalDamageBonus;
        PassiveGoldGainRate = baseData.passiveGoldGainRate;

        GradeColor = GetGradeColor(Grade);
    }

    private Color GetGradeColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.Normal: return new Color(0.53f, 0.53f, 0.53f);
            case Grade.Uncommon: return new Color(0.22f, 0.92f, 0.54f);
            case Grade.Rare: return new Color(1f, 0.62f, 0.28f);
            case Grade.Hero: return new Color(0.24f, 0.58f, 1f);
            case Grade.Legendary: return new Color(0.75f, 0.25f, 1f);
            case Grade.Mythic: return new Color(1f, 0.2f, 0.2f);
            case Grade.Ultimate: return new Color(1f, 1f, 0.24f);
            default: return Color.white;
        }
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
        UpdateWeaponEffects();
        PassiveManager.Instance.UpdateWeaponEffects();
        PlayerObjManager.Instance.Player.inventory.NotifyInventoryChanged(true);
        return true;
    }

    private void UpdateWeaponEffects()
    {
        switch (Grade)
        {
            case Grade.Normal:
                EquipAtkIncreaseRate += EnhancementLevel * 1;
                PassiveEquipAtkIncreaseRate += EnhancementLevel * 1;
                break;

            case Grade.Uncommon:
                EquipAtkIncreaseRate += EnhancementLevel * 10;
                PassiveEquipAtkIncreaseRate += EnhancementLevel * 2;
                break;

            case Grade.Rare:
                EquipAtkIncreaseRate += EnhancementLevel * 30;
                PassiveEquipAtkIncreaseRate += EnhancementLevel * 10;
                break;

            case Grade.Hero:
                EquipAtkIncreaseRate += EnhancementLevel * 200;
                PassiveEquipAtkIncreaseRate += EnhancementLevel * 50;
                break;

            case Grade.Legendary:
                EquipAtkIncreaseRate += EnhancementLevel * 2000;
                PassiveEquipAtkIncreaseRate += EnhancementLevel * 700;
                break;

            case Grade.Mythic:
                if (Rank == 4)
                {
                    EquipAtkIncreaseRate += EnhancementLevel * 100000;
                    PassiveEquipAtkIncreaseRate += EnhancementLevel * 10000;
                }
                else if (Rank == 3)
                {
                    EquipAtkIncreaseRate += EnhancementLevel * 300000;
                    PassiveEquipAtkIncreaseRate += EnhancementLevel * 100000;
                }
                else if (Rank == 2)
                {
                    EquipAtkIncreaseRate += EnhancementLevel * 700000;
                    PassiveEquipAtkIncreaseRate += EnhancementLevel * 200000;
                }
                else if (Rank == 1)
                {
                    EquipAtkIncreaseRate += EnhancementLevel * 1000000;
                    PassiveEquipAtkIncreaseRate += EnhancementLevel * 300000;
                }
                break;

            case Grade.Ultimate:
                EquipAtkIncreaseRate += EnhancementLevel * 10000000;
                PassiveEquipAtkIncreaseRate += EnhancementLevel * 1000000;
                break;
        }

        if (PassiveCriticalDamageBonus > 0)
        {
            PassiveCriticalDamageBonus += EnhancementLevel * 1;
        }

        if (PassiveGoldGainRate > 0)
        {
            PassiveGoldGainRate += EnhancementLevel * 1;
        }
    }

    public bool Fuse(int materialCount)
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        int requiredCount = BaseData.requireFuseItemCount;
        int totalRequiredMaterials = materialCount * requiredCount;

        var existingItem = inventory.WeaponInventory.GetItem(BaseData.itemName);
        if (existingItem == null || existingItem.StackCount < totalRequiredMaterials)
        {
            return false;
        }

        existingItem.RemoveStack(totalRequiredMaterials);

        for (int i = 0; i < materialCount; i++)
        {
            var nextWeaponData = DataManager.Instance.GetNextWeapon(BaseData.grade, BaseData.rank);

            if (nextWeaponData is WeaponDataSO weaponDataSO)
            {
                inventory.AddItemToInventory(weaponDataSO);
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