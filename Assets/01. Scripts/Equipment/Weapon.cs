using UnityEngine;

public class Weapon : MonoBehaviour, IEquipment
{
    public WeaponDataSO weaponDataSO;

    [Header("WeaponStat")]
    public float atkIncreaseRate; //공격력 증가율. 보유효과는 장착효과의 30%
    public float criticalDamageBonus; //보유효과: 치명타 데미지
    public float increaseGoldGainRate; //보유효과: 골드 획득량 증대량

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string weaponName;
    public Grade grade;
    public int curLevel = 1; //현재강화레벨
    [Range(1, 4)] public int rank; //해당 아이템의 등급. 1,2,3,4등급 있음.(1등급이 가장 높음)
    public int requireCubeForUpgrade; //강화에 필요한 큐브량

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
