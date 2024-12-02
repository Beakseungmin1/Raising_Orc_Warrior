using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int curGold = 0; // 능력치 강화용
    public int curEmerald = 0; // 스킬, 동료 강화용
    public int curCube = 0; // 장비 강화용
    public int curDiamond = 0; // 뽑기용

    private Dictionary<CurrencyType, int> currencies;

    private void Awake()
    {
        // 딕셔너리 초기화 및 기존 변수와 동기화
        currencies = new Dictionary<CurrencyType, int>
        {
            { CurrencyType.Gold, curGold },
            { CurrencyType.Emerald, curEmerald },
            { CurrencyType.Cube, curCube },
            { CurrencyType.Diamond, curDiamond }
        };
    }

    public void AddCurrency(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] += amount;
            SyncToVariables(type);
        }
    }

    public void SubtractCurrency(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] -= amount;
            if (currencies[type] < 0) currencies[type] = 0; // 재화가 0 이하로 내려가지 않도록 보정
            SyncToVariables(type);
        }
    }

    //재화 타입에 따라 값을 반환;
    public int GetCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type) ? currencies[type] : 0;
    }

    private void SyncToVariables(CurrencyType type)
    {
        // 딕셔너리 값 변경 시 기존 변수에 동기화
        switch (type)
        {
            case CurrencyType.Gold:
                curGold = currencies[CurrencyType.Gold];
                break;
            case CurrencyType.Emerald:
                curEmerald = currencies[CurrencyType.Emerald];
                break;
            case CurrencyType.Cube:
                curCube = currencies[CurrencyType.Cube];
                break;
            case CurrencyType.Diamond:
                curDiamond = currencies[CurrencyType.Diamond];
                break;
        }
    }


}