using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgrademultiplieBtn : MonoBehaviour
{
    public GameObject OneMultiplieBtn;
    public GameObject TenMultiplieBtn;
    public GameObject HunMultiplieBtn;


    public void OnOneMultiplieBtn()
    {
        PlayerObjManager.Instance.Player.stat.ChangeUpgradeMultiplier(1);
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        PlayerObjManager.Instance.Player.stat.UpdateAllStatUI.Invoke();
        OneMultiplieBtn.SetActive(false);
        TenMultiplieBtn.SetActive(true);
    }

    public void OnTenMultiplieBtn()
    {
        PlayerObjManager.Instance.Player.stat.ChangeUpgradeMultiplier(2);
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        PlayerObjManager.Instance.Player.stat.UpdateAllStatUI.Invoke();
        TenMultiplieBtn.SetActive(false);
        HunMultiplieBtn.SetActive(true);
    }

    public void OnHunMultiplieBtn()
    {
        PlayerObjManager.Instance.Player.stat.ChangeUpgradeMultiplier(0);
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        PlayerObjManager.Instance.Player.stat.UpdateAllStatUI.Invoke();
        HunMultiplieBtn.SetActive(false);
        OneMultiplieBtn.SetActive(true);
    }

}
