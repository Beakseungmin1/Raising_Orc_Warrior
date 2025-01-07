using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger ���


public class CurrencyManager : Singleton<CurrencyManager>
{
    private BigInteger gold = 99999999999; // �ɷ�ġ ��ȭ�� (���� ����)

    private Dictionary<CurrencyType, float> currencies; // float Ÿ�Ը� ���
    private PlayerStat stat;

    private void Awake()
    {
        // ��ųʸ� �ʱ�ȭ
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

    // Gold �߰�
    public void AddGold(BigInteger amount)
    {
        BigInteger goldMultiplier = stat.extraGoldGainRate;
        BigInteger adjustedAmount = amount + (amount * (goldMultiplier / 100));

        gold += adjustedAmount;
        GameEventsManager.Instance.currencyEvents.GoldChanged();
    }

    // Gold ����
    public void SubtractGold(BigInteger amount)
    {
        gold -= amount;
        if (gold < 0) gold = BigInteger.Zero;
        GameEventsManager.Instance.currencyEvents.GoldChanged();
    }

    // Gold ��ȸ
    public BigInteger GetGold()
    {
        return gold;
    }

    // ��ȭ �߰� (float Ÿ�Ը�)
    public void AddCurrency(CurrencyType type, float amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] += amount;
        }
        GameEventsManager.Instance.currencyEvents.CallCurrencyAsFloatChangedMathod(type);
    }

    // ��ȭ ���� (float Ÿ�Ը�)
    public void SubtractCurrency(CurrencyType type, float amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type] -= amount;
            if (currencies[type] < 0) currencies[type] = 0f;
        }
        GameEventsManager.Instance.currencyEvents.CallCurrencyAsFloatChangedMathod(type);
    }

    // ��ȭ ��ȸ
    public float GetCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type) ? currencies[type] : 0f;
    }
}