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
            Debug.LogWarning("강화 실패! 큐브가 부족합니다.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, BaseData.requireCubeForUpgrade);
        EnhancementLevel++;
        Debug.Log($"무기 {BaseData.itemName} 강화 완료. 현재 레벨: {EnhancementLevel}");
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

        StackCount -= BaseData.rank;

        WeaponDataSO nextWeapon = DataManager.Instance.GetNextWeapon(BaseData.grade, BaseData.rank);
        if (nextWeapon != null)
        {
            Debug.Log($"무기 합성 성공! 새로운 무기: {nextWeapon.itemName}");
            return true;
        }

        Debug.LogWarning("합성 실패! 다음 단계의 무기 데이터를 찾을 수 없습니다.");
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