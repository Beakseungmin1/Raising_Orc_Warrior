using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GenericInventory<BaseSkill> SkillInventory { get; private set; } // Skill → BaseSkill
    public GenericInventory<Weapon> WeaponInventory { get; private set; }
    public GenericInventory<Accessory> AccessoryInventory { get; private set; }

    public event Action<bool> OnInventoryChanged;
    public event Action OnSkillsChanged;

    private void Awake()
    {
        SkillInventory = new GenericInventory<BaseSkill>(); // Skill → BaseSkill
        WeaponInventory = new GenericInventory<Weapon>();
        AccessoryInventory = new GenericInventory<Accessory>();
    }

    public void AddItemToInventory(BaseItemDataSO item)
    {
        switch (item)
        {
            case SkillDataSO skillData:
                AddItem(SkillInventory, CreateSkillInstance(skillData));
                OnSkillsChanged?.Invoke();
                break;

            case WeaponDataSO weaponData:
                AddItem(WeaponInventory, new Weapon(weaponData));
                OnInventoryChanged?.Invoke(true);
                break;

            case AccessoryDataSO accessoryData:
                AddItem(AccessoryInventory, new Accessory(accessoryData));
                OnInventoryChanged?.Invoke(false);
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
                RemoveItem(SkillInventory, skillData.itemName);
                OnSkillsChanged?.Invoke();
                break;

            case WeaponDataSO weaponData:
                RemoveItem(WeaponInventory, weaponData.itemName);
                OnInventoryChanged?.Invoke(true);
                break;

            case AccessoryDataSO accessoryData:
                RemoveItem(AccessoryInventory, accessoryData.itemName);
                OnInventoryChanged?.Invoke(false);
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }

    public void RemoveItemFromInventory(BaseItemDataSO item, int amount)
    {
        switch (item)
        {
            case SkillDataSO skillData:
                for (int i = 0; i < amount; i++)
                {
                    RemoveItem(SkillInventory, skillData.itemName);
                }
                OnSkillsChanged?.Invoke();
                break;

            case WeaponDataSO weaponData:
                for (int i = 0; i < amount; i++)
                {
                    RemoveItem(WeaponInventory, weaponData.itemName);
                }
                OnInventoryChanged?.Invoke(true);
                break;

            case AccessoryDataSO accessoryData:
                for (int i = 0; i < amount; i++)
                {
                    RemoveItem(AccessoryInventory, accessoryData.itemName);
                }
                OnInventoryChanged?.Invoke(false);
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }

    public int GetItemStackCount<T>(T item) where T : class, IEnhanceable
    {
        if (item is BaseSkill) // Skill → BaseSkill
        {
            return SkillInventory.GetItemStackCount(item as BaseSkill);
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

    private void AddItem<T>(GenericInventory<T> inventory, T item) where T : class, IEnhanceable
    {
        if (inventory.GetItem(item.BaseData.itemName) is T existingItem)
        {
            (existingItem as IStackable)?.AddStack(1);
        }
        else
        {
            inventory.AddItem(item);
        }
    }

    private void RemoveItem<T>(GenericInventory<T> inventory, string itemName) where T : class, IEnhanceable
    {
        T item = inventory.GetItem(itemName);
        if (item != null)
        {
            (item as IStackable)?.RemoveStack(1);
            if ((item as IStackable)?.StackCount <= 0)
            {
                inventory.RemoveItem(item);
            }
        }
    }

    private BaseSkill CreateSkillInstance(SkillDataSO skillData)
    {
        var playerStat = PlayerObjManager.Instance.Player?.GetComponent<PlayerStat>();

        if (playerStat == null)
        {
            Debug.LogError("PlayerStat을 찾을 수 없습니다.");
            return null;
        }

        GameObject skillObject = new GameObject(skillData.itemName);
        skillObject.transform.SetParent(PlayerObjManager.Instance.Player.transform);

        BaseSkill skillInstance = null;

        switch (skillData.skillType)
        {
            case SkillType.Active:
                skillInstance = skillObject.AddComponent<ActiveSkill>();
                break;
            case SkillType.Buff:
                skillInstance = skillObject.AddComponent<BuffSkill>();
                break;
            case SkillType.Passive:
                skillInstance = skillObject.AddComponent<PassiveSkill>();
                break;
            default:
                Debug.LogError("알 수 없는 스킬 타입입니다.");
                return null;
        }

        skillInstance.Initialize(skillData, playerStat);
        return skillInstance;
    }

    public void NotifyInventoryChanged(bool isWeapon)
    {
        OnInventoryChanged?.Invoke(isWeapon);
    }

    public void NotifySkillsChanged()
    {
        OnSkillsChanged?.Invoke();
    }
}