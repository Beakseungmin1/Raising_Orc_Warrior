public interface IEnhanceable
{
    BaseItemDataSO BaseData { get; }
    int EnhancementLevel { get; }
    int StackCount { get; }
    bool CanEnhance(); // ��ȭ ���� ���� Ȯ��
    bool Enhance(); // ��ȭ ����
}