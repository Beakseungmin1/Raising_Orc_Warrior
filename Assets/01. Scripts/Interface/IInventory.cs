using System.Collections.Generic;

public interface IInventory<T> where T : BaseItemSO
{
    void AddItem(T item); // ������ �߰�
    void RemoveItem(T item); // ������ ����
    T GetItem(string itemName); // �̸����� ������ �˻�
    List<T> GetAllItems(); // ��� ������ ��������
    int GetItemCount(); // ���� ������ ����
}