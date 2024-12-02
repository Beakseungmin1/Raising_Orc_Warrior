using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoPopupUI : MonoBehaviour
{
    public void ExitBtn()
    {
        UIManager.Instance.Hide("DimmedImage");
        UIManager.Instance.Hide("SkillInfoPopupUI");
    }
}
