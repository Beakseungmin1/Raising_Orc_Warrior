using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public SkillInventory SkillInventory { get; private set; }
    public WeaponInventory WeaponInventory { get; private set; }
    public AccessoryInventory AccessoryInventory { get; private set; }

    public event Action OnSkillsChanged;
    public event Action OnWeaponsChanged;
    public event Action OnAccessoriesChanged;

    private void Awake()
    {
        SkillInventory = new SkillInventory();
        WeaponInventory = new WeaponInventory();
        AccessoryInventory = new AccessoryInventory();
    }

    public void AddItemToInventory(BaseItemDataSO item)
    {
        switch (item)
        {
            case SkillDataSO skillData:
                Skill skill = SkillInventory.GetItem(skillData.itemName) ?? new Skill(skillData);
                SkillInventory.AddItem(skill);
                OnSkillsChanged?.Invoke();
                break;

            case WeaponDataSO weaponData:
                Weapon weapon = WeaponInventory.GetItem(weaponData.itemName) ?? new Weapon(weaponData);
                WeaponInventory.AddItem(weapon);
                OnWeaponsChanged?.Invoke();
                break;

            case AccessoryDataSO accessoryData:
                Accessory accessory = AccessoryInventory.GetItem(accessoryData.itemName) ?? new Accessory(accessoryData);
                AccessoryInventory.AddItem(accessory);
                OnAccessoriesChanged?.Invoke();
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }

    public void RemoveItemFromInventory(BaseItemDataSO item)
    {
        switch (item)
        {
            case SkillDataSO skillData:
                Skill skill = SkillInventory.GetItem(skillData.itemName);
                if (skill != null)
                {
                    SkillInventory.RemoveItem(skill);
                    OnSkillsChanged?.Invoke();
                }
                break;

            case WeaponDataSO weaponData:
                Weapon weapon = WeaponInventory.GetItem(weaponData.itemName);
                if (weapon != null)
                {
                    WeaponInventory.RemoveItem(weapon);
                    OnWeaponsChanged?.Invoke();
                }
                break;

            case AccessoryDataSO accessoryData:
                Accessory accessory = AccessoryInventory.GetItem(accessoryData.itemName);
                if (accessory != null)
                {
                    AccessoryInventory.RemoveItem(accessory);
                    OnAccessoriesChanged?.Invoke();
                }
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }
}