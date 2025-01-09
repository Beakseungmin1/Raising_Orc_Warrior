public interface IEnhanceable : IStackable
{
    BaseItemDataSO BaseData { get; }
    int EnhancementLevel { get; set; }
    int RequiredCurrencyForUpgrade { get; }
    bool CanEnhance();
    bool Enhance();
}