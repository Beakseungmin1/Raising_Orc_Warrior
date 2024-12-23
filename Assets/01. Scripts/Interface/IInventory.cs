using System.Collections.Generic;

public interface IInventory<T>
{
    void AddItem(T item);
    void RemoveItem(T item);
    T GetItem(string itemName);
    int GetItemStackCount(T item);
    List<T> GetAllItems();
    int GetTotalItemCount();
    bool CanAddItem(T item);
}