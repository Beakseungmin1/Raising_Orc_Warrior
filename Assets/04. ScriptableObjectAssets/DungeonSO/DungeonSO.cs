using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultDungeonSO", menuName = "DungeonSO", order = 1)]
public class DungeonSO : ScriptableObject
{
    public EnemySO dungeonBoss;
    public List<EnemySO> enemySOs;

    public string stageName;
}
