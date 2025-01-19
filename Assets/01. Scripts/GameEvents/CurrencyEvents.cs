using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyEvents
{
    public void CallCurrencyAsFloatChangedMathod(CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.Emerald:
                EmeraldChanged();
                break;
            case CurrencyType.Diamond:
                DiamondChanged();
                break;
            case CurrencyType.Cube:
                CubeChanged();
                break;
        }
    }

    public event Action onGoldChanged;

    public void GoldChanged()
    {
        if (onGoldChanged != null)
        {
            onGoldChanged();
        }
    }

    public event Action onEmeraldChanged;

    public void EmeraldChanged()
    {
        if (onEmeraldChanged != null)
        {
            onEmeraldChanged();
        }
    }

    public event Action onDiamondChanged;

    public void DiamondChanged()
    {
        if (onDiamondChanged != null)
        {
            onDiamondChanged();
        }
    }

    public event Action onCubeChanged;

    public void CubeChanged()
    {
        if (onCubeChanged != null)
        {
            onCubeChanged();
        }
    }

    public event Action onDungeonTicketChanged;

    public void DungeonTicketChanged()
    {
        if (onDungeonTicketChanged != null)
        {
            onDungeonTicketChanged();
        }
    }
}
