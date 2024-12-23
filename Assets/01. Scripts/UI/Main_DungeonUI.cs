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
}
