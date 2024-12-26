using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultStageSO", menuName = "StageSO", order = 1)]
public class StageSO : ScriptableObject
{
    public List<EnemySO> enemySOs;

    public string stageName;
    //ex) ∞•∏¡¿«∂• 1,2,3,4,5,6,7
}
