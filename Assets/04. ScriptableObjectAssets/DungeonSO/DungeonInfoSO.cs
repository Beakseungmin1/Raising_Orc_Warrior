using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultDungeonInfoSO", menuName = "DungeonInfoSO", order = 1)]
public class DungeonInfoSO : ScriptableObject
{
    public string id;
    public int level;
    public DungeonType type;

    public EnemySO dungeonBoss;
    public List<EnemySO> enemySOs;

    public string stageName;

    public CurrencyType currenyType;
    public float rewardAmount;
}
