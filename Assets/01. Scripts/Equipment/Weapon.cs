using UnityEngine;

[System.Serializable]
public class Weapon : IEnhanceable, IFusable, IStackable
{
    public WeaponDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;
    public int Rank => BaseData.rank;
    public int EnhancementLevel { get; private set; }
    public int StackCount { get; private set; }
    public int RequiredCurrencyForUpgrade { get; private set; }
    public float EquipAtkIncreaseRate { get; private set; }
    public float PassiveEquipAtkIncreaseRate { get; private set; }
    public float PassiveCriticalDamageBonus { get; private set; }
    public float PassiveGoldGainRate { get; private set; }

    public Weapon(WeaponDataSO baseData)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = 1; // 기본 스택 수
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipAtkIncreaseRate = baseData.equipAtkIncreaseRate;
        PassiveEquipAtkIncreaseRate = Mathf.RoundToInt(baseData.equipAtkIncreaseRate / 3f);
        PassiveCriticalDamageBonus = baseData.passiveCriticalDamageBonus;
        PassiveGoldGainRate = baseData.passiveGoldGainRate;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Cube) >= RequiredCurrencyForUpgrade
               && EnhancementLevel < 100; // 최대 레벨 100으로 설정
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
        int totalRequiredMaterials = materialCount;

        if (StackCount < totalRequiredMaterials)
        {
            return false;
        }

        RemoveStack(totalRequiredMaterials); // 필요한 재료 수만큼 차감

        if (BaseData.grade == Grade.Ultimate && BaseData.rank == 4)
        {
            return false;
        }

        // rank가 1인 경우, 다음 등급으로 넘어가고 rank 4로 설정
        if (BaseData.rank == 1)
        {
            Grade nextGrade = BaseData.grade + 1;
            WeaponDataSO nextWeaponData = DataManager.Instance.GetWeaponByGradeAndRank(nextGrade, 4);

            if (nextWeaponData != null)
            {
                Weapon newWeapon = new Weapon(nextWeaponData);
                PlayerObjManager.Instance.Player.inventory.WeaponInventory.AddItem(newWeapon);
                return true;
            }
        }
        else
        {
            WeaponDataSO nextWeaponData = DataManager.Instance.GetNextWeapon(BaseData.grade, BaseData.rank);

            if (nextWeaponData != null)
            {
                Weapon newWeapon = new Weapon(nextWeaponData);
                PlayerObjManager.Instance.Player.inventory.WeaponInventory.AddItem(newWeapon);
                return true;
            }
        }

        return false;
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