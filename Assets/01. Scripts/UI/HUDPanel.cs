using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : UIBase
{
    private PlayerStat stat;

    [SerializeField] private TextMeshProUGUI playerNameTxt;
    [SerializeField] private TextMeshProUGUI playerlevelTxt;
    [SerializeField] private TextMeshProUGUI expTxt;
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI emeraldTxt;
    [SerializeField] private TextMeshProUGUI diamondTxt;




    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;

        stat.UpdateLevelStatUI += UpdateTopInformatinBar;
        stat.UpdateLevelStatUI?.Invoke();
    }

    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<PlayerInfoPopupUI>();
        stat.UpdateUserInformationUI?.Invoke();
    }

    public void UpdateTopInformatinBar()
    {
        if (stat != null)
        {
            playerlevelTxt.text = "Lv." + stat.level.ToString();

            BigInteger currentExp = stat.exp;
            BigInteger needExp = stat.needExp;

            //현재 경험치가 총경험치의 몇% 인지 계산
            float percentage = needExp > 0 ? (float)(currentExp / needExp) * 100 : 0;

            //경험치 표시가 100% 을 넘어가지 못하게함
            float ExppercentTxt = percentage >= 100 ? 100 : percentage;

            expBar.value = percentage / 100;

            expTxt.text = ExppercentTxt.ToString("F2");

        }
    }




}
