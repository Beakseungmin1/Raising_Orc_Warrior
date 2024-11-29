using UnityEngine;

public class PlayerInventory
{
    public SkillInventory SkillInventory { get; private set; }
    public WeaponInventory WeaponInventory { get; private set; }
    public AccessoryInventory AccessoryInventory { get; private set; }

    public PlayerInventory()
    {
        SkillInventory = new SkillInventory();
        WeaponInventory = new WeaponInventory();
        AccessoryInventory = new AccessoryInventory();
    }

    public void AddItemToInventory(BaseItemSO item)
    {
        switch (item)
        {
            case SkillSO skill:
                SkillInventory.AddItem(skill);
                break;

            case WeaponDataSO weapon:
                WeaponInventory.AddItem(weapon);
                break;

            case AccessoryDataSO accessory:
                AccessoryInventory.AddItem(accessory);
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }
}