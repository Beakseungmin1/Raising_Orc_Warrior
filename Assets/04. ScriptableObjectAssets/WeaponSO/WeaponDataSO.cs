using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // 착용 효과
    public float equipAtkIncreaseRate; // 공격력 증가율 (장착 효과)

    [Header("Ownership Effects")] // 보유 효과
    public float passiveEquipAtkIncreaseRate; // 보유 효과 공격력 증가율 (장착 효과의 1/3)
    public float passiveCriticalDamageBonus; // 치명타 데미지 (보유 효과)
    public float passiveGoldGainRate; // 골드 획득량 증가율 (보유 효과)

    [Header("General Info")]
    public int rank;
}