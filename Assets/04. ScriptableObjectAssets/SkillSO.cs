using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName; // ��ų �̸�
    public string description; // ��ų ����

    [Header("Skill Type")]
    public SkillType skillType; // ��ų Ÿ�� (��Ƽ��, ����, �нú�)

    [Header("Activation Condition")]
    public ActivationCondition activationCondition; // �ߵ� ���� (��ٿ�, ���� Ƚ��)

    [Header("Skill Properties")]
    public float cooldown; // ��ٿ� �ð� (��ٿ� ����� �� ��ȿ)
    public int requiredHits; // �ʿ��� ���� Ƚ�� (���� Ƚ�� ����� �� ��ȿ)
    public int manaCost; // ���� �Ҹ�

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // ��ų ȿ�� ������
    public float effectRange; // ��ų ȿ�� ����
    public float damagePercent; // ��Ƽ�� ��ų: ���� �� ������ �ִ� ���ݷ� ���� (%)
    public float buffDuration; // ���� ���� �ð� (���� ��ų���� ��ȿ)
    public float attackIncreasePercent; // ���� �Ǵ� �нú�: ���ݷ� ���� ���� (%)


    [Header("Gacha Data")]
    public Grade skillGrade; // ��ų ���
    public int currentSkillLevel; // ���� ��ų ����
    public int requireEmelardForUpgrade; // ���׷��̵忡 �ʿ��� ���޶��� ��
    public int requiredSkillCardsForUpgrade; // ���׷��̵忡 �ʿ��� ��ų ī�� ��
    public int currentStackAmount; // ���� ��� �ִ� ��ų ī�� ��
}