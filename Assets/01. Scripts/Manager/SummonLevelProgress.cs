using UnityEngine;

public class SummonLevelProgress
{
    //소환 레벨
    public int Level { get; set; }
    //소환 레벨 경험치
    public float Exp { get; set; }
    //소환 레벨 최대 경험치
    public float ExpToNextLevel { get; set; }

    public SummonLevelProgress(int level, float exp, float expToNextLevel)
    {
        Level = level;
        Exp = exp;
        ExpToNextLevel = expToNextLevel;
    }
}