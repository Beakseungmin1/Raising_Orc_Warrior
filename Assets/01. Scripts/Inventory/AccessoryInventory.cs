using System.Collections.Generic;

public class AccessoryInventory : IInventory<AccessoryDataSO>
{
    private Dictionary<AccessoryDataSO, int> accessories = new Dictionary<AccessoryDataSO, int>();

    public void AddItem(AccessoryDataSO item)
    {
        if (accessories.ContainsKey(item))
        {
            accessories[item]++;
        }
        else
        {
            accessories[item] = 1;
        }
    }

    public void RemoveItem(AccessoryDataSO item)
    {
        if (accessories.ContainsKey(item))
        {
            accessories[item]--;
            if (accessories[item] <= 0)
            {
                accessories.Remove(item);
            }
        }
    }

    public AccessoryDataSO GetItem(string itemName)
    {
        foreach (var accessory in accessories.Keys)
        {
            if (accessory.itemName == itemName)
                return accessory;
        }
        return null;
    }

    public int GetItemStackCount(AccessoryDataSO item)
    {
        return accessories.TryGetValue(item, out int count) ? count : 0;
    }

    public List<AccessoryDataSO> GetAllItems()
    {
        return new List<AccessoryDataSO>(accessories.Keys);
    }

    public int GetTotalItemCount()
    {
        return accessories.Count;
    }
}