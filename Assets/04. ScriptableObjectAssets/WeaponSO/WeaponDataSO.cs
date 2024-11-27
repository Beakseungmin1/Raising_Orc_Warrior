using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "New Weapon")]
public class WeaponDataSO : ScriptableObject
{
    [Header("WeaponStat")]
    public float atkIncreaseRate; //���ݷ� ������. ����ȿ���� ����ȿ���� 30%
    public float criticalDamageBonus; //����ȿ��: ġ��Ÿ ������
    public float increaseGoldGainRate; //����ȿ��: ��� ȹ�淮 ���뷮

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string weaponName;
    public string description;
    public Grade grade;
    public int upgradeLevel; //���簭ȭ����
    public int rank; //�ش� �������� ���. 1,2,3,4��� ����.(1����� ���� ����)
    public int requireCubeForUpgrade; //��ȭ�� �ʿ��� ť�귮

    [Header("Stacking")]
    public int curStackAmount;
}
