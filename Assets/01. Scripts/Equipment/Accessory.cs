using UnityEngine;

public class Accessory : MonoBehaviour, IEquipment
{
    public AccessoryDataSO accessoryDataSO;

    [Header("AccessoryStat")]
    public float hpAndHpRecoveryIncreaseRate; //체력, 체력 회복량 증가율이다. 보유효과는 장착효과의 30%
    public float mpAndMpRecoveryIncreaseRate; //보유효과 전체 마나, 마나회복량
    public float addEXPRate; //보유효과 추가 경험치

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string accessoryName;
    public Grade grade;
    public int curLevel = 1; //현재강화레벨
    [Range(1,5)]public int rank; //해당 아이템의 등급. 1,2,3,4등급 있음.(1등급이 가장 높음)
    public int requireCubeForUpgrade; //강화에 필요한 큐브량

    [Header("Stacking")]
    public int curStackAmount = 1; //현재 보유량은 SO에 있는 게 X, 그냥 클래스자체에(악세서리프리팹 컴포넌트

    public void Awake()
    {
        icon = GetComponent<Sprite>();
        inGameImage = GetComponent<Sprite>();

        //hpAndHpRecoveryIncreaseRate = accessoryDataSO.hpAndHpRecoveryIncreaseRate;
        //mpAndMpRecoveryIncreaseRate = accessoryDataSO.mpAndMpRecoveryIncreaseRate;
        //addEXPRate = accessoryDataSO.addEXPRate;
        accessoryName = accessoryDataSO.itemName;
        grade = accessoryDataSO.grade;
        requireCubeForUpgrade = accessoryDataSO.requireCubeForUpgrade;
    }

    public void Equip()
    {

    }

    public void Upgrade()
    {
        curLevel += 1;
    }

    public void UnEquip()
    {

    }

    public void Fusion()
    {

    }

    public void AddStackAmount(int count)
    {
        curStackAmount += count; 
    }

    public void SubtractStackAmount(int count)
    {
        curStackAmount -= count;
    }

}
