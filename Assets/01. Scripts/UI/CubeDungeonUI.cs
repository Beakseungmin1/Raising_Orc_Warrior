using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDungeonUI : UIBase
{
    
    public void ExitBtn()
    {
        Hide();
        UIManager.Instance.Show<Main_DungeonUI>();
    }
}
