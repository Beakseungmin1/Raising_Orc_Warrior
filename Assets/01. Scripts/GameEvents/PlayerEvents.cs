using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public event Action onPlayerStatChange;

    public void StatChange()
    {
        if (onPlayerStatChange != null)
        {
            onPlayerStatChange();
        }
    }
}
