using UnityEngine;

[System.Serializable]
public class Weapon : IFusable
{
    public WeaponDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;
    public int Rank => BaseData.rank;
    public int EnhancementLevel { get; private set; }
    public int StackCount { get; internal set; }
    public int RequiredCurrencyForUpgrade { get; private set; }
    public float EquipAtkIncreaseRate { get; private set; }
    public float PassiveEquipAtkIncreaseRate { get; private set; }
    public float PassiveCriticalDamageBonus { get; private set; }
    public float PassiveGoldGainRate { get; private set; }

    public Weapon(WeaponDataSO baseData, int initialStackCount = 1)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = initialStackCount;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipAtkIncreaseRate = baseData.equipAtkIncreaseRate;
        PassiveEquipAtkIncreaseRate = Mathf.RoundToInt(baseData.equipAtkIncreaseRate / 3f);
        PassiveCriticalDamageBonus = baseData.passiveCriticalDamageBonus;
        PassiveGoldGainRate = baseData.passiveGoldGainRate;
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
        UpdateWeaponEffects();

        PlayerObjManager.Instance.Player.inventory.NotifyInventoryChanged(true);
        return true;
    }

    private void UpdateWeaponEffects()
    {
        EquipAtkIncreaseRate += EnhancementLevel * 2;
        PassiveEquipAtkIncreaseRate = Mathf.RoundToInt(EquipAtkIncreaseRate / 3f);

        if (PassiveCriticalDamageBonus > 0)
        {
            PassiveCriticalDamageBonus += EnhancementLevel * 1;
        }

        if (PassiveGoldGainRate > 0)
        {
            PassiveGoldGainRate += EnhancementLevel * 0.5f;
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