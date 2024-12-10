using UnityEngine;

[System.Serializable]
public class Accessory : IEnhanceable, IFusable
{
    public AccessoryDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;

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
        StackCount = 1;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipHpAndHpRecoveryIncreaseRate = baseData.equipHpAndHpRecoveryIncreaseRate;
        PassiveHpAndHpRecoveryIncreaseRate = Mathf.RoundToInt(baseData.equipHpAndHpRecoveryIncreaseRate / 3f);
        PassiveMpAndMpRecoveryIncreaseRate = baseData.passiveMpAndMpRecoveryIncreaseRate;
        PassiveAddEXPRate = baseData.passiveAddEXPRate;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Cube) >= RequiredCurrencyForUpgrade;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("강화 실패! 큐브가 부족합니다.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, RequiredCurrencyForUpgrade);
        EnhancementLevel++;
        RequiredCurrencyForUpgrade = Mathf.RoundToInt(RequiredCurrencyForUpgrade * 1.5f);
        UpdateAccessoryEffects();

        Debug.Log($"악세사리 {BaseData.itemName} 강화 완료. 현재 레벨: {EnhancementLevel}, 다음 강화 비용: {RequiredCurrencyForUpgrade}");
        return true;
    }

    public bool CanFuse()
    {
        return StackCount >= BaseData.rank;
    }

    public bool Fuse()
    {
        if (!CanFuse())
        {
            Debug.LogWarning("합성 실패! 보유 개수가 부족합니다.");
            return false;
        }

        PlayerInventory playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("플레이어 인벤토리를 찾을 수 없습니다.");
            return false;
        }

        RemoveStack(BaseData.rank);

        AccessoryDataSO nextAccessoryData = DataManager.Instance.GetNextAccessory(BaseData.grade, BaseData.rank);
        if (nextAccessoryData != null)
        {
            Accessory newAccessory = new Accessory(nextAccessoryData);
            playerInventory.AccessoryInventory.AddItem(newAccessory);

            Debug.Log($"악세사리 합성 성공! 새로운 악세사리: {nextAccessoryData.itemName}");
            return true;
        }

        Debug.LogWarning("합성 실패! 다음 단계의 악세사리 데이터를 찾을 수 없습니다.");
        return false;
    }

    public void AddStack(int count)
    {
        StackCount += count;
    }

    public void RemoveStack(int count)
    {
        StackCount -= count;
        if (StackCount < 0) StackCount = 0;
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
}