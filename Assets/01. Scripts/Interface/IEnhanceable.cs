public interface IEnhanceable
{
    BaseItemDataSO BaseData { get; }
    int EnhancementLevel { get; }
    int StackCount { get; }
    bool CanEnhance(); // 강화 가능 여부 확인
    bool Enhance(); // 강화 실행
}