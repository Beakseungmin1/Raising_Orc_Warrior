using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "New Weapon")]
public class WeaponDataSO : ScriptableObject
{
    [Header("WeaponStat")]
    public float atkIncreaseRate; //공격력 증가율. 보유효과는 장착효과의 30%
    public float criticalDamageBonus; //보유효과: 치명타 데미지
    public float increaseGoldGainRate; //보유효과: 골드 획득량 증대량

    [Header("Info")]
    public Sprite icon;
    public Sprite inGameImage;
    public string weaponName;
    public string description;
    public Grade grade;
    public int upgradeLevel; //현재강화레벨
    public int rank; //해당 아이템의 등급. 1,2,3,4등급 있음.(1등급이 가장 높음)
    public int requireCubeForUpgrade; //강화에 필요한 큐브량

    [Header("Stacking")]
    public int curStackAmount;
}
