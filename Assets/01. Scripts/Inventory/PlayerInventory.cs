using System;
using System.Collections.Generic;
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



    public void SetSkillInventory(List<BaseSkill> skillInven)
    {
        foreach (BaseSkill item in skillInven)
        {
            for (int i = 0; i < item.StackCount; i++)
            {
                AddItem(SkillInventory, CreateSkillInstance(item.SkillData));
            }
            Debug.Log("씨발");
        }
        OnSkillsChanged?.Invoke();
    }

    public void SetTestSkillInventory(SkillSaveData skills)
    {
        for(int i = 0; i < skills.StackCount; i++)
        {
            AddItem(SkillInventory, CreateSkillInstance(skills.skillData));
            //인챈트레벨 넣는법도 구상
        }
        OnSkillsChanged?.Invoke();
    }

    public void SetWeaponInventory(GenericInventory<Weapon> weaponInven)
    {
        foreach (var item in weaponInven.GetRealItems())
        {
            AddItem(WeaponInventory, item);
        }
        OnInventoryChanged?.Invoke(true);
        PassiveManager.Instance.UpdateWeaponEffects();
    }

    public void SetAccessoryInventory(GenericInventory<Accessory> accessoryinven)
    {
        foreach (var item in accessoryinven.GetRealItems())
        {
            AddItem(AccessoryInventory, item);
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

    /// <summary>
    /// 아이템 소지 여부 체크
    /// </summary>
    public bool HasRequiredItem(string itemKey, bool isWeapon)
    {
        // 아이템 소지 여부를 확인. 예: "Rare3"를 얻으려면 "Rare4"가 있어야 함
        string[] parts = itemKey.Split(new[] { "Normal", "Uncommon", "Rare", "Hero", "Legendary", "Mythic", "Ultimate" }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 1) return true; // 잘못된 입력 처리

        string grade = itemKey.Substring(0, itemKey.Length - 1); // 등급 추출 (예: "Rare")
        int rank = int.Parse(parts[0]); // 랭크 추출 (예: "3")

        // 노말, 언커먼, 레어 등급 아이템은 이전 단계가 없어도 소환 가능
        if (grade == "Normal" || grade == "Uncommon" || grade == "Rare")
        {
            return true;
        }

        // Rank4는 최상위 단계이므로 제한 없음
        if (rank == 4) return true;

        // 이전 단계 아이템 이름 생성
        string requiredItem = $"{grade}{rank + 1}";

        // 무기 또는 악세서리에 따라 소지 여부 확인
        if (isWeapon)
        {
            // 무기 인벤토리에서 이전 등급 확인
            return PlayerObjManager.Instance.Player.inventory.WeaponInventory.GetItem(requiredItem) != null;
        }
        else
        {
            // 악세서리 인벤토리에서 이전 등급 확인
            return PlayerObjManager.Instance.Player.inventory.AccessoryInventory.GetItem(requiredItem) != null;
        }
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