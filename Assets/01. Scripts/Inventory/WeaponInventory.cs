using System.Collections.Generic;

public class WeaponInventory : IInventory<WeaponDataSO>
{
    private Dictionary<WeaponDataSO, int> weapons = new Dictionary<WeaponDataSO, int>();

    public void AddItem(WeaponDataSO item)
    {
        if (weapons.ContainsKey(item))
        {
            weapons[item]++;
        }
        else
        {
            weapons[item] = 1;
        }
    }

    public void RemoveItem(WeaponDataSO item)
    {
        if (weapons.ContainsKey(item))
        {
            weapons[item]--;
            if (weapons[item] <= 0)
            {
                weapons.Remove(item);
            }
        }
    }

    public WeaponDataSO GetItem(string itemName)
    {
        foreach (var weapon in weapons.Keys)
        {
            if (weapon.itemName == itemName)
                return weapon;
        }
        return null;
    }

    public int GetItemStackCount(WeaponDataSO item)
    {
        return weapons.TryGetValue(item, out int count) ? count : 0;
    }

    public List<WeaponDataSO> GetAllItems()
    {
        return new List<WeaponDataSO>(weapons.Keys);
    }

    public int GetTotalItemCount()
    {
        return weapons.Count;
    }
}