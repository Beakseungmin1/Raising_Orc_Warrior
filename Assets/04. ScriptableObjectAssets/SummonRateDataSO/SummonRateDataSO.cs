using UnityEngine;


[CreateAssetMenu(fileName = "DefaultSummonRateDataSO", menuName = "SummonRateDataSO", order = 1)]
public class SummonRateDataSO : ScriptableObject
{
    // 등급별 소환 확률 변수
    [Header("RateByGrade")]
    public float normalGradeSummonRate = 60.5f;
    public float uncommonGradeSummonRate = 20f;
    public float rareGradeSummonRate = 10f;
    public float heroGradeSummonRate = 5f;
    public float legendaryGradeSummonRate = 3f;
    public float mythicGradeSummonRate = 1f;
    public float ultimateGradeSummonRate = 0.5f;

    //4,3,2,1랭크에 따른 소환 확률
    [Header("RateByRank")]
    public float rank4SummonRate = 65f;
    public float rank3SummonRate = 20f;
    public float rank2SummonRate = 10f;
    public float rank1SummonRate = 5f;
}
