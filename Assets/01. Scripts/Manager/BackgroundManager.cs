using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    public GameObject beforeBackground;
    public GameObject curBackground;
    public ParallaxBackground ParallaxBackground;

    public void Init()
    {
        if (StageManager.Instance.curChapterIndex == 0)
        {
            curBackground = ObjectPool.Instance.GetObject("ForestBG");
        }
        else if (StageManager.Instance.curChapterIndex == 1)
        {
            curBackground = ObjectPool.Instance.GetObject("VineForestBG");
        }
        else if (StageManager.Instance.curChapterIndex == 2)
        {
            curBackground = ObjectPool.Instance.GetObject("BeachBG");
        }
        else if (StageManager.Instance.curChapterIndex == 3)
        {
            curBackground = ObjectPool.Instance.GetObject("CaveBG");
        }
        ParallaxBackground = curBackground.GetComponent<ParallaxBackground>();
    }

    public void ChangeBackGround()
    {
        StartCoroutine(BGChangeCoroutine());
    }

    private IEnumerator BGChangeCoroutine()
    {
        //배경이 변경될시 현재 배경을 이전배경으로 저장
        beforeBackground = curBackground;

        //현재 챕터에 맞는 배경으로 변경
        if (StageManager.Instance.curChapterIndex == 0)
        {
            curBackground = ObjectPool.Instance.GetObject("ForestBG");
        }
        else if (StageManager.Instance.curChapterIndex == 1)
        {
            curBackground = ObjectPool.Instance.GetObject("VineForestBG");
        }
        else if (StageManager.Instance.curChapterIndex == 2)
        {
            curBackground = ObjectPool.Instance.GetObject("BeachBG");
        }
        else if (StageManager.Instance.curChapterIndex == 3)
        {
            curBackground = ObjectPool.Instance.GetObject("CaveBG");
        }

        //바뀐 배경의 스크립트 가져오기
        ParallaxBackground = curBackground.GetComponent<ParallaxBackground>();

        //스크립트 플레이어에게 넣어주기
        PlayerObjManager.Instance.Player.PlayerBattle.background = ParallaxBackground;

        //바뀐 배경 잠시 꺼진후 Faid in 이 시작되면 맵 변경
        curBackground.SetActive(false);

        yield return new WaitForSeconds(0.8f);

        returnBG();
        curBackground.SetActive(true);
        PlayerObjManager.Instance.Player.PlayerBattle.SetPlayerStateIdle();
    }

    public void returnBG()
    {
        ObjectPool.Instance.ReturnObject(beforeBackground);
    }
}
