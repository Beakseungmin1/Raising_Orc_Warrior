using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // ���� ȿ��
    public double equipHpAndHpRecoveryIncreaseRate; // ü�� �� ü�� ȸ���� ������ (���� ȿ��)

    [Header("Ownership Effects")] // ���� ȿ��
    public int passiveHpAndHpRecoveryIncreaseRate;
    public int passiveMpAndMpRecoveryIncreaseRate; // ���� �� ���� ȸ���� ������ (���� ȿ��)
    public int passiveAddEXPRate; // �߰� ����ġ ������ (���� ȿ��)

    [Header("General Info")]
    public int rank;

    [Header("Fuse Info")]
    public int requireFuseItemCount = 5;
}