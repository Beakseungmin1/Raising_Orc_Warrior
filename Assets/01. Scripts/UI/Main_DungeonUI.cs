using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_DungeonUI : UIBase
{
    public GameObject goldRedDot;
    public GameObject cubeRedDot;
    public GameObject expRedDot;

    private void OnEnable()
    {
        if (CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket) > 0)
        {
            goldRedDot.SetActive(true);
            cubeRedDot.SetActive(true);
            expRedDot.SetActive(true);
        }
        else
        {
            goldRedDot.SetActive(false);
            cubeRedDot.SetActive(false);
            expRedDot.SetActive(false);
        }
    }

    public void ShowEXPDungeonUI()
    {
        Hide();
        UIManager.Instance.Show<EXPDungeonUI>();
    }
    public void ShowGoldDungeonUI()
    {
        Hide();
        UIManager.Instance.Show<GoldDungeonUI>();
    }
    public void ShowCubeDungeonUI()
    {
        Hide();
        UIManager.Instance.Show<CubeDungeonUI>();
    }
}
