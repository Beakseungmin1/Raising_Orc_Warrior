using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPopupUI : UIBase
{
    public void ExitBtn()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}
