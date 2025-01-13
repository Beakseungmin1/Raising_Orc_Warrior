public interface IFusable : IEnhanceable
{
    bool Fuse(int materialCount);
    int Rank { get; }
    Grade Grade { get; }
}