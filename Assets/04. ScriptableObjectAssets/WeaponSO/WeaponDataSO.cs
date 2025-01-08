using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // ���� ȿ��
    public int equipAtkIncreaseRate; // ���ݷ� ������ (���� ȿ��)

    [Header("Ownership Effects")] // ���� ȿ��
    public int passiveEquipAtkIncreaseRate; // ���� ȿ�� ���ݷ� ������ (���� ȿ���� 1/3)
    public int passiveCriticalDamageBonus; // ġ��Ÿ ������ (���� ȿ��)
    public int passiveGoldGainRate; // ��� ȹ�淮 ������ (���� ȿ��)

    [Header("General Info")]
    public int rank;

    [Header("Fuse Info")]
    public int requireFuseItemCount = 5;
}