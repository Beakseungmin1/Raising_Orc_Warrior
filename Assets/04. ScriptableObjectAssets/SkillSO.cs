using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName; // ��ų �̸�
    public string description; // ��ų ����

    [Header("Skill Properties")]
    public float cooldown; // ��ٿ� �ð�
    public int manaCost; // ���� �Ҹ�

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // ��ų ȿ�� ������
    public float damage; // ��ų ������
    public float range; // ��ų ����
}