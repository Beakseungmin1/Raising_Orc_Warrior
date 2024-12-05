using UnityEngine;

[System.Serializable]
public class Accessory : IEnhanceable, IFusable
{
    public AccessoryDataSO BaseData { get; private set; }
    public int EnhancementLevel { get; private set; } // ���� ��ȭ ����
    public int StackCount { get; private set; } // ���� �Ǽ��縮 ���� ����

    public Accessory(AccessoryDataSO baseData)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = 1;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Cube) >= BaseData.requiredCurrencyForUpgrade;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("��ȭ ����! ť�갡 �����մϴ�.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, BaseData.requiredCurrencyForUpgrade);
        EnhancementLevel++;
        Debug.Log($"�Ǽ��縮 {BaseData.itemName} ��ȭ �Ϸ�. ���� ����: {EnhancementLevel}");
        return true;
    }

    public bool CanFuse()
    {
        return StackCount >= BaseData.rank; // Rank�� �䱸 ����
    }

    public bool Fuse()
    {
        if (!CanFuse())
        {
            Debug.LogWarning("�ռ� ����! ���� ������ �����մϴ�.");
            return false;
        }

        StackCount -= BaseData.rank;

        AccessoryDataSO nextAccessory = DataManager.Instance.GetNextAccessory(BaseData.grade, BaseData.rank);
        if (nextAccessory != null)
        {
            Debug.Log($"�Ǽ��縮 �ռ� ����! ���ο� �Ǽ��縮: {nextAccessory.itemName}");
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
}