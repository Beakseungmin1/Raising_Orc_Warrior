using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    //��޺� ��ȯ Ȯ���� ��ųʸ��� �����ϱ�
    //DataManager�� �ִ� Weapon, Acc, Skill�� ��� �����͸� �޾ƿ´�.
    //��ȯ �ż��尡 ����Ǹ� ��޿� ���� ��ȯ�Ѵ�.

    //��ȯȮ��
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
        //��ũ 1,2,3,4�� �����ؾ��Ѵ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //��ũ 1,2,3,4�� �����ؾ��Ѵ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //��ũ 1,2,3,4�� �����ؾ��Ѵ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //��ũ 1,2,3,4�� �����ؾ��Ѵ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //��ũ 1,2,3,4�� �����ؾ��Ѵ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
        //�����͸Ŵ����� �ִ� ��� ���� �����͸� ���� ��
        //��޺� Ȯ���� ����
        //�̱⸦�Ѵ�.
        //�̸��� ��Ī�� �� ���
        //PlayerInventory���ִ� AddItemToInventory()�� �ẻ��.
    }

    private void OnSkillCardSummon()
    {

    }

    private void OnAccessarySummon()
    {

    }
}
