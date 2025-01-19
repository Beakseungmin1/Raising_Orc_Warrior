using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_DungeonUI : UIBase
{
    public GameObject goldRedDot;
    public GameObject cubeRedDot;
    public GameObject emeraldRedDot;

    private void OnEnable()
    {
        GameEventsManager.Instance.currencyEvents.onDungeonTicketChanged += ShowOrHideRedDot;
        ShowOrHideRedDot();
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.currencyEvents.onDungeonTicketChanged -= ShowOrHideRedDot;
    }

    public void ShowEmeraldDungeonUI()
    {
        Hide();
        UIManager.Instance.Show<EmeraldDungeonUI>();
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
    public void ShowOrHideRedDot()
    {
        if (CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket) > 0)
        {
            goldRedDot.SetActive(true);
            cubeRedDot.SetActive(true);
            emeraldRedDot.SetActive(true);
        }
        else
        {
            goldRedDot.SetActive(false);
            cubeRedDot.SetActive(false);
            emeraldRedDot.SetActive(false);
        }
    }
}
