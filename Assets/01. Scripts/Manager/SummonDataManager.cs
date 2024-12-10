using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataManager : Singleton<SummonDataManager>
{
    //현재 소환중인 아이템 타입이 무엇인가
    public ItemType curSummoningItemType;

    //소환 레벨
    //소환 레벨에 따른 등급, 랭크별 소환 확률, 소환 레벨 경험치만 관리하는 스크립트.

    //소환 레벨
    public int weaponSummonLevel;
    public int skillSummonLevel;
    public int accessorySummonLevel;

    //소환 레벨 경험치
    public int curWeaponSummonEXP;
    public int curSkillSummonEXP;
    public int curAccessorySummonEXP;

    //소환 레벨 최대 경험치
    public int maxWeaponSummonEXP;
    public int maxSkillSummonEXP;
    public int maxAccessorySummonEXP;

    // 등급별 소환 확률 변수
    public float normalGradeSummonRate = 60.5f;
    public float uncommonGradeSummonRate = 20f;
    public float rareGradeSummonRate = 10f;
    public float heroGradeSummonRate = 5f;
    public float legendaryGradeSummonRate = 3f;
    public float mythicGradeSummonRate = 1f;
    public float ultimateGradeSummonRate = 0.5f;

    //4,3,2,1랭크에 따른 소환 확률
    public float rank4SummonRate = 65f;
    public float rank3SummonRate = 20f;
    public float rank2SummonRate = 10f;
    public float rank1SummonRate = 5f;
}
