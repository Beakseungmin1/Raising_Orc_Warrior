using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgrademultiplieBtn : MonoBehaviour
{
    public TextMeshProUGUI MultiplieTxt;
    private PlayerStat stat;

    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;
        stat.UpdateAllStatUI += UpdateBtnUI;
    }

    public void OnMultiplieBtn()
    {
        if (stat.statUpgradeMultiplier == 0)
        {
            stat.ChangeUpgradeMultiplier(1);
            stat.UpdateNeedMoney();
            stat.UpdateAllStatUI.Invoke();
        }
        else if (stat.statUpgradeMultiplier == 1)
        {
            stat.ChangeUpgradeMultiplier(2);
            stat.UpdateNeedMoney();
            stat.UpdateAllStatUI.Invoke();
        }
        else
        {
            stat.ChangeUpgradeMultiplier(0);
            stat.UpdateNeedMoney();
            stat.UpdateAllStatUI.Invoke();
        }
    }

    public void UpdateBtnUI()
    {
        if (PlayerObjManager.Instance.Player.stat.statUpgradeMultiplier == 0)
        {
            MultiplieTxt.text = "1x";
        }
        else if (PlayerObjManager.Instance.Player.stat.statUpgradeMultiplier == 1)
        {
            MultiplieTxt.text = "10x";
        }
        else
        {
            MultiplieTxt.text = "100x";
        }
    }

}
