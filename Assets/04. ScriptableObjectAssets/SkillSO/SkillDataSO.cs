using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/New Skill")]
public class SkillDataSO : BaseItemDataSO
{
    [Header("Skill Details")]
    public string description; // ��ų ����
    public SkillType skillType; // ��ų Ÿ�� (��Ƽ��, ����, �нú�)
    public EffectType effectType; // ��ų ����Ʈ Ÿ��

    [Header("Activation Condition")]
    public ActivationCondition activationCondition; // �ߵ� ���� (��ٿ�, ���� Ƚ��)

    [Header("Skill Properties")]
    public float cooldown; // ��ٿ� �ð�
    public int requiredHits; // �ʿ��� ���� Ƚ��
    public float periodicInterval; // �ֱ��� �ߵ� �ð� (�нú� ��ų)
    public int manaCost; // ���� �Ҹ�

    [Header("Skill Effects")]
    public GameObject effectPrefab; // ����Ʈ ������
    public float effectRange; // ȿ�� ����
    public float damagePercent; // ���ݷ� ����
    public float buffDuration; // ���� ���� �ð�
    public float attackIncreasePercent; // ���ݷ� ���� ����

    [Header("Upgrade Info")]
    public int requireEmelardForUpgrade; // ���׷��̵忡 �ʿ��� ���޶��� ��
    public int requireSkillCardsForUpgrade; // ���׷��̵忡 �ʿ��� ��ų ī�� ��
}