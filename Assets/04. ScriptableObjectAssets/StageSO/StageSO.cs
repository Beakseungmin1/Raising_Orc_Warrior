using UnityEngine;

[CreateAssetMenu(fileName = "DefaultStageSO", menuName = "StageSO", order = 1)]
public class StageSO : ScriptableObject
{
    public EnemySO[] enemySOs;

    public string chapterName;
    //ex) ∞•∏¡¿«∂• 1,2,3,4,5,6,7
}
