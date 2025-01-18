using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEvents
{
    public event Action<bool> onPlayerDeadOrTimeEndInDungeon;

    public void PlayerDeadOrTimeEndInDungeon(bool isCleared)
    {
        if (onPlayerDeadOrTimeEndInDungeon != null)
        {
            onPlayerDeadOrTimeEndInDungeon(isCleared);
        }
    }

    public event Action onDungeonUIChanged;
    public void DungeonUIChanged()
    {
        if (onDungeonUIChanged != null)
        {
            onDungeonUIChanged();
        }
    }

}
