using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataManager : Singleton<SummonDataManager>
{
    //���� ��ȯ���� ������ Ÿ���� �����ΰ�
    public ItemType curSummoningItemType;

    //��ȯ ����
    //��ȯ ������ ���� ���, ��ũ�� ��ȯ Ȯ��, ��ȯ ���� ����ġ�� �����ϴ� ��ũ��Ʈ.

    //��ȯ ����
    public int weaponSummonLevel;
    public int skillSummonLevel;
    public int accessorySummonLevel;

    //��ȯ ���� ����ġ
    public int curWeaponSummonEXP;
    public int curSkillSummonEXP;
    public int curAccessorySummonEXP;

    //��ȯ ���� �ִ� ����ġ
    public int maxWeaponSummonEXP;
    public int maxSkillSummonEXP;
    public int maxAccessorySummonEXP;

    // ��޺� ��ȯ Ȯ�� ����
    public float normalGradeSummonRate = 60.5f;
    public float uncommonGradeSummonRate = 20f;
    public float rareGradeSummonRate = 10f;
    public float heroGradeSummonRate = 5f;
    public float legendaryGradeSummonRate = 3f;
    public float mythicGradeSummonRate = 1f;
    public float ultimateGradeSummonRate = 0.5f;

    //4,3,2,1��ũ�� ���� ��ȯ Ȯ��
    public float rank4SummonRate = 65f;
    public float rank3SummonRate = 20f;
    public float rank2SummonRate = 10f;
    public float rank1SummonRate = 5f;
}
