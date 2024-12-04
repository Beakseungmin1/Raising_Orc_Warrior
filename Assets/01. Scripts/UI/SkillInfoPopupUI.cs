using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoPopupUI : UIBase
{
    public void ExitBtn()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}
