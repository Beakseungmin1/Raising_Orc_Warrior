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
                if (SkillInventory.CanAddItem(skill))
                {
                    SkillInventory.AddItem(skill);
                    OnSkillsChanged?.Invoke();
                }
                else
                {
                    Debug.LogWarning("스킬 인벤토리에 더 이상 아이템을 추가할 수 없습니다.");
                }
                break;

            case WeaponDataSO weaponData:
                Weapon weapon = WeaponInventory.GetItem(weaponData.itemName) ?? new Weapon(weaponData);
                if (WeaponInventory.CanAddItem(weapon))
                {
                    WeaponInventory.AddItem(weapon);
                    OnWeaponsChanged?.Invoke();
                }
                else
                {
                    Debug.LogWarning("무기 인벤토리에 더 이상 아이템을 추가할 수 없습니다.");
                }
                break;

            case AccessoryDataSO accessoryData:
                Accessory accessory = AccessoryInventory.GetItem(accessoryData.itemName) ?? new Accessory(accessoryData);
                if (AccessoryInventory.CanAddItem(accessory))
                {
                    AccessoryInventory.AddItem(accessory);
                    OnAccessoriesChanged?.Invoke();
                }
                else
                {
                    Debug.LogWarning("악세사리 인벤토리에 더 이상 아이템을 추가할 수 없습니다.");
                }
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
                    NotifySkillsChanged();
                }
                break;

            case WeaponDataSO weaponData:
                Weapon weapon = WeaponInventory.GetItem(weaponData.itemName);
                if (weapon != null)
                {
                    WeaponInventory.RemoveItem(weapon);
                    OnWeaponsChanged?.Invoke();
                    NotifyWeaponsChanged();
                }
                break;

            case AccessoryDataSO accessoryData:
                Accessory accessory = AccessoryInventory.GetItem(accessoryData.itemName);
                if (accessory != null)
                {
                    AccessoryInventory.RemoveItem(accessory);
                    OnAccessoriesChanged?.Invoke();
                    NotifyAccessoriesChanged();
                }
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }

    public void NotifyWeaponsChanged()
    {
        OnWeaponsChanged?.Invoke();
    }

    public void NotifyAccessoriesChanged()
    {
        OnAccessoriesChanged?.Invoke();
    }

    public void NotifySkillsChanged()
    {
        OnSkillsChanged?.Invoke();
    }
}