using UnityEngine;

[System.Serializable]
public class Accessory : IEnhanceable, IFusable
{
    public AccessoryDataSO BaseData { get; private set; }
    public int EnhancementLevel { get; private set; } // 현재 강화 레벨
    public int StackCount { get; private set; } // 동일 악세사리 보유 개수

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
            Debug.LogWarning("강화 실패! 큐브가 부족합니다.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Cube, BaseData.requiredCurrencyForUpgrade);
        EnhancementLevel++;
        Debug.Log($"악세사리 {BaseData.itemName} 강화 완료. 현재 레벨: {EnhancementLevel}");
        return true;
    }

    public bool CanFuse()
    {
        return StackCount >= BaseData.rank; // Rank별 요구 개수
    }

    public bool Fuse()
    {
        if (!CanFuse())
        {
            Debug.LogWarning("합성 실패! 보유 개수가 부족합니다.");
            return false;
        }

        StackCount -= BaseData.rank;

        AccessoryDataSO nextAccessory = DataManager.Instance.GetNextAccessory(BaseData.grade, BaseData.rank);
        if (nextAccessory != null)
        {
            Debug.Log($"악세사리 합성 성공! 새로운 악세사리: {nextAccessory.itemName}");
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
}