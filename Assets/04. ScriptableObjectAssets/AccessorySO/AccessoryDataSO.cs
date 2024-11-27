using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : ScriptableObject
{
    [Header("AccessoryStat")]
    public float hpAndHpRecoveryIncreaseRate; //ü��, ü�� ȸ���� �������̴�. ����ȿ���� ����ȿ���� 30%
    public float mpAndMpRecoveryIncreaseRate; //����ȿ�� ��ü ����, ����ȸ����
    public float addEXPRate; //����ȿ�� �߰� ����ġ

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string accessoryName;
    public Grade grade;
    public int upgradeLevel; //���簭ȭ����
    public int rank; //�ش� �������� ���. 1,2,3,4��� ����.(1����� ���� ����)
    public int requireCubeForUpgrade; //��ȭ�� �ʿ��� ť�귮

    [Header("Stacking")]
    public int curStackAmount;
}
