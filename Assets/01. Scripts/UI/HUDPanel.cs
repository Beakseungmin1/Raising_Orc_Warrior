using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDPanel : MonoBehaviour
{
    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show("DimmedImage");
        UIManager.Instance.Show("PlayerInfoPopupUI");
    }
}
