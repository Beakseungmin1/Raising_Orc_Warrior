using System.Collections.Generic;

public interface IInventory<T>
{
    void AddItem(T item); // 아이템 추가
    void RemoveItem(T item); // 아이템 삭제
    T GetItem(string itemName); // 이름으로 아이템 검색
    int GetItemStackCount(T item); // 특정 아이템의 스택 개수
    List<T> GetAllItems(); // 모든 아이템 가져오기
    int GetTotalItemCount(); // 전체 아이템 종류 개수
    bool CanAddItem(T item); // 아이템 추가 가능 여부 확인
}