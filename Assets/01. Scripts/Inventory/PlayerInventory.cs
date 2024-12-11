using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GenericInventory<Skill> SkillInventory { get; private set; }
    public GenericInventory<Weapon> WeaponInventory { get; private set; }
    public GenericInventory<Accessory> AccessoryInventory { get; private set; }

    public event Action OnSkillsChanged;
    public event Action OnWeaponsChanged;
    public event Action OnAccessoriesChanged;

    private void Awake()
    {
        SkillInventory = new GenericInventory<Skill>();
        WeaponInventory = new GenericInventory<Weapon>();
        AccessoryInventory = new GenericInventory<Accessory>();
    }

    public void AddItemToInventory(BaseItemDataSO item)
    {
        Debug.LogWarning($"[PlayerInventory] AddItemToInventory 호출: {item.itemName}");

        switch (item)
        {
            case SkillDataSO skillData:
                AddItem(SkillInventory, new Skill(skillData), OnSkillsChanged);
                break;

            case WeaponDataSO weaponData:
                AddItem(WeaponInventory, new Weapon(weaponData), OnWeaponsChanged);
                break;

            case AccessoryDataSO accessoryData:
                AddItem(AccessoryInventory, new Accessory(accessoryData), OnAccessoriesChanged);
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
                RemoveItem(SkillInventory, skillData.itemName, OnSkillsChanged);
                break;

            case WeaponDataSO weaponData:
                RemoveItem(WeaponInventory, weaponData.itemName, OnWeaponsChanged);
                break;

            case AccessoryDataSO accessoryData:
                RemoveItem(AccessoryInventory, accessoryData.itemName, OnAccessoriesChanged);
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }

    // 추가된 메서드: 특정 아이템의 스택 수량 가져오기
    public int GetItemStackCount<T>(T item) where T : class, IEnhanceable
    {
        if (item is Skill)
        {
            return SkillInventory.GetItemStackCount(item as Skill);
        }
        else if (item is Weapon)
        {
            return WeaponInventory.GetItemStackCount(item as Weapon);
        }
        else if (item is Accessory)
        {
            return AccessoryInventory.GetItemStackCount(item as Accessory);
        }

        Debug.LogError("지원되지 않는 아이템 유형");
        return 0;
    }

    private void AddItem<T>(GenericInventory<T> inventory, T item, Action onChanged) where T : class, IEnhanceable
    {
        if (inventory.GetItem(item.BaseData.itemName) is T existingItem)
        {
            (existingItem as IStackable)?.AddStack(1); // 스택 증가
            onChanged?.Invoke();
        }
        else
        {
            inventory.AddItem(item);
            onChanged?.Invoke();
        }
    }

    private void RemoveItem<T>(GenericInventory<T> inventory, string itemName, Action onChanged) where T : class, IEnhanceable
    {
        T item = inventory.GetItem(itemName);
        if (item != null)
        {
            (item as IStackable)?.RemoveStack(1); // 스택 감소
            if ((item as IStackable)?.StackCount <= 0)
            {
                inventory.RemoveItem(item); // 스택 0이면 제거
            }
            onChanged?.Invoke();
        }
    }

    public void NotifyWeaponsChanged() => OnWeaponsChanged?.Invoke();
    public void NotifyAccessoriesChanged() => OnAccessoriesChanged?.Invoke();
    public void NotifySkillsChanged() => OnSkillsChanged?.Invoke();
}