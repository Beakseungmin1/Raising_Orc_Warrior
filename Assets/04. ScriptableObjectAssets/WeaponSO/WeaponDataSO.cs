using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // 착용 효과
    public float equipAtkIncreaseRate; // 공격력 증가율 (장착 효과)

    [Header("Ownership Effects")] // 보유 효과
    public float passiveCriticalDamageBonus; // 치명타 데미지 (보유 효과)
    public float passiveGoldGainRate; // 골드 획득량 증가율 (보유 효과)

    [Header("General Info")]
    public int rank;

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // 강화에 필요한 큐브 수
}