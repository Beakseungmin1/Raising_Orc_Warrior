using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoPopupUI : MonoBehaviour
{
    public void ExitBtn()
    {
        UIManager.Instance.Hide("DimmedImage");
        UIManager.Instance.Hide("PlayerInfoPopupUI");
    }
}
