using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    //등급별 소환 확률을 딕셔너리로 구분하기
    //DataManager에 있는 Weapon, Acc, Skill의 모든 데이터를 받아온다.
    //소환 매서드가 실행되면 등급에 맞춰 소환한다.

    //소환확률
    private float totalSummonRate = 100f;

    private float normalSummonRate;
    private float uncommonSummonRate;
    private float rareSummonRate;
    private float heroSummonRate;
    private float legendarySummonRate;
    private float mythicSummonRate;
    private float ultimateSummonRate;

    private Dictionary<Grade, float> summonRateByGrade;

    private void Awake()
    {
        SetSummonRate();
    }

    private void SetSummonRate()
    {
        //랭크 1,2,3,4도 대응해야한다!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //랭크 1,2,3,4도 대응해야한다!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //랭크 1,2,3,4도 대응해야한다!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //랭크 1,2,3,4도 대응해야한다!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //랭크 1,2,3,4도 대응해야한다!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        normalSummonRate = totalSummonRate - (ultimateSummonRate + mythicSummonRate + legendarySummonRate + heroSummonRate + rareSummonRate + uncommonSummonRate);
        uncommonSummonRate = 20f;
        rareSummonRate = 10f;
        heroSummonRate = 5f;
        legendarySummonRate = 3f;
        mythicSummonRate = 1f;
        ultimateSummonRate = 0.5f;

        summonRateByGrade = new Dictionary<Grade, float>
        {
            { Grade.Normal, normalSummonRate },
            { Grade.Uncommon, uncommonSummonRate },
            { Grade.Rare, rareSummonRate },
            { Grade.Hero, heroSummonRate },
            { Grade.Legendary, legendarySummonRate },
            { Grade.Mythic, mythicSummonRate },
            { Grade.Ultimate, ultimateSummonRate }
        };

        if (normalSummonRate < 0)
        {
            Debug.LogError("Error: normalSummonRate exceed 100%.");
        }
    }

    private void OnWeaponSummon()
    {
        //데이터매니저에 있는 모든 웨폰 데이터를 뒤진 후
        //등급별 확률에 따라서
        //뽑기를한다.
        //이름이 매칭이 된 경우
        //PlayerInventory에있는 AddItemToInventory()를 써본다.
    }

    private void OnSkillCardSummon()
    {

    }

    private void OnAccessarySummon()
    {

    }
}
