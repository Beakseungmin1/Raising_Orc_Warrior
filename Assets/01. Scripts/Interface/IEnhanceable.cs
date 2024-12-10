public interface IEnhanceable : IStackable
{
    BaseItemDataSO BaseData { get; }
    int EnhancementLevel { get; } // ���� ��ȭ ����
    int RequiredCurrencyForUpgrade { get; }
    bool CanEnhance(); // ��ȭ ���� ���� Ȯ��
    bool Enhance(); // ��ȭ ����
}