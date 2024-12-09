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
            Debug.LogWarning("��ȭ ����! ��ȭ �Ǵ� ��ų ī�尡 �����մϴ�.");
            return false;
        }

        // ��ȭ ���� ���� ��, ��ȭ�� ī�� ���� ����
        CurrencyManager.Instance.SubtractCurrency(CurrencyType.Emerald, BaseData.requiredCurrencyForUpgrade);
        StackCount -= BaseData.requireSkillCardsForUpgrade;

        // ��ȭ ���� ����
        EnhancementLevel++;

        // ��Ÿ�� ������ ������Ʈ
        UpdateSkillEffects();

        Debug.Log($"��ų {BaseData.itemName} ��ȭ �Ϸ�. ���� ����: {EnhancementLevel}");
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
            Debug.LogWarning("�ռ� ����! ���� ��ų�� �����մϴ�.");
            return false;
        }

        PlayerInventory playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("�÷��̾� �κ��丮�� ã�� �� �����ϴ�.");
            return false;
        }

        StackCount -= 2;

        SkillDataSO nextSkill = DataManager.Instance.GetNextSkill(BaseData.grade);
        if (nextSkill != null)
        {
            Skill newSkill = new Skill(nextSkill);
            playerInventory.SkillInventory.AddItem(newSkill);

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