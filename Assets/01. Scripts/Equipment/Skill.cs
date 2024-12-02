using UnityEngine;

[System.Serializable]
public class Skill : IEnhanceable, IFusable
{
    public SkillDataSO BaseData { get; private set; }
    public int EnhancementLevel { get; private set; } // ���� ��ȭ ����
    public int StackCount { get; private set; } // ���� ��ų ���� ����

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
            Debug.LogWarning("��ȭ ����! ��ȭ �Ǵ� ��ų ī�尡 �����մϴ�.");
            return false;
        }

        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, BaseData.requireEmelardForUpgrade);
        StackCount -= BaseData.requireSkillCardsForUpgrade;

        EnhancementLevel++;
        UpdateSkillEffects();
        Debug.Log($"��ų {BaseData.itemName} ��ȭ �Ϸ�. ���� ����: {EnhancementLevel}");
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
            Debug.LogWarning("�ռ� ����! ���� ��ų�� �����մϴ�.");
            return false;
        }

        StackCount -= 2;

        SkillDataSO nextSkill = DataManager.Instance.GetNextSkill(BaseData.grade);
        if (nextSkill != null)
        {
            Debug.Log($"��ų �ռ� ����! ���ο� ��ų: {nextSkill.itemName}");
            return true;
        }

        Debug.LogWarning("�ռ� ����! ���� ����� ��ų �����͸� ã�� �� �����ϴ�.");
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