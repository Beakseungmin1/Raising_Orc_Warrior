using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Equipment/New Accessory")]
public class AccessoryDataSO : ScriptableObject
{
    [Header("AccessoryStat")]
    public float hpAndHpRecoveryIncreaseRate; //체력, 체력 회복량 증가율이다. 보유효과는 장착효과의 30%
    public float mpAndMpRecoveryIncreaseRate; //보유효과 전체 마나, 마나회복량
    public float addEXPRate; //보유효과 추가 경험치

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string accessoryName;
    public Grade grade;
    public int upgradeLevel; //현재강화레벨
    public int rank; //해당 아이템의 등급. 1,2,3,4등급 있음.(1등급이 가장 높음)
    public int requireCubeForUpgrade; //강화에 필요한 큐브량

    [Header("Stacking")]
    public int curStackAmount;
}
