using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int curGold = 100000000; // �ɷ�ġ ��ȭ��
    public int curEmerald = 100000000; // ��ų, ���� ��ȭ��
    public int curCube = 100000000; // ��� ��ȭ��
    public int curDiamond = 100000000; // �̱��

    private Dictionary<CurrencyType, int> currencies;

    private void Awake()
    {
        // ��ųʸ� �ʱ�ȭ �� ���� ������ ����ȭ
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
            if (currencies[type] < 0) currencies[type] = 0; // ��ȭ�� 0 ���Ϸ� �������� �ʵ��� ����
            SyncToVariables(type);
        }
    }

    //��ȭ Ÿ�Կ� ���� ���� ��ȯ;
    public int GetCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type) ? currencies[type] : 0;
    }

    private void SyncToVariables(CurrencyType type)
    {
        // ��ųʸ� �� ���� �� ���� ������ ����ȭ
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