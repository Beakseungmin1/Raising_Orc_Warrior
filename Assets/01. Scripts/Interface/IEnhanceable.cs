public interface IEnhanceable : IStackable
{
    BaseItemDataSO BaseData { get; }
    int EnhancementLevel { get; } // 현재 강화 레벨
    int RequiredCurrencyForUpgrade { get; }
    bool CanEnhance(); // 강화 가능 여부 확인
    bool Enhance(); // 강화 실행
}