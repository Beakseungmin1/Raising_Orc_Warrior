using UnityEngine;

public class Accessory : MonoBehaviour, IEquipment
{
    public AccessoryDataSO accessoryDataSO;

    [Header("AccessoryStat")]
    public float hpAndHpRecoveryIncreaseRate; //ü��, ü�� ȸ���� �������̴�. ����ȿ���� ����ȿ���� 30%
    public float mpAndMpRecoveryIncreaseRate; //����ȿ�� ��ü ����, ����ȸ����
    public float addEXPRate; //����ȿ�� �߰� ����ġ

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string accessoryName;
    public Grade grade;
    public int curLevel = 1; //���簭ȭ����
    [Range(1,5)]public int rank; //�ش� �������� ���. 1,2,3,4��� ����.(1����� ���� ����)
    public int requireCubeForUpgrade; //��ȭ�� �ʿ��� ť�귮

    [Header("Stacking")]
    public int curStackAmount = 1; //���� �������� SO�� �ִ� �� X, �׳� Ŭ������ü��(�Ǽ����������� ������Ʈ

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
