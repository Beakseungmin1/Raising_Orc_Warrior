using UnityEngine;

[System.Serializable]
public class Skill : IEnhanceable, IFusable
{
    public SkillDataSO BaseData { get; private set; }
    public int EnhancementLevel { get; private set; } // 현재 강화 레벨
    public int StackCount { get; private set; } // 동일 스킬 보유 개수

    public Skill(SkillDataSO baseData)
    {
        BaseData = baseData;
        EnhancementLevel = 0;
        StackCount = 1;
    }

    public bool CanEnhance()
    {
        return CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald) >= BaseData.requireEmelardForUpgrade &&
               StackCount >= BaseData.requireSkillCardsForUpgrade;
    }

    public bool Enhance()
    {
        if (!CanEnhance())
        {
            Debug.LogWarning("강화 실패! 재화 또는 스킬 카드가 부족합니다.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, BaseData.requireEmelardForUpgrade);
        StackCount -= BaseData.requireSkillCardsForUpgrade;

        EnhancementLevel++;
        UpdateSkillEffects();
        Debug.Log($"스킬 {BaseData.itemName} 강화 완료. 현재 레벨: {EnhancementLevel}");
        return true;
    }

    private void UpdateSkillEffects()
    {
        BaseData.cooldown -= EnhancementLevel * 0.1f;
        BaseData.damagePercent += EnhancementLevel * 0.05f;
        BaseData.buffDuration += EnhancementLevel * 0.2f;
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

        StackCount -= 2;

        SkillDataSO nextSkill = DataManager.Instance.GetNextSkill(BaseData.grade);
        if (nextSkill != null)
        {
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