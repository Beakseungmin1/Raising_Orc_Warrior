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
                Debug.LogError("�������� �ʴ� ������");
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
                Debug.LogError("�������� �ʴ� ������");
                break;
        }
    }

    private void AddItem<T>(GenericInventory<T> inventory, T item, Action onChanged) where T : class
    {
        if (inventory.GetItem((item as IEnhanceable).BaseData.itemName) is T existingItem)
        {
            (existingItem as IStackable)?.AddStack(1); // ���� ����
            onChanged?.Invoke();
        }
        else
        {
            inventory.AddItem(item);
            onChanged?.Invoke();
        }
    }

    private void RemoveItem<T>(GenericInventory<T> inventory, string itemName, Action onChanged) where T : class
    {
        T item = inventory.GetItem(itemName);
        if (item != null)
        {
            (item as IStackable)?.RemoveStack(1); // ���� ����
            if ((item as IStackable)?.StackCount <= 0)
            {
                inventory.RemoveItem(item); // ���� 0�̸� ����
            }
            onChanged?.Invoke();
        }
    }

    public void NotifyWeaponsChanged() => OnWeaponsChanged?.Invoke();
    public void NotifyAccessoriesChanged() => OnAccessoriesChanged?.Invoke();
    public void NotifySkillsChanged() => OnSkillsChanged?.Invoke();
}