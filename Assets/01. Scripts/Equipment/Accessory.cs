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
            Debug.LogWarning("��ȭ ����! ť�갡 �����մϴ�.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, RequiredCurrencyForUpgrade);
        EnhancementLevel++;
        RequiredCurrencyForUpgrade = Mathf.RoundToInt(RequiredCurrencyForUpgrade * 1.5f);
        UpdateAccessoryEffects();

        Debug.Log($"�Ǽ��縮 {BaseData.itemName} ��ȭ �Ϸ�. ���� ����: {EnhancementLevel}, ���� ��ȭ ���: {RequiredCurrencyForUpgrade}");
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
            Debug.LogWarning("�ռ� ����! ���� ������ �����մϴ�.");
            return false;
        }

        PlayerInventory playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("�÷��̾� �κ��丮�� ã�� �� �����ϴ�.");
            return false;
        }

        RemoveStack(BaseData.rank);

        AccessoryDataSO nextAccessoryData = DataManager.Instance.GetNextAccessory(BaseData.grade, BaseData.rank);
        if (nextAccessoryData != null)
        {
            Accessory newAccessory = new Accessory(nextAccessoryData);
            playerInventory.AccessoryInventory.AddItem(newAccessory);

            Debug.Log($"�Ǽ��縮 �ռ� ����! ���ο� �Ǽ��縮: {nextAccessoryData.itemName}");
            return true;
        }

        Debug.LogWarning("�ռ� ����! ���� �ܰ��� �Ǽ��縮 �����͸� ã�� �� �����ϴ�.");
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