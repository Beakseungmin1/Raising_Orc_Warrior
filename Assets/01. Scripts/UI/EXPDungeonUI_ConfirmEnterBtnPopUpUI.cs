using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPDungeonUI_ConfirmEnterBtnPopUpUI : UIBase
{
    public void ExitBtn()
    {
        Hide();
        UIManager.Instance.Hide<DimmedUI>();
    }
}
