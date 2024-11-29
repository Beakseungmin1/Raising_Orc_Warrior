using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int curGold = 0; //�ɷ�ġ ��ȭ��
    public int curEmerald = 0; //��ų, ���� ��ȭ��
    public int curCube = 0; //��� ��ȭ��
    public int curDiamond = 0; //�̱��

    public void AddGold(int addAmount)
    {
        curGold += addAmount;
    }

    public void SubtractGold(int subtractAmount)
    {
        curGold -= subtractAmount;
    }

    public void AddEmerald(int addAmount)
    {
        curEmerald += addAmount;
    }

    public void SubtractEmerald(int subtractAmount)
    {
        curEmerald -= subtractAmount;
    }

    public void AddCube(int addAmount)
    {
        curCube += addAmount;
    }

    public void SubtractCube(int subtractAmount)
    {
        curCube -= subtractAmount;
    }

    public void AddDiamond(int addAmount)
    {
        curDiamond += addAmount;
    }

    public void SubtractDiamond(int subtractAmount)
    {
        curDiamond -= subtractAmount;
    }
}