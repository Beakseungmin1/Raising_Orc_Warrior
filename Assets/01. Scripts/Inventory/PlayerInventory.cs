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
            case SkillDataSO skill:
                SkillInventory.AddItem(skill);
                OnSkillsChanged?.Invoke();
                break;

            case WeaponDataSO weapon:
                WeaponInventory.AddItem(weapon);
                OnWeaponsChanged?.Invoke();
                break;

            case AccessoryDataSO accessory:
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
            case SkillDataSO skill:
                SkillInventory.RemoveItem(skill);
                OnSkillsChanged?.Invoke();
                break;

            case WeaponDataSO weapon:
                WeaponInventory.RemoveItem(weapon);
                OnWeaponsChanged?.Invoke();
                break;

            case AccessoryDataSO accessory:
                AccessoryInventory.RemoveItem(accessory);
                OnAccessoriesChanged?.Invoke();
                break;

            default:
                Debug.LogError("지원되지 않는 아이템");
                break;
        }
    }
}