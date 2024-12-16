using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger 사용


public class CurrencyManager : Singleton<CurrencyManager>
{
    public BigInteger gold = 100000000; // 능력치 강화용 (전역 변수)

    private Dictionary<CurrencyType, float> currencies; // float 타입만 사용

    private void Awake()
    {
        // 딕셔너리 초기화
        currencies = new Dictionary<CurrencyType, float>
        {
            { CurrencyType.Emerald, 100000000f }, // float
            { CurrencyType.Cube, 100000000f },    // float
            { CurrencyType.Diamond, 100000000f } // float
        };
    }

    // Gold 추가
    public void AddGold(BigInteger amount)
    {
        gold += amount;
    }

    // Gold 차감
    public void SubtractGold(BigInteger amount)
    {
        gold -= amount;
        if (gold < 0) gold = BigInteger.Zero;
    }

    // Gold 조회
    public BigInteger GetGold()
    {
        return gold;
    }

    // 재화 추가 (float 타입만)
    public void AddCurrency(CurrencyType type, float amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] += amount;
        }
    }

    // 재화 차감 (float 타입만)
    public void SubtractCurrency(CurrencyType type, float amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] -= amount;
            if (currencies[type] < 0) currencies[type] = 0f;
        }
    }

    // 재화 조회
    public float GetCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type) ? currencies[type] : 0f;
    }
}