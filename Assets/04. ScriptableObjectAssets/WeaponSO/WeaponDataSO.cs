using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // 착용 효과
    public int equipAtkIncreaseRate; // 공격력 증가율 (장착 효과)

    [Header("Ownership Effects")] // 보유 효과
    public int passiveEquipAtkIncreaseRate; // 보유 효과 공격력 증가율 (장착 효과의 1/3)
    public int passiveCriticalDamageBonus; // 치명타 데미지 (보유 효과)
    public int passiveGoldGainRate; // 골드 획득량 증가율 (보유 효과)

    [Header("General Info")]
    public int rank;

    [Header("Fuse Info")]
    public int requireFuseItemCount = 5;
}