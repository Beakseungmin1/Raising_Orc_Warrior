using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEvents
{
    public event Action<bool> onPlayerFinishDungeon;

    public void PlayerFinishDungeon(bool isCleared)
    {
        if (onPlayerFinishDungeon != null)
        {
            onPlayerFinishDungeon(isCleared);
        }
    }
}
