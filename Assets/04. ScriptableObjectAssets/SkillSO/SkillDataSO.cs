using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill")]
[ES3Serializable]
public class SkillDataSO : BaseItemDataSO
{
    [Header("Skill Details")]
    public int SkillId;
    public string description;
    public string effectDescription;
    public SkillType skillType;
    public EffectType effectType;

    [Header("Activation Condition")]
    public ActivationCondition activationCondition;
    public float cooldown;
    public int requiredHits;

    [Header("Skill Properties")]
    public float damagePercent;
    public int manaCost;
    public float effectRange;
    public float buffDuration;
    public float attackIncreasePercent;

    [Header("Recovery and Speed")]
    public float manaRecoveryAmount;
    public float hpRecoveryAmount;
    public float moveSpeedIncrease;
    public float attackSpeedIncrease;

    [Header("Upgrade Info")]
    public int requireSkillCardsForUpgrade;
    public int maxLevel;

    [Header("Visual and Effects")]
    public GameObject effectPrefab;
    public float effectDuration;

    [Header("Equipped State")]
    public bool isEquipped;    
}