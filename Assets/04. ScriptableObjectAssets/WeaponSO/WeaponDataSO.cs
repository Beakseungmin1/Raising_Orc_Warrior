using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // ���� ȿ��
    public float equipAtkIncreaseRate; // ���ݷ� ������ (���� ȿ��)

    [Header("Ownership Effects")] // ���� ȿ��
    public float passiveEquipAtkIncreaseRate; // ���� ȿ�� ���ݷ� ������ (���� ȿ���� 1/3)
    public float passiveCriticalDamageBonus; // ġ��Ÿ ������ (���� ȿ��)
    public float passiveGoldGainRate; // ��� ȹ�淮 ������ (���� ȿ��)

    [Header("General Info")]
    public int rank;

    [Header("Fuse Info")]
    public int requireFuseItemCount = 5;
}