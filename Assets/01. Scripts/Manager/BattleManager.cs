using System;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    public event Action OnBattleStart;
    public event Action OnBattleEnd;

    private bool isBattleActive = false;

    public void StartBattle()
    {
        if (isBattleActive) return;

        isBattleActive = true;
        if (OnBattleStart != null)
            OnBattleStart.Invoke();
    }

    public void EndBattle()
    {
        if (!isBattleActive) return;

        isBattleActive = false;
        if (OnBattleEnd != null)
            OnBattleEnd.Invoke();
    }

    public bool IsBattleActive => isBattleActive;
}