using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    public GameObject beforeBackground;
    public GameObject curBackground;
    public ParallaxBackground ParallaxBackground;
    public GameObject map;

    public void Init()
    {
        curBackground = ObjectPool.Instance.GetObject("ForestBG");
        ParallaxBackground = curBackground.GetComponent<ParallaxBackground>();
    }

    public void ChangeBackGround(string map)
    {
        StartCoroutine(BGChangeCoroutine(map));
    }

    private IEnumerator BGChangeCoroutine(string map)
    {
        beforeBackground = curBackground;
        curBackground = ObjectPool.Instance.GetObject(map);
        ParallaxBackground = curBackground.GetComponent<ParallaxBackground>();
        PlayerObjManager.Instance.Player.PlayerBattle.background = ParallaxBackground;
        curBackground.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        returnBG();
        curBackground.SetActive(true);
    }

    public void returnBG()
    {
        ObjectPool.Instance.ReturnObject(beforeBackground);
    }
}
