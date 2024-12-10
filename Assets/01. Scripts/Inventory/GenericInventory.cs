using System.Collections.Generic;
using UnityEngine;

public class GenericInventory<T> : IInventory<T> where T : class
{
    private Dictionary<T, int> items = new Dictionary<T, int>(); // �����۰� ���� ī��Ʈ ����

    public void AddItem(T item)
    {
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items[item] = 1;
        }
    }

    public void RemoveItem(T item)
    {
        if (items.ContainsKey(item))
        {
            items[item] = Mathf.Max(items[item] - 1, 0); // ���� ����
            if (items[item] <= 0)
            {
                items.Remove(item); // ������ 0�̸� ����
            }
        }
    }

    public T GetItem(string itemName)
    {
        foreach (var item in items.Keys)
        {
            if ((item as IEnhanceable)?.BaseData.itemName == itemName) // IEnhanceable �������̽� ���
                return item;
        }
        return null;
    }

    public int GetItemStackCount(T item)
    {
        return items.TryGetValue(item, out int count) ? count : 0;
    }

    public List<T> GetAllItems()
    {
        return new List<T>(items.Keys);
    }

    public int GetTotalItemCount()
    {
        return items.Count;
    }

    public bool CanAddItem(T item)
    {
        return true; // �׻� ������ �߰� ����
    }
}