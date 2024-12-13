public interface IFusable
{
    BaseItemDataSO BaseData { get; }
    bool Fuse(int materialCount);
}