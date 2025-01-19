using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_SkillUI : UIBase
{
    public void ShowSkillInfoPopupUI()
    {
        UIManager.Instance.Show<SkillInfoPopupUI>();
    }
}
