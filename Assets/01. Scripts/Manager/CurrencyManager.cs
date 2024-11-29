using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : Singleton<CurrencyManager>
{
    public int curGold = 0; //능력치 강화용
    public int curEmerald = 0; //스킬, 동료 강화용
    public int curCube = 0; //장비 강화용
    public int curDiamond = 0; //뽑기용

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