using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // ���� ȿ��
    public float equipHpAndHpRecoveryIncreaseRate; // ü�� �� ü�� ȸ���� ������ (���� ȿ��)

    [Header("Ownership Effects")] // ���� ȿ��
    public float passiveMpAndMpRecoveryIncreaseRate; // ���� �� ���� ȸ���� ������ (���� ȿ��)
    public float passiveAddEXPRate; // �߰� ����ġ ������ (���� ȿ��)

    [Header("General Info")]
    public int rank;

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // ��ȭ�� �ʿ��� ť�� ��
}