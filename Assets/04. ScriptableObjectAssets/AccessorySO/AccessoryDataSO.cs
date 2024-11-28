using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : BaseItemSO
{
    [Header("Accessory Stats")]
    public float hpAndHpRecoveryIncreaseRate; // 체력, 체력 회복량 증가율
    public float mpAndMpRecoveryIncreaseRate; // 마나, 마나 회복량 증가율
    public float addEXPRate; // 추가 경험치 증가율

    [Header("Upgrade Info")]
    public int requireCubeForUpgrade; // 강화에 필요한 큐브 수
}