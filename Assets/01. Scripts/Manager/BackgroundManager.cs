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
        //����� ����ɽ� ���� ����� ����������� ����
        beforeBackground = curBackground;

        //���� é�Ϳ� �´� ������� ����
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

        //�ٲ� ����� ��ũ��Ʈ ��������
        ParallaxBackground = curBackground.GetComponent<ParallaxBackground>();

        //��ũ��Ʈ �÷��̾�� �־��ֱ�
        PlayerObjManager.Instance.Player.PlayerBattle.background = ParallaxBackground;

        //�ٲ� ��� ��� ������ Faid in �� ���۵Ǹ� �� ����
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
