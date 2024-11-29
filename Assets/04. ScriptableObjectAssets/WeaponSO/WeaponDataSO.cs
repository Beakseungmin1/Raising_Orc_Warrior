using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemSO
{
    [Header("Weapon Stats")]
    public float atkIncreaseRate; // 공격력 증가율
    public float criticalDamageBonus; // 치명타 데미지
    public float increaseGoldGainRate; // 골드 획득량 증가율

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // 강화에 필요한 큐브 수
}