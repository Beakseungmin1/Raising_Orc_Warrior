using UnityEngine;

[System.Serializable]
public class Weapon : IEnhanceable, IFusable
{
    public WeaponDataSO BaseData { get; private set; }
    public int EnhancementLevel { get; private set; }
    public int StackCount { get; private set; }

    public Weapon(WeaponDataSO baseData)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = 1;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Cube) >= BaseData.requireCubeForUpgrade;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("��ȭ ����! ť�갡 �����մϴ�.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, BaseData.requireCubeForUpgrade);
        EnhancementLevel++;
        Debug.Log($"���� {BaseData.itemName} ��ȭ �Ϸ�. ���� ����: {EnhancementLevel}");
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

        StackCount -= BaseData.rank;

        WeaponDataSO nextWeapon = DataManager.Instance.GetNextWeapon(BaseData.grade, BaseData.rank);
        if (nextWeapon != null)
        {
            Debug.Log($"���� �ռ� ����! ���ο� ����: {nextWeapon.itemName}");
            return true;
        }

        Debug.LogWarning("�ռ� ����! ���� �ܰ��� ���� �����͸� ã�� �� �����ϴ�.");
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