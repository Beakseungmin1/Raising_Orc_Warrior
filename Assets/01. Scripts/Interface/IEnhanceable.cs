public interface IEnhanceable : IStackable
{
    BaseItemDataSO BaseData { get; }
    int EnhancementLevel { get; }
    int RequiredCurrencyForUpgrade { get; }
    bool CanEnhance();
    bool Enhance();
}