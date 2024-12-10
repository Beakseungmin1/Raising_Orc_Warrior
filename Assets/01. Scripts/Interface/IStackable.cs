public interface IStackable
{
    int StackCount { get; } // 현재 스택 개수
    void AddStack(int count); // 스택 추가
    void RemoveStack(int count); // 스택 감소
}