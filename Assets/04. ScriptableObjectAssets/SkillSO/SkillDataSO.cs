using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill")]
public class SkillDataSO : BaseItemDataSO
{
    [Header("Skill Details")]
    public string description; // ��ų ����
    public string effectDescription; // ��ų ȿ�� ����    
    public SkillType skillType; // ��ų Ÿ�� (��Ƽ��, ����, �нú�)
    public EffectType effectType; // ��ų ����Ʈ Ÿ��

    [Header("Activation Condition")]
    public ActivationCondition activationCondition; // �ߵ� ���� (��ٿ�, ���� Ƚ��)
    public float cooldown; // ��ٿ� �ð�
    public int requiredHits; // �ʿ��� ���� Ƚ��
    public float periodicInterval;

    [Header("Skill Properties")]
    public float damagePercent; // ���ݷ� ����
    public int manaCost; // ���� �Ҹ�
    public float effectRange; // ȿ�� ����
    public float buffDuration; // ���� ���� �ð�
    public float attackIncreasePercent; // ���ݷ� ���� ����

    [Header("Upgrade Info")]
    public int requireSkillCardsForUpgrade; // ���׷��̵忡 �ʿ��� ī���
    public int maxLevel; // �ִ� ����    

    [Header("Visual and Effects")]
    public GameObject effectPrefab; // ����Ʈ ������
}