using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBtnUI : UIBase
{
    public void ShowQuestPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<QuestPopupUI>();
    }
}
