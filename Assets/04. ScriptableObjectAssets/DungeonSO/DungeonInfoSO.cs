using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultDungeonInfoSO", menuName = "DungeonInfoSO", order = 1)]
public class DungeonInfoSO : ScriptableObject
{
    [Header("Generals")]
    public int level;
    public DungeonType type;

    [Header("Enemies")]
    public EnemySO dungeonBoss;
    public List<EnemySO> enemySOs;

    [Header("Rewards")]
    public float rewardAmount;
}
