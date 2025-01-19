using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    public GenericInventory<BaseSkill> SkillInventory { get; private set; }
    public GenericInventory<Weapon> WeaponInventory { get; private set; }
    public GenericInventory<Accessory> AccessoryInventory { get; private set; }

    public event Action<bool> OnInventoryChanged;
    public event Action OnSkillsChanged;

    private Transform playerTransform;
    private PlayerStat playerStat;

    private void Awake()
    {
        SkillInventory = new GenericInventory<BaseSkill>();
        WeaponInventory = new GenericInventory<Weapon>();
        AccessoryInventory = new GenericInventory<Accessory>();
    }

    private void Start()
    {
        var player = PlayerObjManager.Instance.Player;
        if (player != null)
        {
            playerTransform = player.transform;
            playerStat = player.stat;
        }
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
                PassiveManager.Instance.UpdateWeaponEffects();
                break;

            case AccessoryDataSO accessoryData:
                AddItem(AccessoryInventory, new Accessory(accessoryData));
                OnInventoryChanged?.Invoke(false);
                PassiveManager.Instance.UpdateAccessoryEffects();
                break;
        }
    }

    public GenericInventory<BaseSkill> GetSkillInventory()
    {
        return SkillInventory;
    }

    public GenericInventory<Weapon> GetWeaponInventory()
    {
        return WeaponInventory;
    }

    public GenericInventory<Accessory> GetAccessoryInventory()
    {
        return AccessoryInventory;
    }

    public void SetSkillSaveDataInventory(SkillSaveData skills)
    {
        SkillDataSO skillData = new SkillDataSO();

        skillData = skills.SkillDataSO;
        for (int i = 0; i < skills.StackCount; i++)
        {
            AddItem(SkillInventory, CreateSkillInstance(skillData));
            //강화레벨 넣는법도 구상
        }
        OnSkillsChanged?.Invoke();
    }

    public void SetWeaponSaveDataInventory(WeaponSaveData weapons)
    {
        WeaponDataSO weaponData = new WeaponDataSO();

        weaponData = weapons.WeaponDataSO;
        for (int i = 0; i < weapons.StackCount; i++)
        {
            AddItem(WeaponInventory, new Weapon(weaponData));
            //강화레벨 넣는법도 구상
        }
        OnInventoryChanged?.Invoke(true);
        PassiveManager.Instance.UpdateWeaponEffects();
    }

    public void SetAccessorySaveDataInventory(AccessorySaveData accessorys)
    {
        AccessoryDataSO accessoryData = new AccessoryDataSO();

        accessoryData = accessorys.AccessoryDataSO;
        for (int i = 0; i < accessorys.StackCount; i++)
        {
            AddItem(AccessoryInventory, new Accessory(accessoryData));
            //강화레벨 넣는법도 구상
        }
        OnInventoryChanged?.Invoke(false);
        PassiveManager.Instance.UpdateAccessoryEffects();
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
        }
    }

    public int GetItemStackCount<T>(T item) where T : class, IEnhanceable
    {
        if (item is BaseSkill)
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
        if (playerStat == null || playerTransform == null)
        {
            return null;
        }

        foreach (Transform child in playerTransform)
        {
            var existingSkill = child.GetComponent<BaseSkill>();
            if (existingSkill != null && existingSkill.SkillData == skillData)
            {
                existingSkill.AddStack(1);
                return existingSkill;
            }
        }

        GameObject skillObject = new GameObject(skillData.itemName);
        skillObject.name = string.IsNullOrEmpty(skillData.englishItemName) ? "Skill" : skillData.englishItemName;
        skillObject.transform.SetParent(playerTransform);

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

    private float CalculateTotalEffect<T>(GenericInventory<T> inventory, Func<T, float> effectSelector) where T : class, IEnhanceable
    {
        HashSet<string> processedItems = new HashSet<string>();
        float totalEffect = 0f;

        foreach (var item in inventory.GetAllItems())
        {
            if (!processedItems.Contains(item.BaseData.itemName))
            {
                totalEffect += effectSelector(item);
                processedItems.Add(item.BaseData.itemName);
            }
        }

        return totalEffect;
    }

    public float GetTotalAccessoryHpAndHpRecovery()
    {
        return CalculateTotalEffect(AccessoryInventory, item => item.PassiveHpAndHpRecoveryIncreaseRate);
    }

    public float GetTotalAccessoryMpAndMpRecovery()
    {
        return CalculateTotalEffect(AccessoryInventory, item => item.PassiveMpAndMpRecoveryIncreaseRate);
    }

    public float GetTotalAccessoryAddEXPRate()
    {
        return CalculateTotalEffect(AccessoryInventory, item => item.PassiveAddEXPRate);
    }

    public float GetTotalWeaponPassiveAtkIncrease()
    {
        return CalculateTotalEffect(WeaponInventory, item => item.PassiveEquipAtkIncreaseRate);
    }

    public float GetTotalWeaponCriticalDamageBonus()
    {
        return CalculateTotalEffect(WeaponInventory, item => item.PassiveCriticalDamageBonus);
    }

    public float GetTotalWeaponGoldGainRate()
    {
        return CalculateTotalEffect(WeaponInventory, item => item.PassiveGoldGainRate);
    }
}