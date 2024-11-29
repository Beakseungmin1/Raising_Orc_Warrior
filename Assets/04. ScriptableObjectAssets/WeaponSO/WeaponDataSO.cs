using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemSO
{
    [Header("Weapon Stats")]
    public float atkIncreaseRate; // ���ݷ� ������
    public float criticalDamageBonus; // ġ��Ÿ ������
    public float increaseGoldGainRate; // ��� ȹ�淮 ������

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // ��ȭ�� �ʿ��� ť�� ��
}