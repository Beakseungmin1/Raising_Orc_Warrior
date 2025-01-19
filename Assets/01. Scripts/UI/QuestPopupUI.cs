using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPopupUI : UIBase
{
    public void ExitBtn()
    {
        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);
        Hide();
    }
}
