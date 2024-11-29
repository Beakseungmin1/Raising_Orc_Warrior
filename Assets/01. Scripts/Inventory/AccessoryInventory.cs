using System.Collections.Generic;

public class AccessoryInventory : IInventory<AccessoryDataSO>
{
    private List<AccessoryDataSO> accessories = new List<AccessoryDataSO>();

    public void AddItem(AccessoryDataSO item)
    {
        accessories.Add(item);
    }

    public void RemoveItem(AccessoryDataSO item)
    {
        accessories.Remove(item);
    }

    public AccessoryDataSO GetItem(string itemName)
    {
        return accessories.Find(accessory => accessory.itemName == itemName);
    }

    public List<AccessoryDataSO> GetAllItems()
    {
        return new List<AccessoryDataSO>(accessories);
    }

    public int GetItemCount()
    {
        return accessories.Count;
    }
}