using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // 착용 효과
    public double equipHpAndHpRecoveryIncreaseRate; // 체력 및 체력 회복량 증가율 (장착 효과)

    [Header("Ownership Effects")] // 보유 효과
    public int passiveHpAndHpRecoveryIncreaseRate;
    public int passiveMpAndMpRecoveryIncreaseRate; // 마나 및 마나 회복량 증가율 (보유 효과)
    public int passiveAddEXPRate; // 추가 경험치 증가율 (보유 효과)

    [Header("General Info")]
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