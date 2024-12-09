using UnityEngine;

[System.Serializable]
public class Skill : IEnhanceable, IFusable
{
    public SkillDataSO BaseData { get; private set; }
    BaseItemDataSO IEnhanceable.BaseData => BaseData;

    public int EnhancementLevel { get; private set; }
    public int StackCount { get; private set; }
    public float Cooldown { get; private set; }
    public float DamagePercent { get; private set; }
    public float BuffDuration { get; private set; }
    public int RequiredCurrencyForUpgrade { get; private set; }

    public Skill(SkillDataSO baseData)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = 1;
        Cooldown = baseData.cooldown;
        DamagePercent = baseData.damagePercent;
        BuffDuration = baseData.buffDuration;
        RequiredCurrencyForUpgrade = baseData.requiredCurrencyForUpgrade;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald) >= RequiredCurrencyForUpgrade &&
               StackCount >= BaseData.requireSkillCardsForUpgrade;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("강화 실패! 재화 또는 스킬 카드가 부족합니다.");
            return false;
        }

        // 강화 조건 만족 시, 재화와 카드 수량 차감
        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, BaseData.requiredCurrencyForUpgrade);
        StackCount -= BaseData.requireSkillCardsForUpgrade;

        // 강화 레벨 증가
        EnhancementLevel++;

        // 런타임 데이터 업데이트
        UpdateSkillEffects();

        Debug.Log($"스킬 {BaseData.itemName} 강화 완료. 현재 레벨: {EnhancementLevel}");
        return true;
    }

    private void UpdateSkillEffects()
    {
        Cooldown = Mathf.Max(0, BaseData.cooldown - EnhancementLevel * 0.1f);
        DamagePercent = BaseData.damagePercent + EnhancementLevel * 0.05f;
        BuffDuration = BaseData.buffDuration + EnhancementLevel * 0.2f;
    }

    public bool CanFuse()
    {
        return StackCount >= 2;
    }

    public bool Fuse()
    {
        if (!CanFuse())
        {
            Debug.LogWarning("합성 실패! 동일 스킬이 부족합니다.");
            return false;
        }

        PlayerInventory playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("플레이어 인벤토리를 찾을 수 없습니다.");
            return false;
        }

        StackCount -= 2;

        SkillDataSO nextSkill = DataManager.Instance.GetNextSkill(BaseData.grade);
        if (nextSkill != null)
        {
            Skill newSkill = new Skill(nextSkill);
            playerInventory.SkillInventory.AddItem(newSkill);

            Debug.Log($"스킬 합성 성공! 새로운 스킬: {nextSkill.itemName}");
            return true;
        }

        Debug.LogWarning("합성 실패! 다음 등급의 스킬 데이터를 찾을 수 없습니다.");
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