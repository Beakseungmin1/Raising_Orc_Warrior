using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
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
