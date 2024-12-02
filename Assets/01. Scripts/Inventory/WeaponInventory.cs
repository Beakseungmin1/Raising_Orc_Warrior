using UnityEngine;
using System.Collections.Generic;

public class WeaponInventory : IInventory<WeaponDataSO>
{
    private List<WeaponDataSO> weapons = new List<WeaponDataSO>();

    public void AddItem(WeaponDataSO item)
    {
        weapons.Add(item);
    }

    public void RemoveItem(WeaponDataSO item)
    {
        weapons.Remove(item);
    }

    public WeaponDataSO GetItem(string itemName)
    {
        return weapons.Find(weapon => weapon.itemName == itemName);
    }

    public List<WeaponDataSO> GetAllItems()
    {
        return new List<WeaponDataSO>(weapons);
    }

    public int GetItemCount()
    {
        return weapons.Count;
    }
}