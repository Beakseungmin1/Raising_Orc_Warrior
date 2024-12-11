using System.Collections.Generic;
using UnityEngine;

public class GenericInventory<T> : IInventory<T> where T : class, IEnhanceable
{
    private List<T> items;

    public GenericInventory()
    {
        items = new List<T>();
    }

    public void AddItem(T item)
    {
        var existingItem = items.Find(i => i.BaseData.itemName == item.BaseData.itemName);
        if (existingItem != null)
        {
            (existingItem as IStackable)?.AddStack(1);
        }
        else
        {
            items.Add(item);
        }
    }

    public void RemoveItem(T item)
    {
        var existingItem = items.Find(i => i.BaseData.itemName == item.BaseData.itemName);
        if (existingItem != null)
        {
            (existingItem as IStackable)?.RemoveStack(1);
            if ((existingItem as IStackable)?.StackCount <= 0)
            {
                items.Remove(existingItem);
            }
        }
    }

    public T GetItem(string itemName)
    {
        return items.Find(i => i.BaseData.itemName == itemName);
    }

    public int GetItemStackCount(T item)
    {
        var existingItem = items.Find(i => i.BaseData.itemName == item.BaseData.itemName);
        return (existingItem as IStackable)?.StackCount ?? 0;
    }

    public List<T> GetAllItems()
    {
        return new List<T>(items);
    }

    public int GetTotalItemCount()
    {
        return items.Count;
    }

    public bool CanAddItem(T item)
    {
        return true;
    }
}