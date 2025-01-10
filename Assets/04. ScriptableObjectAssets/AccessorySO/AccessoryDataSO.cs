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
}