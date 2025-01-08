using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    public GameObject curBackground;

    public void Init()
    {
        curBackground = ObjectPool.Instance.GetObject("ForestBG");
    }
}
