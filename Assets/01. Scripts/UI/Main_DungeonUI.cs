using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_DungeonUI : UIBase
{
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
