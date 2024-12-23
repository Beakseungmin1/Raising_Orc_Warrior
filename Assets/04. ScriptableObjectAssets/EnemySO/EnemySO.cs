using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemys/Enemy")]
public class EnemySO : ScriptableObject
{

    [Header("Enemy information")]
    public string enemyCode; // ���ĺ��ڵ�
    public string hpString; // ü��
    public string maxHpString; // �ִ�ü��
    public string giveExpString; // �ִ� ����ġ
    public GameObject model; //�� ��

    public BigInteger hp;
    public BigInteger maxHp;
    public BigInteger giveExp;

    [Header("Skill Properties")]
    public float cooldown; // ��ٿ� �ð� (������ ��ų�� ���ϰ� ������ ���)

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // ��ų ȿ�� ������
    public Collider2D effectRange; // ��ų ȿ�� ����
    public float damagePercent; // ��Ƽ�� ��ų: ���� �� ������ �ִ� ���ݷ� ���� (%)

    private void OnValidate() //�ν�����â�� �ö���� �� ȣ�� �Ǵ� �ڵ�
    {
        hp = BigInteger.Parse(hpString);
        maxHp = BigInteger.Parse(maxHpString);
        giveExp = BigInteger.Parse(giveExpString);
    }
}