using UnityEngine;

public class Weapon : MonoBehaviour, IEquipment
{
    public WeaponDataSO weaponDataSO;

    [Header("WeaponStat")]
    public float atkIncreaseRate; //���ݷ� ������. ����ȿ���� ����ȿ���� 30%
    public float criticalDamageBonus; //����ȿ��: ġ��Ÿ ������
    public float increaseGoldGainRate; //����ȿ��: ��� ȹ�淮 ���뷮

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string weaponName;
    public Grade grade;
    public int curLevel = 1; //���簭ȭ����
    [Range(1, 4)] public int rank; //�ش� �������� ���. 1,2,3,4��� ����.(1����� ���� ����)
    public int requireCubeForUpgrade; //��ȭ�� �ʿ��� ť�귮

    [Header("Stacking")]
    public int curStackAmount = 1;

    public void Awake()
    {
        icon = GetComponent<Sprite>();
        inGameImage = GetComponent<Sprite>();

        atkIncreaseRate = weaponDataSO.atkIncreaseRate;
        criticalDamageBonus = weaponDataSO.criticalDamageBonus;
        increaseGoldGainRate = weaponDataSO.increaseGoldGainRate;

        icon = weaponDataSO.icon;
        inGameImage = weaponDataSO.inGameImage;
        weaponName = weaponDataSO.itemName;
        grade = weaponDataSO.grade;
        requireCubeForUpgrade = weaponDataSO.requireCubeForUpgrade;
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
