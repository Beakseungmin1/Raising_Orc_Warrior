using System.Collections.Generic;

public interface IInventory<T> where T : BaseItemDataSO
{
    void AddItem(T item); // ������ �߰�
    void RemoveItem(T item); // ������ ����
    T GetItem(string itemName); // �̸����� ������ �˻�
    int GetItemStackCount(T item); // Ư�� �������� ���� ����
    List<T> GetAllItems(); // ��� ������ ��������
    int GetTotalItemCount(); // ��ü ������ ���� ����
}