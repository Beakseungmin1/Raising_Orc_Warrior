using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger 사용


public class CurrencyManager : Singleton<CurrencyManager>
{
    private BigInteger gold = 99999999999; // 능력치 강화용 (전역 변수)

    private Dictionary<CurrencyType, float> currencies; // float 타입만 사용
    private PlayerStat stat;

    private void Awake()
    {
        // 딕셔너리 초기화
        currencies = new Dictionary<CurrencyType, float>
        {
            { CurrencyType.Emerald, 99999f }, // float
            { CurrencyType.Cube, 99999f },    // float
            { CurrencyType.Diamond, 99999f }, // float
            { CurrencyType.DungeonTicket, 10f } // float
        };
    }

    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;
    }

    // Gold 추가
    public void AddGold(BigInteger amount)
    {
        BigInteger goldMultiplier = stat.extraGoldGainRate;
        BigInteger adjustedAmount = amount + (amount * (goldMultiplier / 100));

        gold += adjustedAmount;
        GameEventsManager.Instance.currencyEvents.GoldChanged();
    }

    // Gold 차감
    public void SubtractGold(BigInteger amount)
    {
        gold -= amount;
        if (gold < 0) gold = BigInteger.Zero;
        GameEventsManager.Instance.currencyEvents.GoldChanged();
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
        GameEventsManager.Instance.currencyEvents.CallCurrencyAsFloatChangedMathod(type);
    }

    // 재화 차감 (float 타입만)
    public void SubtractCurrency(CurrencyType type, float amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] -= amount;
            if (currencies[type] < 0) currencies[type] = 0f;
        }
        GameEventsManager.Instance.currencyEvents.CallCurrencyAsFloatChangedMathod(type);
    }

    // 재화 조회
    public float GetCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type) ? currencies[type] : 0f;
    }
}