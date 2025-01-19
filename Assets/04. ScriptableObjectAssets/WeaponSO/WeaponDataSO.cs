using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // 착용 효과
    public double equipAtkIncreaseRate; // 공격력 증가율 (장착 효과)

    [Header("Ownership Effects")] // 보유 효과
    public int passiveEquipAtkIncreaseRate; // 보유 효과 공격력 증가율 (장착 효과의 1/3)
    public int passiveCriticalDamageBonus; // 치명타 데미지 (보유 효과)
    public int passiveGoldGainRate; // 골드 획득량 증가율 (보유 효과)

    [Header("General Info")]
    public int weaponId;
    public int rank;

    [Header("Fuse Info")]
    public int requireFuseItemCount = 5;

    public Color GetGradeColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.Normal: return Color.white;
            case Grade.Uncommon: return new Color(0.22f, 0.92f, 0.54f);
            case Grade.Rare: return new Color(1f, 0.62f, 0.28f);
            case Grade.Hero: return new Color(0.24f, 0.58f, 1f);
            case Grade.Legendary: return new Color(0.75f, 0.25f, 1f);
            case Grade.Mythic: return new Color(1f, 0.2f, 0.2f);
            case Grade.Ultimate: return new Color(1f, 1f, 0.24f);
            default: return Color.white;
        }
    }
}