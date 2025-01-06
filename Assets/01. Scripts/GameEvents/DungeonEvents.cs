using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEvents
{
    public event Action<bool, bool> onPlayerDeadOrTimeEndInDungeon;

    public void PlayerDeadOrTimeEndInDungeon(bool isCleared, bool isPlayerDead)
    {
        if (onPlayerDeadOrTimeEndInDungeon != null)
        {
            onPlayerDeadOrTimeEndInDungeon(isCleared, isPlayerDead);
        }
    }
}
