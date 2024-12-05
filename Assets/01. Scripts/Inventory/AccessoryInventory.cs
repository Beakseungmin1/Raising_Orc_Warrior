using System.Collections.Generic;

public class AccessoryInventory : IInventory<Accessory>
{
    private Dictionary<Accessory, int> accessories = new Dictionary<Accessory, int>();

    public void AddItem(Accessory item)
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

    public void RemoveItem(Accessory item)
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

    public Accessory GetItem(string itemName)
    {
        foreach (var accessory in accessories.Keys)
        {
            if (accessory.BaseData.itemName == itemName)
                return accessory;
        }
        return null;
    }

    public int GetItemStackCount(Accessory item)
    {
        return accessories.TryGetValue(item, out int count) ? count : 0;
    }

    public List<Accessory> GetAllItems()
    {
        return new List<Accessory>(accessories.Keys);
    }

    public int GetTotalItemCount()
    {
        return accessories.Count;
    }
}