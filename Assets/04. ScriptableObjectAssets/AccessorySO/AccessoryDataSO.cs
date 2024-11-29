using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // 착용 효과
    public float equipHpAndHpRecoveryIncreaseRate; // 체력 및 체력 회복량 증가율 (장착 효과)

    [Header("Ownership Effects")] // 보유 효과
    public float passiveMpAndMpRecoveryIncreaseRate; // 마나 및 마나 회복량 증가율 (보유 효과)
    public float passiveAddEXPRate; // 추가 경험치 증가율 (보유 효과)

    [Header("General Info")]
    public int rank;

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // 강화에 필요한 큐브 수
}