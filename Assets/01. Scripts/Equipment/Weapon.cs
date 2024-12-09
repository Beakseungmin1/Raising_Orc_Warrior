using UnityEngine;

[System.Serializable]
public class Weapon : IEnhanceable, IFusable
{
    public WeaponDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;

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
        StackCount = 1;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
        EquipAtkIncreaseRate = baseData.equipAtkIncreaseRate;
        PassiveEquipAtkIncreaseRate = Mathf.RoundToInt(baseData.equipAtkIncreaseRate / 3f);
        PassiveCriticalDamageBonus = baseData.passiveCriticalDamageBonus;
        PassiveGoldGainRate = baseData.passiveGoldGainRate;
    }

    public bool CanEnhance()
    {
        int currentCube = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube);
        Debug.Log($"[CanEnhance] 현재 큐브: {currentCube}, 필요한 큐브: {RequiredCurrencyForUpgrade}");

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
        UpdateWeaponEffects();

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

        PlayerInventory playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("플레이어 인벤토리를 찾을 수 없습니다.");
            return false;
        }

        RemoveStack(BaseData.rank);

        WeaponDataSO nextWeaponData = DataManager.Instance.GetNextWeapon(BaseData.grade, BaseData.rank);
        if (nextWeaponData != null)
        {
            Weapon newWeapon = new Weapon(nextWeaponData);
            playerInventory.WeaponInventory.AddItem(newWeapon);

            Debug.Log($"무기 합성 성공! 새로운 무기: {nextWeaponData.itemName} 추가.");
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
}