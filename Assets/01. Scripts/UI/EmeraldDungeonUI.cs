using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldDungeonUI : UIBase
{
    
    public void ExitBtn()
    {
        Hide();
        UIManager.Instance.Show<Main_DungeonUI>();
    }
}
