using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemys/Enemy")]
public class EnemySO : ScriptableObject
{

    [Header("Enemy information")]
    public string enemyCode; // ���ĺ��ڵ�
    public float hp; // ü��
    public float maxHp; // �ִ�ü��
    public int giveExp; // �ִ� ����ġ
    public Sprite sprite; //�� �̹���

    [Header("Skill Properties")]
    public float cooldown; // ��ٿ� �ð� (������ ��ų�� ���ϰ� ������ ���)

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // ��ų ȿ�� ������
    public Collider2D effectRange; // ��ų ȿ�� ����
    public float damagePercent; // ��Ƽ�� ��ų: ���� �� ������ �ִ� ���ݷ� ���� (%)

    
}