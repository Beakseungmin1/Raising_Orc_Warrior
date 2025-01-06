using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEvents
{
    public event Action<bool, bool> onPlayerFinishDungeon;

    public void PlayerFinishDungeon(bool isCleared, bool isPlayerDead)
    {
        if (onPlayerFinishDungeon != null)
        {
            onPlayerFinishDungeon(isCleared, isPlayerDead);
        }
    }
}
