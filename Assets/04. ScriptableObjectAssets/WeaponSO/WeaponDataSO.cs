using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Equipment/New Weapon")]
public class WeaponDataSO : BaseItemDataSO
{
    [Header("Equip Effects")] // ���� ȿ��
    public double equipAtkIncreaseRate; // ���ݷ� ������ (���� ȿ��)

    [Header("Ownership Effects")] // ���� ȿ��
    public int passiveEquipAtkIncreaseRate; // ���� ȿ�� ���ݷ� ������ (���� ȿ���� 1/3)
    public int passiveCriticalDamageBonus; // ġ��Ÿ ������ (���� ȿ��)
    public int passiveGoldGainRate; // ��� ȹ�淮 ������ (���� ȿ��)

    [Header("General Info")]
    public int weaponId;
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