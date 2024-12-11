using UnityEngine;

[System.Serializable]
public class Accessory : IEnhanceable, IFusable, IStackable
{
    public AccessoryDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;
    public int Rank => BaseData.rank;
    public int EnhancementLevel { get; private set; }
    public int StackCount { get; private set; }
    public int RequiredCurrencyForUpgrade { get; private set; }
    public float EquipHpAndHpRecoveryIncreaseRate { get; private set; }
    public float PassiveHpAndHpRecoveryIncreaseRate { get; private set; }
    public float PassiveMpAndMpRecoveryIncreaseRate { get; private set; }
    public float PassiveAddEXPRate { get; private set; }

    public Accessory(AccessoryDataSO baseData)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = 1; // 기본 스택 수
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
        int totalRequiredMaterials = materialCount; // 합성 횟수만큼 필요한 재료 수

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
            AccessoryDataSO nextAccessoryData = DataManager.Instance.GetAccessoryByGradeAndRank(nextGrade, 4);

            if (nextAccessoryData != null)
            {
                Accessory newAccessory = new Accessory(nextAccessoryData);
                PlayerObjManager.Instance.Player.inventory.AccessoryInventory.AddItem(newAccessory);
                return true;
            }
        }
        else
        {
            AccessoryDataSO nextAccessoryData = DataManager.Instance.GetNextAccessory(BaseData.grade, BaseData.rank);

            if (nextAccessoryData != null)
            {
                Accessory newAccessory = new Accessory(nextAccessoryData);
                PlayerObjManager.Instance.Player.inventory.AccessoryInventory.AddItem(newAccessory);
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