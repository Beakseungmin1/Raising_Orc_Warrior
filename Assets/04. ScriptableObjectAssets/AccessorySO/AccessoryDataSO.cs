using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : BaseItemSO
{
    [Header("Accessory Stats")]
    public float hpAndHpRecoveryIncreaseRate; // ü��, ü�� ȸ���� ������
    public float mpAndMpRecoveryIncreaseRate; // ����, ���� ȸ���� ������
    public float addEXPRate; // �߰� ����ġ ������

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // ��ȭ�� �ʿ��� ť�� ��
}