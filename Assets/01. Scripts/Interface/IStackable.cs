public interface IStackable
{
    int StackCount { get; }
    void AddStack(int count);
    void RemoveStack(int count);
}