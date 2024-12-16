using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill")]
public class SkillDataSO : BaseItemDataSO
{
    [Header("Skill Details")]
    public string description;
    public string effectDescription;  
    public SkillType skillType;
    public EffectType effectType;

    [Header("Activation Condition")]
    public ActivationCondition activationCondition;
    public float cooldown;
    public int requiredHits;
    public float periodicInterval;

    [Header("Skill Properties")]
    public float damagePercent;
    public int manaCost;
    public float effectRange;
    public float buffDuration;
    public float attackIncreasePercent;

    [Header("Upgrade Info")]
    public int requireSkillCardsForUpgrade;
    public int maxLevel;  

    [Header("Visual and Effects")]
    public GameObject effectPrefab;
}