using System.Collections.Generic;

public class WeaponInventory : IInventory<Weapon>
{
    private Dictionary<Weapon, int> weapons = new Dictionary<Weapon, int>();
    private int maxInventorySize = 100;

    public void AddItem(Weapon item)
    {
        if (CanAddItem(item))
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
    }

    public void RemoveItem(Weapon item)
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

    public Weapon GetItem(string itemName)
    {
        foreach (var weapon in weapons.Keys)
        {
            if (weapon.BaseData.itemName == itemName)
                return weapon;
        }
        return null;
    }

    public int GetItemStackCount(Weapon item)
    {
        return weapons.TryGetValue(item, out int count) ? count : 0;
    }

    public List<Weapon> GetAllItems()
    {
        return new List<Weapon>(weapons.Keys);
    }

    public int GetTotalItemCount()
    {
        return weapons.Count;
    }

    public bool CanAddItem(Weapon item)
    {
        if (weapons.ContainsKey(item))
        {
            return true;
        }
        return weapons.Count < maxInventorySize;
    }
}