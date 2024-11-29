using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // ���� ȿ��
    public float equipAtkIncreaseRate; // ���ݷ� ������ (���� ȿ��)

    [Header("Ownership Effects")] // ���� ȿ��
    public float passiveCriticalDamageBonus; // ġ��Ÿ ������ (���� ȿ��)
    public float passiveGoldGainRate; // ��� ȹ�淮 ������ (���� ȿ��)

    [Header("General Info")]
    public int rank;

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // ��ȭ�� �ʿ��� ť�� ��
}