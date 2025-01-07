using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgrademultiplieBtn : MonoBehaviour
{
    public TextMeshProUGUI OneMultiplieTxt;
    public GameObject TenMultiplieBtn;
    public GameObject HunMultiplieBtn;


    public void OnOneMultiplieBtn()
    {
        if (GetBtnTurnOn())
        {
            PlayerObjManager.Instance.Player.stat.ChangeUpgradeMultiplier(0);
            PlayerObjManager.Instance.Player.stat.UpdateAllStatUI.Invoke();
            TurnOffBtns();
        }
        else
        {
            TurnOnBtns();
        }
    }

    public void OnTenMultiplieBtn()
    {
        PlayerObjManager.Instance.Player.stat.ChangeUpgradeMultiplier(1);
        PlayerObjManager.Instance.Player.stat.UpdateAllStatUI.Invoke();
        TurnOffBtns();
    }

    public void OnHunMultiplieBtn()
    {
        PlayerObjManager.Instance.Player.stat.ChangeUpgradeMultiplier(2);
        PlayerObjManager.Instance.Player.stat.UpdateAllStatUI.Invoke();
        TurnOffBtns();
    }

    public void TurnOnBtns()
    {
        TenMultiplieBtn.SetActive(true);
        HunMultiplieBtn.SetActive(true);
        OneMultiplieTxt.text = "1x";
    }

    public void TurnOffBtns()
    {
        TenMultiplieBtn.SetActive(false);
        HunMultiplieBtn.SetActive(false);

        if (PlayerObjManager.Instance.Player.stat.statUpgradeMultiplier == 0)
        {
            OneMultiplieTxt.text = "1x";
        }
        else if (PlayerObjManager.Instance.Player.stat.statUpgradeMultiplier == 1)
        {
            OneMultiplieTxt.text = "10x";
        }
        else
        {
            OneMultiplieTxt.text = "100x";
        }
    }

    public bool GetBtnTurnOn()
    {
        return TenMultiplieBtn.activeInHierarchy;
    }


}
