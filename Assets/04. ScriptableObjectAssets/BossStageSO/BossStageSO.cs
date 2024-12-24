using UnityEngine;

[CreateAssetMenu(fileName = "DefaultBossStageSO", menuName = "BossStageSO", order = 1)]
public class BossStageSO : ScriptableObject
{
    public EnemySO bossEnemySO;

    public string stageName;
    //ex) ∞•∏¡¿«∂• 1,2,3,4,5,6,7
}
